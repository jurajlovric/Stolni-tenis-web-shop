using System.Text;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
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

string connectionString = builder.Configuration.GetConnectionString("DefaultConnection");


builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8
                .GetBytes(builder.Configuration.GetSection("AppSettings:Token").Value)),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });

builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder =>
{
    
    containerBuilder
        .RegisterType<CategoryService>()
        .As<ICategoryService>()
        .InstancePerLifetimeScope();

    containerBuilder
        .RegisterType<CategoryRepository>()
        .As<ICategoryRepository>()
        .InstancePerLifetimeScope();


    containerBuilder.RegisterInstance(connectionString).As<string>();
});


builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        policyBuilder =>
        {
            policyBuilder.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();
        });
});

var app = builder.Build();

app.UseCors("AllowAllOrigins");
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "TableTennis API v1"));
}

app.MapControllers();
app.Run();
