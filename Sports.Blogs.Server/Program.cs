using Sports.Blogs.Server.Services.BlogPage;
 
using Sports.Blogs.Server.Data;
using Sports.Blogs.Server.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Syncfusion.Blazor;
using Blazored.LocalStorage;
using Sports.Blogs.Server.Services;

var builder = WebApplication.CreateBuilder(args);

// Adding syncufion 
Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("Ngo9BigBOggjHTQxAR8/V1NBaF5cXmZCf1FpRmJGdld5fUVHYVZUTXxaS00DNHVRdkdnWXpceHRSRmddVEx1X0s=");
builder.Services.AddSyncfusionBlazor();

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddScoped<SharedService>();
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddHttpClient<IBlogPageService, BlogPageService>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7114/api/");
});

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
