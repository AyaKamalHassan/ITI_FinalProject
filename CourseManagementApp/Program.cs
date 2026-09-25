
using CourseManagementApp.Data;
using CourseManagementApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CourseManagementApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllersWithViews();

            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(
                    builder.Configuration.GetConnectionString("DefaultConnection")
                    ?? "Server=.;Database=CourseCenterDb_ITI;Trusted_Connection=True;Encrypt=False;"
                )
            );

            builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
            {
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 6;
            })
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();

            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                context.Database.EnsureCreated();

                if (!context.Instructors.Any())
                {
                    context.Instructors.AddRange(
                        new Instructor
                        {
                            Name = "Eng. Abdullah Salama",
                            Email = "salama@iti.gov.eg",
                            Specialization = ".NET & Backend"
                        },
                        new Instructor
                        {
                            Name = "Eng. Mohamed",
                            Email = "m.ali@iti.gov.eg",
                            Specialization = "Front-End Development"
                        },
                        new Instructor
                        {
                            Name = "Eng. Aya Kamal",
                            Email = "sara@iti.gov.eg",
                            Specialization = "Database & AI"
                        }
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

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Course}/{action=Index}/{id?}"
            );

            app.Run();
        }
    }
}

