using Microsoft.AspNetCore.Hosting;

[assembly: HostingStartup(typeof(AaravEnterprise.Areas.Identity.IdentityHostingStartup))]
namespace AaravEnterprise.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) =>
            {
            });
        }
    }
}