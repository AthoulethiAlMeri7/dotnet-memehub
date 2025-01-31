using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using api.Application.Dtos;
using API.Domain.Models;
using API.Infrastructure.Persistence.DbContext;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;

namespace api.Infrastructure.Persistence
{
    public class DbSeeder
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IMapper _mapper;
        private readonly ILogger<DbSeeder> _logger;

        public DbSeeder(IServiceProvider serviceProvider, IMapper mapper, ILogger<DbSeeder> logger)
        {
            _serviceProvider = serviceProvider;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task SeedAsync()
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<Guid>>>();

                // Seed roles
                var roles = new List<string> { "ROLE_ADMIN", "ROLE_USER", "ROLE_GUEST" };
                foreach (var role in roles)
                {
                    if (!await roleManager.RoleExistsAsync(role))
                    {
                        await roleManager.CreateAsync(new IdentityRole<Guid> { Name = role });
                    }
                }

                // Seed admin user
                var adminEmail = "admin@gmail.com";
                var adminUser = await userManager.FindByEmailAsync(adminEmail);
                if (adminUser == null)
                {
                    adminUser = new ApplicationUser
                    {
                        UserName = "admin",
                        Email = adminEmail,
                        EmailConfirmed = true,
                        ProfilePic = "/Infrastructure/Assets/ProfilePics/default.png",
                    };
                    var result = await userManager.CreateAsync(adminUser, "Admin@123");
                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(adminUser, "ROLE_ADMIN");
                    }
                }

                if (await context.Templates.AnyAsync())
                {
                    _logger.LogInformation("Database has already been seeded.");
                    return; // DB has been seeded
                }

                try
                {
                    var httpClient = new HttpClient();
                    var response = await httpClient.GetStringAsync("https://api.imgflip.com/get_memes");
                    var memes = JObject.Parse(response)["data"]["memes"].ToObject<List<ApiTemplateDto>>();

                    var templates = _mapper.Map<List<Template>>(memes);

                    // Convert string Id to Guid
                    foreach (var template in templates)
                    {
                        template.Id = Guid.NewGuid(); // Generate a new Guid for each template
                    }

                    context.Templates.AddRange(templates);
                    await context.SaveChangesAsync();

                    _logger.LogInformation("Database seeding completed successfully.");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "An error occurred while seeding the database.");
                }
            }
        }
    }
}