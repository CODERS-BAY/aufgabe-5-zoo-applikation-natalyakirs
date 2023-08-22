
using System.Text.Json; 
using ZooAPI.Controller; 
using ZooAPI.Model; 
using ZooAPI.Service; 
using Microsoft.AspNetCore.Builder; 
using Microsoft.AspNetCore.Http; 
using Microsoft.Extensions.DependencyInjection; 
using Microsoft.OpenApi.Models; 

 namespace ZooAPI 
 { 
    public class Program 
    { 
        public static void Main(string[] args) 
        { 
            var myAllowSpecificOrigins = "_myAllowSpecificOrigins"; 
            var builder = WebApplication.CreateBuilder(args); 
 
            // Enable CORS
            builder.Services.AddCors(options => 
            { 
                options.AddPolicy(name: myAllowSpecificOrigins, 
                    policyBuilder => 
                    { 
                        policyBuilder.AllowAnyOrigin() 
                            .AllowAnyHeader() 
                            .AllowAnyMethod(); 
                    }); 
            }); 
 
            // Configure services 
            ConfigureServices(builder); 
 
            // Create application
            var app = builder.Build(); 
 
            // Enable CORS 
            app.UseCors(myAllowSpecificOrigins); 
 
            // Configure application 
            Configure(app); 
 
            app.Run(); 
        } 
 
        private static void ConfigureServices(WebApplicationBuilder builder) 
        { 
            // Add services for API discovery and Swagger 
            builder.Services.AddEndpointsApiExplorer(); 
            builder.Services.AddSwaggerGen(option => 
            { 
                option.SwaggerDoc("v1", new OpenApiInfo { Title = "Zoo API", Version = "v1" }); 
            }); 
 
            // Database connection
            string connectionString = builder.Configuration.GetConnectionString("ZooDb") ?? string.Empty; 
 
            // Register services
            builder.Services.AddScoped(_ => new DBConnection(connectionString, builder.Configuration)); 
            builder.Services.AddScoped<VisitorService>(); 
            builder.Services.AddScoped<CashierService>(); 
            builder.Services.AddScoped<AnimalKeeperService>(); 
 
            // Register controllers
            builder.Services.AddControllers(); 
            builder.Services.AddScoped<VisitorController>(); 
            builder.Services.AddScoped<CashierController>(); 
            builder.Services.AddScoped<AnimalKeeperController>(); 
        } 
 
        private static void Configure(WebApplication app) 
        { 
            using var scope = app.Services.CreateScope(); 
 
            CashierService cashierService; 
            cashierService = scope.ServiceProvider.GetRequiredService<CashierService>(); 
 
            VisitorService visitorService; 
            visitorService = scope.ServiceProvider.GetRequiredService<VisitorService>(); 
 
            AnimalKeeperService animalkeeperService; 
            animalkeeperService = scope.ServiceProvider.GetRequiredService<AnimalKeeperService>(); 
 
            // Swagger set up
            app.UseSwagger();
            app.UseSwaggerUI(option => 
            { 
                option.SwaggerEndpoint("/swagger/v1/swagger.json", "zoo"); 
                option.RoutePrefix = string.Empty; 
            }); 
 
            // Cashier endpoint
            app.MapControllers(); 
            app.MapControllerRoute("cashier", "api/cashier/{controller=Home}/{action=Index}/{id?}"); 
 
            // Animal Keeper endpoint
            app.MapControllerRoute("animalkeeper", "api/animalkeeper/{controller=Home}/{action=Index}/{id?}"); 
 
            // Visitor endpoint 
            app.MapControllerRoute("visitor", "api/visitor/{controller=Home}/{action=Index}/{id?}"); 
        } 
    } 
} 