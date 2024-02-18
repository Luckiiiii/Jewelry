using Jewelry.Data;
using Jewelry.Data.Entities;
using Jewelry.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.Reflection;
using System.Text;

var builder = WebApplication.CreateBuilder(args);



// Add services to the container.
    builder.Services.AddIdentity<StoreUser, IdentityRole>(cfg =>
    {
        cfg.User.RequireUniqueEmail = true;
    })
        .AddEntityFrameworkStores<JewelryContext>()
        .AddSignInManager<SignInManager<StoreUser>>();
builder.Services.AddAuthentication().AddCookie().AddJwtBearer(cfg =>
{
    cfg.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidIssuer = builder.Configuration["Token:Issuer"],
        ValidAudience = builder.Configuration["Token:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Token:Key"]))
    };
});

builder.Services.AddTransient<JewelrySeeder>();
builder.Services.AddDbContext<JewelryContext>();
builder.Services.AddControllersWithViews()
    .AddRazorRuntimeCompilation().
    AddNewtonsoftJson(cfg=>cfg.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);
builder.Services.AddRazorPages();
builder.Services.AddTransient<IMailService, NullMailService>();

builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());
builder.Services.AddScoped<IJewelryRepository, JewelryRepository>();
//builder.Services.AddIdentity<StoreUser, IdentityRole>()
        //.AddEntityFrameworkStores<JewelryContext>();
//Can cho biet database nao dbcontext can su dung

var app = builder.Build();

if (args.Length == 1 && args[0].ToLower() == "/seed")
{
    using (var scope = app.Services.CreateScope())
    {
        var serviceProvider = scope.ServiceProvider;

        var seeder = serviceProvider.GetRequiredService<JewelrySeeder>();
        seeder.SeedAsync().Wait();       
    };
    
}


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}


app.UseStatusCodePages();
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapRazorPages();
});

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=App}/{action=Index}/{id?}");

app.Run();

