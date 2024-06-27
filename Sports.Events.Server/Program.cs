using AzureMapsControl.Components;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Sports.Events.Server;
using Sports.Events.Server.Data;
using Sports.Events.Server.Services;
using Syncfusion.Blazor;
using System.Net.Http;

var builder = WebApplication.CreateBuilder(args);



// Adding syncufion 
Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("Ngo9BigBOggjHTQxAR8/V1NBaF5cXmZCf1FpRmJGdld5fUVHYVZUTXxaS00DNHVRdkdnWXpceHRSRmddVEx1X0s=");
builder.Services.AddSyncfusionBlazor();

// Add services to the container.
builder.Services.AddHttpClient<IEventService, EventService>();
builder.Services.AddRazorPages();
builder.Services.AddScoped<SharedService>();
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddServerSideBlazor();

builder.Services
.AddScoped<Darnton.Blazor.DeviceInterop.Geolocation.IGeolocationService,
Darnton.Blazor.DeviceInterop.Geolocation.GeolocationService>();


var AzureMaps = builder.Configuration.GetSection("AzureMaps");
//This code uses an anonymous authentication
builder.Services.AddAzureMapsControl(
    configuration => configuration.ClientId = AzureMaps.GetValue<string>("ClientId"));

AuthService.SetAuthSettings(AzureMaps);
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}


app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
