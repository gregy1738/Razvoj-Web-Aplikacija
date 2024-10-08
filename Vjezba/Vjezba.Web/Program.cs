using Microsoft.EntityFrameworkCore;
using Vjezba.DAL;
using Vjezba.Model;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews()
    .AddRazorRuntimeCompilation();

builder.Services.AddDbContext<ClientManagerDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("ClientManagerDbContext"),
            opt => opt.MigrationsAssembly("Vjezba.DAL")));

//builder.Services.AddDbContext<ClientManagerDbContext>(options =>
//options.UseSqlServer(
//        "Data Source=127.0.0.1;Initial Catalog=mvc-2023-24;User ID=sa;Password=yourStrong(!)Password;MultipleActiveResultSets=True;TrustServerCertificate=True;",
//            b => b.MigrationsAssembly("Vjezba.DAL")));

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
    name: "contact",
    pattern: "kontakt-forma",
    defaults: new { controller = "Home", action = "Contact" });

app.MapControllerRoute(
    name: "about",
    pattern: "o-aplikaciji/{LANG}",
    defaults: new { controller = "Home", action = "Privacy" },
    constraints: new { LANG = @"^[a-zA-Z]{2}$" });

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

//MockClientRepository.Instance.Initialize(Path.Combine(app.Environment.WebRootPath, "data"));
//MockCityRepository.Instance.Initialize(Path.Combine(app.Environment.WebRootPath, "data"));

app.Run();
