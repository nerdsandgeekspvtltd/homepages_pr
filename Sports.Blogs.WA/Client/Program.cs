using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Sports.Blogs.WA;
using Sports.Blogs.WA.Services;
using Sports.Blogs.WA.Services.BlogPage;
using Syncfusion.Blazor;

namespace Sports.Blogs.WA
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");
            builder.RootComponents.Add<HeadOutlet>("head::after");

            // Adding syncufion 
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("Ngo9BigBOggjHTQxAR8/V1NBaF5cXmZCf1FpRmJGdld5fUVHYVZUTXxaS00DNHVRdkdnWXpceHRSRmddVEx1X0s=");
            builder.Services.AddSyncfusionBlazor();

            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:7114/api/") });
            builder.Services.AddScoped<SharedService>();
            builder.Services.AddBlazoredLocalStorage();
            builder.Services.AddScoped<IBlogPageService, BlogPageService>();

            await builder.Build().RunAsync();
        }
    }
}
