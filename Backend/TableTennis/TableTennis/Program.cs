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

// Postavljanje Autofac kao ServiceProviderFactory
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "TableTennis API", Version = "v1" });
});

// Dohva?anje connection stringa iz konfiguracije
string connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Dodavanje AutoMappera i registracija profila
builder.Services.AddAutoMapper(typeof(UserProfile)); // Registracija UserProfile-a za mapiranja

// Konfiguracija JWT autentifikacije ako koristite JWT za prijavu
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

// Dodajte CORS politiku
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Konfiguracija Autofaca za registraciju servisa i repozitorija
builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder =>
{
    // Registracija User servisa i repozitorija
    containerBuilder.RegisterType<UserService>().As<IUserService>().InstancePerLifetimeScope();
    containerBuilder.RegisterType<UserRepository>().As<IUserRepository>().InstancePerLifetimeScope();

    // Registracija drugih servisa i repozitorija prema potrebi
    containerBuilder.RegisterType<CategoryService>().As<ICategoryService>().InstancePerLifetimeScope();
    containerBuilder.RegisterType<CategoryRepository>().As<ICategoryRepository>().InstancePerLifetimeScope();

    containerBuilder.RegisterType<ProductService>().As<IProductService>().InstancePerLifetimeScope();
    containerBuilder.RegisterType<ProductRepository>().As<IProductRepository>().InstancePerLifetimeScope();

    containerBuilder.RegisterType<OrderService>().As<IOrderService>().InstancePerLifetimeScope();
    containerBuilder.RegisterType<OrderRepository>().As<IOrderRepository>().InstancePerLifetimeScope();

    // Registracija connection stringa ako ga želite koristiti negdje posebno
    containerBuilder.RegisterInstance(connectionString).As<string>();
});

var app = builder.Build();

// Middleware konfiguracija
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "TableTennis API v1"));
}

// Aktivacija CORS-a prije definiranja ruta
app.UseCors("AllowAllOrigins");

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
