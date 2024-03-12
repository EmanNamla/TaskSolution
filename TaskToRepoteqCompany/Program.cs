using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TaskToRepoteqCompany.BLL.Repositoies.Abstraction;
using TaskToRepoteqCompany.BLL.Repositoies.Implementation;
using TaskToRepoteqCompany.BLL.UnitOfWork;
using TaskToRepoteqCompany.DAL.Contexts;
using TaskToRepoteqCompany.DAL.Models;
using TaskToRepoteqCompany.PL.Helpers.MappingProfiles;

namespace TaskToRepoteqCompany
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);


            // Add services to the container.
            builder.Services.AddControllersWithViews();

            builder.Services.AddDbContext<EcommerceDbContext>(option =>
            {
                option.UseSqlServer(builder.Configuration.GetConnectionString("DefultConnection"));
            });

            builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
               .AddEntityFrameworkStores<EcommerceDbContext>();

            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/Account/Login";
                options.AccessDeniedPath = "/Home/Error";
                options.LogoutPath = "/Account/SignOut";
                options.ExpireTimeSpan = TimeSpan.FromSeconds(30);
            });
            builder.Services.AddScoped<IUnitofWork, UnitOfwork>();

            builder.Services.AddAutoMapper(typeof(MappingProfiles));

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
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}