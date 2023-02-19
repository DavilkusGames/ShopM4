using ShopM4_DataMigrations.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using ShopM4_Utility;

using ShopM4_DataMigrations.Repository;
using ShopM4_DataMigrations.Repository.IRepository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddHttpContextAccessor();   // нужно для работы с сессиями для View

builder.Services.AddSession(options =>
{
    options.Cookie.Name = "Winter2022";
    //options.IdleTimeout = TimeSpan.FromSeconds(10);
});   // для работы с сессиями

builder.Services.AddDbContext<ApplicationDbContext>(
    options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

// для автоматического создания таблиц в бд
//builder.Services.AddDefaultIdentity<IdentityUser>().
//    AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddIdentity<IdentityUser, IdentityRole>().
    AddDefaultUI().AddDefaultTokenProviders().
    AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddTransient<IEmailSender, EmailSender>();   // EMAIL SENDER


builder.Services.AddScoped<IRepositoryCategory, RepositoryCategory>();


builder.Services.AddControllersWithViews();  // MVC

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

app.UseAuthentication();  // added

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();   // для определения маршрута к странице Razor

/*app.Use((context, next) =>
{
    context.Items["name"] = "Dany";
    return next.Invoke();
}); */

app.UseSession();     // добавление middleware для работы с сессиями

/*app.Run(x =>
{
    //return x.Response.WriteAsync("Hello " + x.Items["name"]);
    if (x.Session.Keys.Contains("name"))
    {
        return x.Response.WriteAsync(x.Session.GetString("name"));
    }
    else
    {
        x.Session.SetString("name", "Uasya");
        return x.Response.WriteAsync("NO");
    }
}); */

app.Run();

