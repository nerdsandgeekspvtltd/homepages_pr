using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Sports.Events.WA;
using Syncfusion.Blazor;
using Sports.Events.WA.Services;
using Blazored.LocalStorage;

namespace Sports.Events.WA
{
    /// <summary>
    /// Main entry point for the Blazor WebAssembly application.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Main method to start the application.
        /// </summary>
        public static async Task Main(string[] args)
        {
            // Create a new WebAssembly host builder with default configurations
            var builder = WebAssemblyHostBuilder.CreateDefault(args);

            // Register the root component of the application and specify the target element in the HTML file
            builder.RootComponents.Add<App>("#app");

            // Add support for injecting components into the head of the HTML document
            builder.RootComponents.Add<HeadOutlet>("head::after");

            // Register services required for the application

            // Register the GeolocationService for accessing geolocation features
            builder.Services.AddScoped<Darnton.Blazor.DeviceInterop.Geolocation.IGeolocationService,
                Darnton.Blazor.DeviceInterop.Geolocation.GeolocationService>();

            // Register the Syncfusion license key for using Syncfusion Blazor components
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense(Utlities.syncfusionKey);
            builder.Services.AddSyncfusionBlazor();

            // Register HttpClient for making HTTP requests
            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

            builder.Services.AddScoped<SharedService>();
            builder.Services.AddBlazoredLocalStorage();

            // Register custom services

            // Register the EventService for accessing event-related data
            builder.Services.AddScoped<IEventService, EventService>();

            // Register the AzureMapJs service for interacting with Azure Maps JavaScript functions
            builder.Services.AddScoped<IAzureMapJs, AzureMapJs>();

            // Build the WebAssembly host and start the application
            await builder.Build().RunAsync();
        }
    }
}
