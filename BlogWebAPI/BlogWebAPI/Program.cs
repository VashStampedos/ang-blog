using AutoMapper;
using BlogWebAPI.Entities;
using BlogWebAPI.MapperProfiles;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connection = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<BlogApplicationContext>(options =>
{
    options.UseSqlServer(connection);
}).AddIdentity<User, Role>(config =>
{
    
    config.Password.RequireNonAlphanumeric = false;
    config.Password.RequiredLength = 2;
    config.Password.RequireDigit = false;
    config.Password.RequireLowercase = false;
    config.Password.RequireUppercase = false;

    
    
}).AddEntityFrameworkStores<BlogApplicationContext>().AddDefaultTokenProviders();
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddControllersWithViews().AddNewtonsoftJson(options =>
{
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
    
});
builder.Services.ConfigureApplicationCookie(config =>
{
    config.LoginPath = "/Account/Login";
    config.Cookie.SameSite = SameSiteMode.None;
    
});
builder.Services.AddCors(options => options.AddPolicy("BlogApi", policy => 
{
    policy.WithOrigins("http://localhost:4200").AllowAnyMethod().AllowAnyHeader().AllowCredentials();
}));

builder.Services.AddEndpointsApiExplorer();


var app = builder.Build();

app.UseRouting();
app.UseHttpsRedirection();
app.UseCors("BlogApi");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
