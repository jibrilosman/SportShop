using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OnlineSportShop.Data;
using OnlineSportShop.Models;
using OnlineSportShop.Repositories.Abstract;
using OnlineSportShop.Repositories.Implementation;
using Stripe;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

var ConnectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.Configure<StripeSettings>(builder.Configuration.GetSection("Stripe"));

StripeConfiguration.ApiKey = builder.Configuration.GetValue<string>("Stripe:SecretKey");


builder.Services.AddDbContext<OnlineSportShopContext>(options => options.UseSqlServer(ConnectionString));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<OnlineSportShopContext>()
    .AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(op => op.LoginPath = "/UserAuthentication/login");

builder.Services.AddScoped<IUserAuthenticationService, UserAuthenticationService>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "Admin",
    pattern: "{area:exists}/{controller=UserAuthentication}/{action=login}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
