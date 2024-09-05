using System.Text;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using TableTennis.Mapper;
using TableTennis.Service;
using TableTennis.Service.Common;
using TableTennis.Repository;
using TableTennis.Repository.Common;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "TableTennis API", Version = "v1" });
});

string connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddAutoMapper(typeof(UserProfile));

var tokenKey = builder.Configuration.GetSection("AppSettings:Token").Value;

if (string.IsNullOrEmpty(tokenKey))
{
    throw new ArgumentNullException("Token key is not configured properly in the appsettings.json.");
}

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKey)),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder =>
{
    containerBuilder.RegisterType<UserService>().As<IUserService>().InstancePerLifetimeScope();
    containerBuilder.RegisterType<UserRepository>().As<IUserRepository>().InstancePerLifetimeScope();

    containerBuilder.RegisterType<CategoryService>().As<ICategoryService>().InstancePerLifetimeScope();
    containerBuilder.RegisterType<CategoryRepository>().As<ICategoryRepository>().InstancePerLifetimeScope();

    containerBuilder.RegisterType<ProductService>().As<IProductService>().InstancePerLifetimeScope();
    containerBuilder.RegisterType<ProductRepository>().As<IProductRepository>().InstancePerLifetimeScope();

    containerBuilder.RegisterType<OrderService>().As<IOrderService>().InstancePerLifetimeScope();
    containerBuilder.RegisterType<OrderRepository>().As<IOrderRepository>().InstancePerLifetimeScope();

    containerBuilder.RegisterInstance(connectionString).As<string>();
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "TableTennis API v1"));
}

app.UseCors("AllowAllOrigins");

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
