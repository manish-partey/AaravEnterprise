using AaravEnterprise.DataAccess;
using AaravEnterprise.Models;
using AaravEnterprise.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using PayPalCheckoutSdk.Core;
using PayPalCheckoutSdk.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AaravEnterprise.Controllers
{
    [Authorize]
    public class PaypalPaymentController : Controller
    {        
        public IConfiguration Configuration { get; }
        private readonly ApplicationDbContext _dbContext;
        Cart cart;
        public PaypalPaymentController(IConfiguration configuration, ApplicationDbContext dbContext)
        {
            Configuration = configuration;
            _dbContext = dbContext;
        }
        
        public ActionResult Index()
        {
            ViewBag.UseAlternateLayout = RouteData.Values["controller"].ToString() == "";
            return View();
        }

        public async Task<ActionResult> Paypalvtwo(string Cancel = null)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            var query = from C in _dbContext.Cart
                        join S in _dbContext.Services on C.ServiceId equals S.Id
                        join P in _dbContext.Package on C.PackageId equals P.Id
                        where C.ApplicationUserId == userId
                        select new CartViewModel
                        {
                            CartId = C.CartId,
                            ServiceTitle = S.ServiceTitle,
                            PackageTitle = P.PackageTitle,
                            Amount = C.Amount
                        };
            ViewBag.CartItemsForUser = query.ToList();
            var total = query.Sum(p => p.Amount);
            ViewBag.Total = total;

            #region local_variables
            MyPaypalPayment.MyPaypalSetup payPalSetup
                = new MyPaypalPayment.MyPaypalSetup { Environment = Configuration.GetSection("PayPal:Mode").Value.ToString(), ClientId = Configuration.GetSection("PayPal:ClientId").Value.ToString(), Secret = Configuration.GetSection("PayPal:Secret").Value.ToString() };
            
            List<string> paymentResultList = new List<string>();
            #endregion

            #region check_payment_cancellation            
            if (!string.IsNullOrEmpty(Cancel) && Cancel.Trim().ToLower() == "true")
            {
                paymentResultList.Add("You cancelled the transaction.");
                ViewBag.UseAlternateLayout = RouteData.Values["controller"].ToString() == "";
                return View("PaymentDetails", paymentResultList);
            }

            #endregion

            payPalSetup.PayerApprovedOrderId = Request.Query["token"];
            string PayerID = Request.Query["PayerID"];

            //when payerID is null it means order is not approved by the payer.            
            if (string.IsNullOrEmpty(PayerID))
            {
                #region order_creation
                //Create order and display it to the payer to approve. 
                //This is the first PayPal screen where payer signin using his PayPal credentials
                try
                {
                    //redirect URL. when approved or cancelled on PayPal, PayPal uses this URL to redirect to your app/website.
                    payPalSetup.RedirectUrl = Request.Scheme + "://" + Request.Host + "/PaypalPayment/Paypalvtwo?";
                    MyPaypalPayment myPaypalPayment = new MyPaypalPayment();
                    var cartViewModels = (List<CartViewModel>)ViewBag.CartItemsForUser;
                    string ordertotal = Convert.ToString(ViewBag.Total);
                    PayPalHttp.HttpResponse response = await myPaypalPayment.createOrder(payPalSetup, cartViewModels, ordertotal);

                    var statusCode = response.StatusCode;
                    PayPalCheckoutSdk.Orders.Order result = response.Result<PayPalCheckoutSdk.Orders.Order>();
                    foreach (PayPalCheckoutSdk.Orders.LinkDescription link in result.Links)
                    {                        
                        if (link.Rel.Trim().ToLower() == "approve")
                        {
                            payPalSetup.ApproveUrl = link.Href;
                        }
                    }

                    if (!string.IsNullOrEmpty(payPalSetup.ApproveUrl))
                        return Redirect(payPalSetup.ApproveUrl);
                }
                catch (Exception ex)
                {
                    paymentResultList.Add("There was an error in processing your payment");
                    paymentResultList.Add("Details: " + ex.Message);
                }

                #endregion
            }
            else
            {
                #region order_execution
                MyPaypalPayment myPaypalPayment = new MyPaypalPayment();
                //this is where actual transaction is carried out
                PayPalHttp.HttpResponse response = await myPaypalPayment.captureOrder(payPalSetup);
                try
                {
                    var statusCode = response.StatusCode;
                    PayPalCheckoutSdk.Orders.Order result = response.Result<PayPalCheckoutSdk.Orders.Order>();

                    //update view bag so user/payer gets to know the status
                    if (result.Status.Trim().ToUpper() == "COMPLETED")
                        paymentResultList.Add("Payment Successful. Thank you.");
                    paymentResultList.Add("Payment State: " + result.Status);
                    paymentResultList.Add("Payment ID: " + result.Id);

                    #region null_checks
                    if (result.PurchaseUnits != null && result.PurchaseUnits.Count > 0 &&
                        result.PurchaseUnits[0].Payments != null && result.PurchaseUnits[0].Payments.Captures != null &&
                        result.PurchaseUnits[0].Payments.Captures.Count > 0)
                        #endregion
                        paymentResultList.Add("Transaction ID: " + result.PurchaseUnits[0].Payments.Captures[0].Id);
                }
                catch (Exception ex)
                {
                    paymentResultList.Add("There was an error in processing your payment");
                    paymentResultList.Add("Details: " + ex.Message);
                }

                #endregion

            }
            ViewBag.UseAlternateLayout = RouteData.Values["controller"].ToString() == "";
            return View("PaymentDetails", paymentResultList);
        }



        /// <summary>
        /// This is your own custom class. NOT from paypal SDK. Write it the way you want it
        /// </summary>
        public class MyPaypalPayment
        {
            List<CartViewModel> cartItemList;
            /// <summary>
            /// Initiates Paypal client. Must ensure correct environment.
            /// </summary>
            /// <param name="paypalEnvironment">provide value sandbox for testing,  provide value live for live environments</param>            
            /// <returns>PayPalHttp.HttpClient</returns>
            public PayPalHttpClient client(MyPaypalSetup paypalEnvironment)
            {
                PayPalEnvironment environment = null;

                if (paypalEnvironment.Environment == "live")
                {
                    // Creating a live environment
                    environment = new LiveEnvironment(paypalEnvironment.ClientId, paypalEnvironment.Secret);
                }
                else
                {
                    // Creating a sandbox environment
                    environment = new SandboxEnvironment(paypalEnvironment.ClientId, paypalEnvironment.Secret);
                }

                // Creating a client for the environment
                PayPalHttpClient client = new PayPalHttpClient(environment);
                return client;
            }


            //### Creating an Order
            //This will create an order and print order id for the created order

            public async Task<PayPalHttp.HttpResponse> createOrder(MyPaypalSetup paypalSetup, List<CartViewModel> cartViewModels, string orderTotal)
            {
                PayPalHttp.HttpResponse response = null;
                cartItemList = cartViewModels;
                var itemList = new List<Item>();
                foreach (var cartItem in cartItemList)
                {
                    itemList.Add(new Item
                    {
                        Quantity = "1",
                        Name = cartItem.PackageTitle,
                        Description = cartItem.ServiceTitle,
                        Sku = "sku",
                        Tax = new Money() { CurrencyCode = "USD", Value = "0.00" },
                        UnitAmount = new Money
                        {
                            CurrencyCode = "USD",
                            Value = cartItem.Amount.ToString("0.##")
                        }
                    });
                }

                try
                {
                    // Construct a request object and set desired parameters
                    // Here, OrdersCreateRequest() creates a POST request to /v2/checkout/orders
                    #region order_creation
                    var order = new OrderRequest()
                    {
                        CheckoutPaymentIntent = "CAPTURE",
                        PurchaseUnits = new List<PurchaseUnitRequest>()
                        {
                            new PurchaseUnitRequest()
                            {
                               Items = itemList,
                               AmountWithBreakdown = new AmountWithBreakdown()
                                {
                                    CurrencyCode = "USD",
                                    Value = orderTotal,

                                    AmountBreakdown = new AmountBreakdown()
                                    {
                                        TaxTotal = new PayPalCheckoutSdk.Orders.Money()
                                        {
                                            CurrencyCode = "USD",
                                            Value = "0.00"
                                        },                                       
                                        ItemTotal = new PayPalCheckoutSdk.Orders.Money()
                                        {
                                            CurrencyCode = "USD",
                                            Value = orderTotal
                                        }
                                    }
                                }
                            }
                        },
                        ApplicationContext = new ApplicationContext()
                        {
                            ReturnUrl = paypalSetup.RedirectUrl,
                            CancelUrl = paypalSetup.RedirectUrl + "&Cancel=true"
                        }
                    };

                    #endregion

                    //IMPORTANT
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                    // Call API with your client and get a response for your call
                    var request = new OrdersCreateRequest();
                    request.Prefer("return=representation");
                    request.RequestBody(order);
                    PayPalHttpClient paypalHttpClient = client(paypalSetup);
                    response = await paypalHttpClient.Execute(request);

                }
                catch (Exception ex)
                {   
                    throw;
                }
                return response;
            }


            //### Capturing an Order
            //Before capturing an order, order should be approved by the buyer using the approve link in create order response

            public async Task<PayPalHttp.HttpResponse> captureOrder(MyPaypalSetup paypalSetup)
            {
                // Construct a request object and set desired parameters
                // Replace ORDER-ID with the approved order id from create order
                var request = new OrdersCaptureRequest(paypalSetup.PayerApprovedOrderId);
                request.RequestBody(new OrderActionRequest());
                PayPalHttpClient paypalHttpClient = client(paypalSetup);
                PayPalHttp.HttpResponse response = await paypalHttpClient.Execute(request);
                return response;
            }

            public class MyPaypalSetup
            {
                /// <summary>
                /// Provide value sandbox for testing,  provide value live for real money
                /// </summary>
                public String Environment { get; set; }
                /// <summary>
                /// Client id as provided by Paypal on dashboard. Ensure you use correct value based on your environment selection
                /// Use sandbox accounts for sandbox testing
                /// </summary>
                public String ClientId { get; set; }
                /// <summary>
                /// Secret as provided by Paypal on dashboard. Ensure you use correct value based on your environment selection
                /// Use sandbox accounts for sandbox testing
                /// </summary>
                public String Secret { get; set; }

                /// <summary>
                /// This is the URL that you will pass to paypal which paypal will use to redirect payer back to your website.
                /// So essentially it is the same controller URL that you must pass
                /// </summary>
                public String RedirectUrl { get; set; }

                /// <summary>
                /// Once order is created on Paypal, it redirects control to your app with a URL that shows order details. Your website must take the payer to this page
                /// so the payer approved the payment. Store this URL in this property
                /// </summary>
                public String ApproveUrl { get; set; }

                /// <summary>
                /// When paypal redirects control to your website it provides a Approved Order ID which we then pass it back to paypal to execute the order.
                /// Store this approved order ID in this property
                /// </summary>
                public String PayerApprovedOrderId { get; set; }
            }

        }
    }
}
