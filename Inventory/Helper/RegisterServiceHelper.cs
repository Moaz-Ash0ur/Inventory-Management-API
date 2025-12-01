using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Inventory.BLL.Interface;
using Inventory.BLL.ProfileMapping;
using Inventory.BLL.Services;
using Inventory.DAL.Contracts;
using Inventory.DAL.Data;
using Inventory.DAL.Models;
using Inventory.DAL.Repositories;
using System.ComponentModel;
using System.Configuration;
using System.Net.Http.Headers;

namespace InventoryApi.Helper
{

    public static class RegisterServiceHelper
    {

        public static void RegisterService(this WebApplicationBuilder builder)
        {


           builder.Services.AddSwaggerGenJwtAuth();

            //Configration  Project
            builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(
                builder.Configuration.GetConnectionString("DefaultConnection")));

           


            //Log error
            //Log.Logger = new LoggerConfiguration()
            //    .WriteTo.Console()
            //    .WriteTo.MSSqlServer(
            //    connectionString: builder.Configuration.GetConnectionString("DefaultConnection"),
            //    tableName: "Log",
            //   autoCreateSqlTable: true)
            //    .CreateLogger();

            //builder.Host.UseSerilog();



            builder.Services.AddAutoMapper(typeof(MappingProfile).Assembly);
            builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            builder.Services.AddScoped(typeof(IUnitOfWork), typeof(UnitOfWork));

            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IRefreshTokens, RefreshTokenService>();
            builder.Services.AddScoped<IAuthService, AuthService>();
            //builder.Services.AddScoped<IEmailService, EmailService>();


            builder.Services.AddScoped<IProductService, ProductService>();
            builder.Services.AddScoped<ICategories, CategoryService>();
            builder.Services.AddScoped<IPurchaseServices, PurchaseService>();
            builder.Services.AddScoped<ICustomerService, CustomerService>();
            builder.Services.AddScoped<ISupplierService, SupplierService>();
            builder.Services.AddScoped<ISalesService, SaleService>();
            builder.Services.AddScoped<ISaleItems, SaleItemService>();
            builder.Services.AddScoped<IStockTransactions, StockTransactionService>();


           
            // Identity
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.SignIn.RequireConfirmedEmail = false;
            })
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();

            // JWT
            builder.Services.CustomJwtAuthConfig(builder.Configuration);

      


        }

    }

}
