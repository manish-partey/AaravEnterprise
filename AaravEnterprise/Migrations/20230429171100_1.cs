using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AaravEnterprise.Migrations
{
    public partial class _1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ApplicationUser",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    UserName = table.Column<string>(nullable: true),
                    NormalizedUserName = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    NormalizedEmail = table.Column<string>(nullable: true),
                    EmailConfirmed = table.Column<bool>(nullable: false),
                    PasswordHash = table.Column<string>(nullable: true),
                    SecurityStamp = table.Column<string>(nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(nullable: false),
                    TwoFactorEnabled = table.Column<bool>(nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(nullable: true),
                    LockoutEnabled = table.Column<bool>(nullable: false),
                    AccessFailedCount = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    StreetAddress = table.Column<string>(nullable: true),
                    City = table.Column<string>(nullable: true),
                    State = table.Column<string>(nullable: true),
                    PostalCode = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationUser", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Order",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(nullable: true),
                    OrderDate = table.Column<DateTime>(nullable: false),
                    TotalAmount = table.Column<double>(nullable: false),
                    OrderStatus = table.Column<string>(nullable: true),
                    PaymentStatus = table.Column<string>(nullable: true),
                    PaymentDate = table.Column<DateTime>(nullable: false),
                    PaymentId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Order", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Services",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ServiceTitle = table.Column<string>(nullable: true),
                    ServiceDescription = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Services", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OrderDetails",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderId = table.Column<int>(nullable: true),
                    ServiceId = table.Column<int>(nullable: true),
                    Quantity = table.Column<int>(nullable: false),
                    Price = table.Column<double>(nullable: false),
                    Total = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderDetails_Order_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Order",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OrderDetails_Services_ServiceId",
                        column: x => x.ServiceId,
                        principalTable: "Services",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Package",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PackageTitle = table.Column<string>(nullable: true),
                    PackageDescription = table.Column<string>(nullable: true),
                    Price = table.Column<int>(nullable: false),
                    ServicesId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Package", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Package_Services_ServicesId",
                        column: x => x.ServicesId,
                        principalTable: "Services",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Cart",
                columns: table => new
                {
                    CartId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ServiceId = table.Column<int>(nullable: false),
                    PackageId = table.Column<int>(nullable: false),
                    Amount = table.Column<int>(nullable: false),
                    ApplicationUserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cart", x => x.CartId);
                    table.ForeignKey(
                        name: "FK_Cart_ApplicationUser_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "ApplicationUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Cart_Package_PackageId",
                        column: x => x.PackageId,
                        principalTable: "Package",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Cart_Services_ServiceId",
                        column: x => x.ServiceId,
                        principalTable: "Services",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.InsertData(
                table: "Services",
                columns: new[] { "Id", "ServiceDescription", "ServiceTitle" },
                values: new object[,]
                {
                    { 1, @"At Aarav Web Solution, UI/UX stands for adding life to your basic software applications with beautiful visuals and crafts.

                The user experience is more than just how simple and appealing the application is to use. UX includes visual design, content, user experience, accessibility, capabilities, and context to provide a satisfactory interaction for a pleasant user experience.

                It requires us to gather and evaluate our user experience. We construct aesthetically appealing scenarios and analyze them with actual build environment users. We use a variety of tools to evaluate product usability. Moreover, UI/UX for us is Innovation and Storytelling.", "UX/UI Design" },
                    { 2, @"Digital marketing is the art of selling your products and services to a specific target audience over the internet. It is the heart of any online business today. While we talk about traditional marketing, it is comparably more expensive and time-consuming.

                We at Aarav Web Solution consider digital marketing to be a wide-ranging field of marketing.

                As a result, we offer various digital marketing services that include the use of websites, mobile devices, social media, search engines, and apps—basically, anything that combines marketing with user feedback or two-way communication between the company and the customer.

                So, let your business grow through digital marketing with Aarav Web Solution.", "Media Marketing" },
                    { 3, @"Over time, the phrase ""content writing"" has spread like wildfire. You might not realize it, but content can be found in everything including what you listen to, see and read here.

                In our opinion, businesses can create emotional connections with their target market through the use of content creation. This led to increased brand engagement and consumer loyalty. 

                Thus, at Aarav Web Solution our dedicated content writer creates content materials to attract users to a particular business, product, or online website. So, as per our client's requirements, we help them in creating and writing blogs, emails, newsletters, white papers, video scripts, and many other types of content.", "Content Solution" },
                    { 4, @"We give strategic solutions to website design and development.

                In the highly competitive world of web development, it is crucial to understand each type of web development and its underlying technology. A website's development is a challenging task, especially if it is done from scratch.

                Aarav Web Solution can make the process easier whether you're creating a small business website, an online store, or something else. Our web development services are centered on providing value that characterizes the online interactions between our clients and their customers.

                We design WordPress websites and create front and back-end code for websites in a choice of coding languages. In addition, we provide our finest support in ongoing website analysis to let you know how your website is performing. If desired, we can additionally provide speed optimization from the standpoint of our SEO expertise.

                We build a user-friendly website for you that helps to build your brand, appear like a pro, and move up the search engine results pages. From getting domain names to adding monthly analytics to your website, we are here for you.

                So, explore all of the ways we may help you create a more efficient website and make your business online!", "Web Development" },
                    { 5, @"Social Media Marketing is far more diverse than anyone could have imagined. Simply said, social marketing is the use of social media channels to advertise and sell your brand's product or service. Similarly to how you would organize other aspects of your marketing strategy.

                However, you must have a plan and a team that can work together to achieve the intended results. To be more precise, social marketing is more than just posting, liking, and commenting. Each post should grab your customers' interest and deserve one share.

                And at Aarav Web Solution, we endeavor to make every like a virtual action. We generate leads, raise brand awareness, expand your brand's audience, and much more.", "Social Media Management" },
                    { 6, "", "Upcoming Services" }
                });

            migrationBuilder.InsertData(
                table: "Package",
                columns: new[] { "Id", "PackageDescription", "PackageTitle", "Price", "ServicesId" },
                values: new object[,]
                {
                    { 1, @"Wireframing
                Mockups
                Logo and color scheme selection with 5 changes
                Web banners with 2 options
                Infographics with 2 options
                Posters with 2 options", "Basic", 249, 1 },
                    { 2, @"Wireframing
                Mockups
                Web banners with 4 options
                Infographics with 4 options
                Posters with 4 options", "Standard", 369, 1 },
                    { 3, @"Wireframing
                Mockups
                Web banners with 7 options
                Infographics with 7 options
                Posters with 7 options
                Mobile app development with customers changes
                Website marketing", "Ultimate", 589, 1 },
                    { 4, @"Google Ads
                Bing Advertising
                Search engine optimization
                Retargeting
                Web analytics", "Basic", 349, 2 },
                    { 5, @"Google Ads
                Bing Advertising and analysis
                Retargeting
                Google ads reports
                Digital media planning and strategy", "Standard", 549, 2 },
                    { 6, @"Google Ads
                Bing advertising and analysis
                Social media Ads
                Google ads reports
                Digital media planning
                Digital strategy consulting
                Digital marketing campaigns", "Ultimate", 749, 2 },
                    { 7, @"Keyword research
                2 Blog posts with up 1000 words
                Website Content Writing
                Research and development
                Proofreading services", "Basic", 169, 3 },
                    { 8, @"Research and development
                Keyword research
                4 blog posts with up to 2000 words
                Technical writing
                Branding content
                Academic writing services
                Proofreading services
                Content marketing", "Standard", 319, 3 },
                    { 9, @"Research and development
                Keyword research
                4 blog posts with up to 2000 words
                Newsletter weekly
                Infographics content
                Measuring and tracking the performance
                Technical writing
                Branding content
                Academic writing services
                Proofreading services
                Content marketing and analysis", "Ultimate", 599, 3 },
                    { 10, @"Managing 2 social media platforms (Facebook and Instagram)
                Creating and posting 3 social media posts weekly
                Facebook Ads
                Instagram Ads
                Customer Targeting", "Basic", 319, 4 },
                    { 11, @"Managing 5 social media platforms (Facebook, Instagram, Pinterest, Twitter, and Linkedin)
                Creating and posting 5 social media posts weekly
                Facebook Ads, Instagram Ads, Pinterest Ads, Twitter Ads, Linkedin Ads
                Organic Social media traffic
                Creative Assistant", "Standard", 449, 4 },
                    { 12, @"Managing all social media platforms to your specifications.
                Creating and posting 7 social media posts weekly
                Social media Ads for all platforms
                Organic social media traffic
                Creative Assistant
                Marketing CRMis ", "Ultimate", 719, 4 },
                    { 13, @"Managing 2 social media platforms (Facebook and Instagram)
                Creating and posting 3 social media posts weekly
                Facebook Ads
                Instagram Ads
                Customer Targeting", "Basic", 319, 5 },
                    { 14, @"Managing 5 social media platforms (Facebook, Instagram, Pinterest, Twitter, and Linkedin)
                Creating and posting 5 social media posts weekly
                Facebook Ads, Instagram Ads, Pinterest Ads, Twitter Ads, Linkedin Ads
                Organic Social media traffic
                Creative Assistant", "Standard", 449, 5 },
                    { 15, @"Managing all social media platforms to your specifications.
                Creating and posting 7 social media posts weekly
                Social media Ads for all platforms
                Organic social media traffic
                Creative Assistant
                Marketing CRMis ", "Ultimate", 719, 5 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Cart_ApplicationUserId",
                table: "Cart",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Cart_PackageId",
                table: "Cart",
                column: "PackageId");

            migrationBuilder.CreateIndex(
                name: "IX_Cart_ServiceId",
                table: "Cart",
                column: "ServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetails_OrderId",
                table: "OrderDetails",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetails_ServiceId",
                table: "OrderDetails",
                column: "ServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_Package_ServicesId",
                table: "Package",
                column: "ServicesId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Cart");

            migrationBuilder.DropTable(
                name: "OrderDetails");

            migrationBuilder.DropTable(
                name: "ApplicationUser");

            migrationBuilder.DropTable(
                name: "Package");

            migrationBuilder.DropTable(
                name: "Order");

            migrationBuilder.DropTable(
                name: "Services");
        }
    }
}
