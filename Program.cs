using cartingWithRedis;
using cartingWithRedis.IRepository;
using cartingWithRedis.IRepository.Repository;
using cartingWithRedis.Models;
using cartingWithRedis.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<ApplicationDbContext>();
builder.Services.AddScoped<ICartingService, CartingService>();
builder.Services.AddScoped<ICustomerService, CustomerService>();

builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<SignInManager<Customer>>();
builder.Services.AddScoped<IConnectionMultiplexer>(option =>
    ConnectionMultiplexer.Connect(("localhost:6379")));

builder.Services.AddIdentity<Customer, IdentityRole<int>>().AddEntityFrameworkStores<ApplicationDbContext>();

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

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
