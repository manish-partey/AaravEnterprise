﻿// <auto-generated />
using System;
using AaravEnterprise.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace AaravEnterprise.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20230622061434_OrderDetailsPK1")]
    partial class OrderDetailsPK1
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.32")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("AaravEnterprise.Models.Cart", b =>
                {
                    b.Property<int>("CartId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Amount")
                        .HasColumnType("int");

                    b.Property<string>("ApplicationUserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("PackageId")
                        .HasColumnType("int");

                    b.Property<int>("ServiceId")
                        .HasColumnType("int");

                    b.HasKey("CartId");

                    b.HasIndex("ApplicationUserId");

                    b.HasIndex("PackageId");

                    b.HasIndex("ServiceId");

                    b.ToTable("Cart");
                });

            modelBuilder.Entity("AaravEnterprise.Models.Order", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("OrderDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("OrderStatus")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("PaymentDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("PaymentId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PaymentStatus")
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("TotalAmount")
                        .HasColumnType("float");

                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Order");
                });

            modelBuilder.Entity("AaravEnterprise.Models.OrderDetails", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("OrderId")
                        .HasColumnType("int");

                    b.Property<double>("Price")
                        .HasColumnType("float");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.Property<int?>("ServiceId")
                        .HasColumnType("int");

                    b.Property<double>("Total")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.ToTable("OrderDetails");
                });

            modelBuilder.Entity("AaravEnterprise.Models.Package", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("PackageDescription")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PackageTitle")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Price")
                        .HasColumnType("int");

                    b.Property<int>("ServicesId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ServicesId");

                    b.ToTable("Package");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            PackageDescription = @"Wireframing
Mockups
Logo and color scheme selection with 5 changes
Web banners with 2 options
Infographics with 2 options
Posters with 2 options",
                            PackageTitle = "Basic",
                            Price = 249,
                            ServicesId = 1
                        },
                        new
                        {
                            Id = 2,
                            PackageDescription = @"Wireframing
Mockups
Web banners with 4 options
Infographics with 4 options
Posters with 4 options",
                            PackageTitle = "Standard",
                            Price = 369,
                            ServicesId = 1
                        },
                        new
                        {
                            Id = 3,
                            PackageDescription = @"Wireframing
Mockups
Web banners with 7 options
Infographics with 7 options
Posters with 7 options
Mobile app development with customers changes
Website marketing",
                            PackageTitle = "Ultimate",
                            Price = 589,
                            ServicesId = 1
                        },
                        new
                        {
                            Id = 4,
                            PackageDescription = @"Google Ads
Bing Advertising
Search engine optimization
Retargeting
Web analytics",
                            PackageTitle = "Basic",
                            Price = 349,
                            ServicesId = 2
                        },
                        new
                        {
                            Id = 5,
                            PackageDescription = @"Google Ads
Bing Advertising and analysis
Retargeting
Google ads reports
Digital media planning and strategy",
                            PackageTitle = "Standard",
                            Price = 549,
                            ServicesId = 2
                        },
                        new
                        {
                            Id = 6,
                            PackageDescription = @"Google Ads
Bing advertising and analysis
Social media Ads
Google ads reports
Digital media planning
Digital strategy consulting
Digital marketing campaigns",
                            PackageTitle = "Ultimate",
                            Price = 749,
                            ServicesId = 2
                        },
                        new
                        {
                            Id = 7,
                            PackageDescription = @"Keyword research
2 Blog posts with up 1000 words
Website Content Writing
Research and development
Proofreading services",
                            PackageTitle = "Basic",
                            Price = 169,
                            ServicesId = 3
                        },
                        new
                        {
                            Id = 8,
                            PackageDescription = @"Research and development
Keyword research
4 blog posts with up to 2000 words
Technical writing
Branding content
Academic writing services
Proofreading services
Content marketing",
                            PackageTitle = "Standard",
                            Price = 319,
                            ServicesId = 3
                        },
                        new
                        {
                            Id = 9,
                            PackageDescription = @"Research and development
Keyword research
4 blog posts with up to 2000 words
Newsletter weekly
Infographics content
Measuring and tracking the performance
Technical writing
Branding content
Academic writing services
Proofreading services
Content marketing and analysis",
                            PackageTitle = "Ultimate",
                            Price = 599,
                            ServicesId = 3
                        },
                        new
                        {
                            Id = 10,
                            PackageDescription = @"Managing 2 social media platforms (Facebook and Instagram)
Creating and posting 3 social media posts weekly
Facebook Ads
Instagram Ads
Customer Targeting",
                            PackageTitle = "Basic",
                            Price = 319,
                            ServicesId = 4
                        },
                        new
                        {
                            Id = 11,
                            PackageDescription = @"Managing 5 social media platforms (Facebook, Instagram, Pinterest, Twitter, and Linkedin)
Creating and posting 5 social media posts weekly
Facebook Ads, Instagram Ads, Pinterest Ads, Twitter Ads, Linkedin Ads
Organic Social media traffic
Creative Assistant",
                            PackageTitle = "Standard",
                            Price = 449,
                            ServicesId = 4
                        },
                        new
                        {
                            Id = 12,
                            PackageDescription = @"Managing all social media platforms to your specifications.
Creating and posting 7 social media posts weekly
Social media Ads for all platforms
Organic social media traffic
Creative Assistant
Marketing CRMis ",
                            PackageTitle = "Ultimate",
                            Price = 719,
                            ServicesId = 4
                        },
                        new
                        {
                            Id = 13,
                            PackageDescription = @"Managing 2 social media platforms (Facebook and Instagram)
Creating and posting 3 social media posts weekly
Facebook Ads
Instagram Ads
Customer Targeting",
                            PackageTitle = "Basic",
                            Price = 319,
                            ServicesId = 5
                        },
                        new
                        {
                            Id = 14,
                            PackageDescription = @"Managing 5 social media platforms (Facebook, Instagram, Pinterest, Twitter, and Linkedin)
Creating and posting 5 social media posts weekly
Facebook Ads, Instagram Ads, Pinterest Ads, Twitter Ads, Linkedin Ads
Organic Social media traffic
Creative Assistant",
                            PackageTitle = "Standard",
                            Price = 449,
                            ServicesId = 5
                        },
                        new
                        {
                            Id = 15,
                            PackageDescription = @"Managing all social media platforms to your specifications.
Creating and posting 7 social media posts weekly
Social media Ads for all platforms
Organic social media traffic
Creative Assistant
Marketing CRMis ",
                            PackageTitle = "Ultimate",
                            Price = 719,
                            ServicesId = 5
                        });
                });

            modelBuilder.Entity("AaravEnterprise.Models.Services", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ServiceDescription")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ServiceTitle")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Services");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            ServiceDescription = @"At Aarav Web Solution, UI/UX stands for adding life to your basic software applications with beautiful visuals and crafts.

The user experience is more than just how simple and appealing the application is to use. UX includes visual design, content, user experience, accessibility, capabilities, and context to provide a satisfactory interaction for a pleasant user experience.

It requires us to gather and evaluate our user experience. We construct aesthetically appealing scenarios and analyze them with actual build environment users. We use a variety of tools to evaluate product usability. Moreover, UI/UX for us is Innovation and Storytelling.",
                            ServiceTitle = "UX/UI Design"
                        },
                        new
                        {
                            Id = 2,
                            ServiceDescription = @"Digital marketing is the art of selling your products and services to a specific target audience over the internet. It is the heart of any online business today. While we talk about traditional marketing, it is comparably more expensive and time-consuming.

We at Aarav Web Solution consider digital marketing to be a wide-ranging field of marketing.

As a result, we offer various digital marketing services that include the use of websites, mobile devices, social media, search engines, and apps—basically, anything that combines marketing with user feedback or two-way communication between the company and the customer.

So, let your business grow through digital marketing with Aarav Web Solution.",
                            ServiceTitle = "Media Marketing"
                        },
                        new
                        {
                            Id = 3,
                            ServiceDescription = @"Over time, the phrase ""content writing"" has spread like wildfire. You might not realize it, but content can be found in everything including what you listen to, see and read here.

In our opinion, businesses can create emotional connections with their target market through the use of content creation. This led to increased brand engagement and consumer loyalty. 

Thus, at Aarav Web Solution our dedicated content writer creates content materials to attract users to a particular business, product, or online website. So, as per our client's requirements, we help them in creating and writing blogs, emails, newsletters, white papers, video scripts, and many other types of content.",
                            ServiceTitle = "Content Solution"
                        },
                        new
                        {
                            Id = 4,
                            ServiceDescription = @"We give strategic solutions to website design and development.

In the highly competitive world of web development, it is crucial to understand each type of web development and its underlying technology. A website's development is a challenging task, especially if it is done from scratch.

Aarav Web Solution can make the process easier whether you're creating a small business website, an online store, or something else. Our web development services are centered on providing value that characterizes the online interactions between our clients and their customers.

We design WordPress websites and create front and back-end code for websites in a choice of coding languages. In addition, we provide our finest support in ongoing website analysis to let you know how your website is performing. If desired, we can additionally provide speed optimization from the standpoint of our SEO expertise.

We build a user-friendly website for you that helps to build your brand, appear like a pro, and move up the search engine results pages. From getting domain names to adding monthly analytics to your website, we are here for you.

So, explore all of the ways we may help you create a more efficient website and make your business online!",
                            ServiceTitle = "Web Development"
                        },
                        new
                        {
                            Id = 5,
                            ServiceDescription = @"Social Media Marketing is far more diverse than anyone could have imagined. Simply said, social marketing is the use of social media channels to advertise and sell your brand's product or service. Similarly to how you would organize other aspects of your marketing strategy.

However, you must have a plan and a team that can work together to achieve the intended results. To be more precise, social marketing is more than just posting, liking, and commenting. Each post should grab your customers' interest and deserve one share.

And at Aarav Web Solution, we endeavor to make every like a virtual action. We generate leads, raise brand awareness, expand your brand's audience, and much more.",
                            ServiceTitle = "Social Media Management"
                        },
                        new
                        {
                            Id = 6,
                            ServiceDescription = "",
                            ServiceTitle = "Upcoming Services"
                        });
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("NormalizedEmail")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers");

                    b.HasDiscriminator<string>("Discriminator").HasValue("IdentityUser");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("RoleId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("AaravEnterprise.Models.ApplicationUser", b =>
                {
                    b.HasBaseType("Microsoft.AspNetCore.Identity.IdentityUser");

                    b.Property<string>("City")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PostalCode")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("State")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("StreetAddress")
                        .HasColumnType("nvarchar(max)");

                    b.HasDiscriminator().HasValue("ApplicationUser");
                });

            modelBuilder.Entity("AaravEnterprise.Models.Cart", b =>
                {
                    b.HasOne("AaravEnterprise.Models.ApplicationUser", "ApplicationUser")
                        .WithMany()
                        .HasForeignKey("ApplicationUserId");

                    b.HasOne("AaravEnterprise.Models.Package", "Package")
                        .WithMany()
                        .HasForeignKey("PackageId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AaravEnterprise.Models.Services", "Service")
                        .WithMany()
                        .HasForeignKey("ServiceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("AaravEnterprise.Models.Package", b =>
                {
                    b.HasOne("AaravEnterprise.Models.Services", "Services")
                        .WithMany()
                        .HasForeignKey("ServicesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
