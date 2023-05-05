using COMP_2084_Lesson_01.Data;
using Humanizer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using static System.Net.Mime.MediaTypeNames;
using System.Runtime.Intrinsics.X86;
using System;

// The WebApplicationBuilder class provides a convenient and flexible way to configure and build an ASP.NET 6.0 application using code.
var builder = WebApplication.CreateBuilder(args);

// Add services to the dependency injection container.
// This line retrieves the connection string from the configuration file. The GetConnectionString method looks for a connection string with the name "DefaultConnection". If it finds the connection string, it assigns it to the connectionString variable. If it doesn't find the connection string, it throws an InvalidOperationException with an error message. (The connection string can be found in the appsettings.json file)
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// This line registers the ApplicationDbContext as a service in the application's dependency injection container. It also configures the context to use SQL Server as the database provider and sets the connection string to the value of connectionString.
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

// This line registers a middleware that displays detailed error pages when the application is running in development mode. It is useful for debugging and troubleshooting database-related issues.
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// This line registers the default ASP.NET Core Identity system in the application. It adds the IdentityUser class as the default user model and configures the system to require confirmed accounts for sign-in. It also specifies that the user data should be stored in the ApplicationDbContext.
builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();

// This line registers the MVC framework in the application. It adds support for controllers and views and registers the necessary services for the framework to work.
builder.Services.AddControllersWithViews();

// Once the application is configured, we can now build an instance of the application and assign it to app
var app = builder.Build();

// Register middleware to the application
// Configure the HTTP request pipeline.
// This code block checks whether the application is running in the development environment. If it is, it registers the UseMigrationsEndPoint middleware, which provides a web interface for applying database migrations. If it isn't, it registers the UseExceptionHandler middleware, which handles exceptions thrown by the application, and the UseHsts middleware, which sets the HTTP Strict Transport Security (HSTS) header.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

// This middleware automatically redirects HTTP requests to HTTPS. It is used to enforce secure connections between the client and server.
app.UseHttpsRedirection();

// This middleware serves static files such as CSS, JavaScript, and images. It is used to serve static content to the client.
app.UseStaticFiles();

// This middleware is responsible for routing incoming requests to the appropriate controller and action method. It sets up the route table for the application.
app.UseRouting();

// This middleware is responsible for authenticating the user. It supports various authentication schemes, such as cookies, tokens, and external authentication providers.
app.UseAuthentication();

// This middleware is responsible for authorizing the user. It checks whether the user has the required permissions to access a resource.
app.UseAuthorization();

// This middleware maps HTTP requests to controller actions. It is used to define the routing rules for controllers and actions.
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// This middleware maps HTTP requests to Razor Pages. It is used to define the routing rules for Razor Pages.
app.MapRazorPages();


// The app.Run() method is the final middleware in the middleware pipeline of an ASP.NET 6.0 application. It is responsible for handling any requests that are not handled by earlier middleware components in the pipeline.

// When a request reaches the app.Run() method, it means that no middleware component has handled the request yet. The app.Run() method takes a delegate that represents the request handling logic. This delegate can be used to generate a response for the request, terminate the request processing pipeline, or perform other actions.

// In the default base template of an ASP.NET 6.0 application, the app.Run() method is not used, as the request handling logic is usually implemented in a controller action method or a Razor Page. However, you can use the app.Run() method to handle specific requests that do not match any controller action or Razor Page.
app.Run();
