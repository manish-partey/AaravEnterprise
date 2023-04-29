using Microsoft.EntityFrameworkCore;
using AaravEnterprise.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace AaravEnterprise.DataAccess
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        public DbSet<Cart> Cart { get; set; }
        public DbSet<Order> Order { get; set; }
        public DbSet<OrderDetails> OrderDetails { get; set; }
        public DbSet<Package> Package { get; set; }
        public DbSet<Services> Services { get; set; }
        public DbSet<ApplicationUser> ApplicationUser { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Services>().HasData(
                new Services { Id = 1, ServiceTitle = "UX/UI Design", ServiceDescription = "At Aarav Web Solution, UI/UX stands for adding life to your basic software applications with beautiful visuals and crafts.\r\n\r\nThe user experience is more than just how simple and appealing the application is to use. UX includes visual design, content, user experience, accessibility, capabilities, and context to provide a satisfactory interaction for a pleasant user experience.\r\n\r\nIt requires us to gather and evaluate our user experience. We construct aesthetically appealing scenarios and analyze them with actual build environment users. We use a variety of tools to evaluate product usability. Moreover, UI/UX for us is Innovation and Storytelling." },
                new Services { Id = 2, ServiceTitle = "Media Marketing", ServiceDescription = "Digital marketing is the art of selling your products and services to a specific target audience over the internet. It is the heart of any online business today. While we talk about traditional marketing, it is comparably more expensive and time-consuming.\r\n\r\nWe at Aarav Web Solution consider digital marketing to be a wide-ranging field of marketing.\r\n\r\nAs a result, we offer various digital marketing services that include the use of websites, mobile devices, social media, search engines, and apps—basically, anything that combines marketing with user feedback or two-way communication between the company and the customer.\r\n\r\nSo, let your business grow through digital marketing with Aarav Web Solution." },
                new Services { Id = 3, ServiceTitle = "Content Solution", ServiceDescription = "Over time, the phrase \"content writing\" has spread like wildfire. You might not realize it, but content can be found in everything including what you listen to, see and read here.\r\n\r\nIn our opinion, businesses can create emotional connections with their target market through the use of content creation. This led to increased brand engagement and consumer loyalty. \r\n\r\nThus, at Aarav Web Solution our dedicated content writer creates content materials to attract users to a particular business, product, or online website. So, as per our client's requirements, we help them in creating and writing blogs, emails, newsletters, white papers, video scripts, and many other types of content." },
                new Services { Id = 4, ServiceTitle = "Web Development", ServiceDescription = "We give strategic solutions to website design and development.\r\n\r\nIn the highly competitive world of web development, it is crucial to understand each type of web development and its underlying technology. A website's development is a challenging task, especially if it is done from scratch.\r\n\r\nAarav Web Solution can make the process easier whether you're creating a small business website, an online store, or something else. Our web development services are centered on providing value that characterizes the online interactions between our clients and their customers.\r\n\r\nWe design WordPress websites and create front and back-end code for websites in a choice of coding languages. In addition, we provide our finest support in ongoing website analysis to let you know how your website is performing. If desired, we can additionally provide speed optimization from the standpoint of our SEO expertise.\r\n\r\nWe build a user-friendly website for you that helps to build your brand, appear like a pro, and move up the search engine results pages. From getting domain names to adding monthly analytics to your website, we are here for you.\r\n\r\nSo, explore all of the ways we may help you create a more efficient website and make your business online!" },
                new Services { Id = 5, ServiceTitle = "Social Media Management", ServiceDescription = "Social Media Marketing is far more diverse than anyone could have imagined. Simply said, social marketing is the use of social media channels to advertise and sell your brand's product or service. Similarly to how you would organize other aspects of your marketing strategy.\r\n\r\nHowever, you must have a plan and a team that can work together to achieve the intended results. To be more precise, social marketing is more than just posting, liking, and commenting. Each post should grab your customers' interest and deserve one share.\r\n\r\nAnd at Aarav Web Solution, we endeavor to make every like a virtual action. We generate leads, raise brand awareness, expand your brand's audience, and much more." },
                new Services { Id = 6, ServiceTitle = "Upcoming Services", ServiceDescription = "" }
                );


            modelBuilder.Entity<Package>().HasData(
               new Package { Id = 1, PackageTitle = "Basic", PackageDescription = "Wireframing\r\nMockups\r\nLogo and color scheme selection with 5 changes\r\nWeb banners with 2 options\r\nInfographics with 2 options\r\nPosters with 2 options", Price = 249, ServicesId = 1 },
               new Package { Id = 2, PackageTitle = "Standard", PackageDescription = "Wireframing\r\nMockups\r\nWeb banners with 4 options\r\nInfographics with 4 options\r\nPosters with 4 options", Price = 369, ServicesId = 1 },
               new Package { Id = 3, PackageTitle = "Ultimate", PackageDescription = "Wireframing\r\nMockups\r\nWeb banners with 7 options\r\nInfographics with 7 options\r\nPosters with 7 options\r\nMobile app development with customers changes\r\nWebsite marketing", Price = 589, ServicesId = 1 },
               new Package { Id = 4, PackageTitle = "Basic", PackageDescription = "Google Ads\r\nBing Advertising\r\nSearch engine optimization\r\nRetargeting\r\nWeb analytics", Price = 349, ServicesId = 2 },
               new Package { Id = 5, PackageTitle = "Standard", PackageDescription = "Google Ads\r\nBing Advertising and analysis\r\nRetargeting\r\nGoogle ads reports\r\nDigital media planning and strategy", Price = 549, ServicesId = 2 },
               new Package { Id = 6, PackageTitle = "Ultimate", PackageDescription = "Google Ads\r\nBing advertising and analysis\r\nSocial media Ads\r\nGoogle ads reports\r\nDigital media planning\r\nDigital strategy consulting\r\nDigital marketing campaigns", Price = 749, ServicesId = 2 },
               new Package { Id = 7, PackageTitle = "Basic", PackageDescription = "Keyword research\r\n2 Blog posts with up 1000 words\r\nWebsite Content Writing\r\nResearch and development\r\nProofreading services", Price = 169, ServicesId = 3 },
               new Package { Id = 8, PackageTitle = "Standard", PackageDescription = "Research and development\r\nKeyword research\r\n4 blog posts with up to 2000 words\r\nTechnical writing\r\nBranding content\r\nAcademic writing services\r\nProofreading services\r\nContent marketing", Price = 319, ServicesId = 3 },
               new Package { Id = 9, PackageTitle = "Ultimate", PackageDescription = "Research and development\r\nKeyword research\r\n4 blog posts with up to 2000 words\r\nNewsletter weekly\r\nInfographics content\r\nMeasuring and tracking the performance\r\nTechnical writing\r\nBranding content\r\nAcademic writing services\r\nProofreading services\r\nContent marketing and analysis", Price = 599, ServicesId = 3 },
               new Package { Id = 10, PackageTitle = "Basic", PackageDescription = "Managing 2 social media platforms (Facebook and Instagram)\r\nCreating and posting 3 social media posts weekly\r\nFacebook Ads\r\nInstagram Ads\r\nCustomer Targeting", Price = 319, ServicesId = 4 },
               new Package { Id = 11, PackageTitle = "Standard", PackageDescription = "Managing 5 social media platforms (Facebook, Instagram, Pinterest, Twitter, and Linkedin)\r\nCreating and posting 5 social media posts weekly\r\nFacebook Ads, Instagram Ads, Pinterest Ads, Twitter Ads, Linkedin Ads\r\nOrganic Social media traffic\r\nCreative Assistant", Price = 449, ServicesId = 4 },
               new Package { Id = 12, PackageTitle = "Ultimate", PackageDescription = "Managing all social media platforms to your specifications.\r\nCreating and posting 7 social media posts weekly\r\nSocial media Ads for all platforms\r\nOrganic social media traffic\r\nCreative Assistant\r\nMarketing CRMis ", Price = 719, ServicesId = 4 },
               new Package { Id = 13, PackageTitle = "Basic", PackageDescription = "Managing 2 social media platforms (Facebook and Instagram)\r\nCreating and posting 3 social media posts weekly\r\nFacebook Ads\r\nInstagram Ads\r\nCustomer Targeting", Price = 319, ServicesId = 5 },
               new Package { Id = 14, PackageTitle = "Standard", PackageDescription = "Managing 5 social media platforms (Facebook, Instagram, Pinterest, Twitter, and Linkedin)\r\nCreating and posting 5 social media posts weekly\r\nFacebook Ads, Instagram Ads, Pinterest Ads, Twitter Ads, Linkedin Ads\r\nOrganic Social media traffic\r\nCreative Assistant", Price = 449, ServicesId = 5 },
               new Package { Id = 15, PackageTitle = "Ultimate", PackageDescription = "Managing all social media platforms to your specifications.\r\nCreating and posting 7 social media posts weekly\r\nSocial media Ads for all platforms\r\nOrganic social media traffic\r\nCreative Assistant\r\nMarketing CRMis ", Price = 719, ServicesId = 5 }
               );
        }
    }
}
