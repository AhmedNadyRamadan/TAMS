using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using TASM.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<TamsContext>(options =>
   options.UseSqlServer(builder.Configuration.GetConnectionString("TamsContext")));

// Fix for CS1061: Ensure the required Identity package is referenced and the correct method is used
builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddEntityFrameworkStores<TamsContext>();
builder.Services.AddRazorPages();

//builder.Services.AddAuthentication(options =>
//{
//    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
//    options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
//})
//.AddCookie()
//.AddGoogle(options =>
//{
//    options.ClientId = "227907847407-jdp7c21rp2n5g1prnnqoo3dkef1k6ckm.apps.googleusercontent.com";
//    options.ClientSecret = "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";
//    options.CallbackPath = "/signin-google";
//    options.SaveTokens = true; 
//});
// Configure the HTTP request pipeline.
var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();
app.MapRazorPages();
app.MapControllerRoute(
   name: "default",
   pattern: "{controller=Home}/{action=Index}/{id?}")
   .WithStaticAssets();

app.Run();