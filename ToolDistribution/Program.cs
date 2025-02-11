using Microsoft.EntityFrameworkCore;
using ToolDistribution;
using ToolDistribution.Models;
using ToolDistribution.Enums;
using ToolDistribution.Models.DBmodels;
using ToolDistribution.Repositories;
using ToolDistribution.RepositoryInterfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddScoped<IStorageRepository, StorageRepository>();
builder.Services.AddScoped<IToolRepository, ToolRepository>();
builder.Services.AddScoped<IArticleRepository, ArticleRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IToolRequestRepository, ToolRequestRepository>();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    var connection = builder.Configuration.GetConnectionString("DefaultConnection");
    options.UseSqlServer(connection);
});

builder.Services.AddDefaultIdentity<User>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddRoles<IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddAuthorization();

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

app.MapRazorPages();

using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();

    for (int i = 0; i < Enum.GetNames(typeof(UserRoles)).Length; i++)
    {
        string role = Enum.GetName(typeof(UserRoles), i);
        if (!await roleManager.RoleExistsAsync(role))
        {
            await roleManager.CreateAsync(new IdentityRole(role));
        }
        bool hasAny = userManager.GetUsersInRoleAsync(role).Result.Any();
        if (!hasAny) {
            User user = new User()
            {
                Name = role,
                Surname = role,
                Patronymic = role,
                UserName = $"testuser{i}",
                Email = "email@email.com",
                Status = UserStaus.Работает.ToString()
            };
            string Password = "Test_password123!";
            var result = await userManager.CreateAsync(user, Password);
            
            await userManager.AddToRoleAsync(user, role);
        }
    }
}

app.Run();
