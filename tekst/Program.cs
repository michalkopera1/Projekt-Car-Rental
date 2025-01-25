using Microsoft.EntityFrameworkCore;
using tekst.Data;
using tekst.Models;
using Microsoft.AspNetCore.Identity;
using tekst.Areas.Identity.Data;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>()
     .AddDefaultUI();


builder.Services.AddControllersWithViews(options =>
{
    options.Filters.Add(new Microsoft.AspNetCore.Mvc.Authorization.AuthorizeFilter());
});

var app = builder.Build();


using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    context.Database.Migrate();

   
    if (!context.Cars.Any())
    {
        context.Cars.AddRange(
            new Car { Make = "Toyota", Model = "Corolla", Year = 2020, DailyPrice = 100.00m },
            new Car { Make = "Honda", Model = "Civic", Year = 2019, DailyPrice = 90.00m }
        );
        context.SaveChanges();
    }
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}


app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();


app.MapRazorPages();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


app.Run();
