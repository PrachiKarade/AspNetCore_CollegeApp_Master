global using AutoMapper;
global using CoreWebApiSuperHero.Models;
global using Microsoft.EntityFrameworkCore;
using System.Text;
using CoreWebApiSuperHero.Configurations;
using CoreWebApiSuperHero.Data;
using CoreWebApiSuperHero.Data.Repository;
using CoreWebApiSuperHero.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
//using Serilog;


var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders(); // This is used to clear the default logging providers

// Add services to the container.

builder.Services.AddControllers(option => option.ReturnHttpNotAcceptable = true).AddNewtonsoftJson().AddXmlDataContractSerializerFormatters(); // This is used to enable JSON patch support in ASP.NET Core

builder.Services.AddScoped<ISuperHeroService, SuperHeroService>();      // This is used to register the SuperHeroService for dependency injection
builder.Services.AddScoped<IStudentRepository, StudentRepository>();    // This is used to register the SuperHeroService for dependency injection
builder.Services.AddScoped(typeof(ICollegeRepository<>), typeof(CollegeRepository<>));// This is used to register the common repository for dependency injection
builder.Services.AddScoped<IUserRepository, UserRepository>();      // This is used to register the UserRepository for dependency injection
builder.Services.AddScoped<IUserService, UserService>();            // This is used to register the UserService for dependency injection

builder.Services.AddDbContext<DataContext>(options =>

    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))//options is the DbContextOptions<DataContext> instance passed to the DataContext constructor
);

builder.Services.AddDbContext<CollegeDBContext>(options =>

    options.UseSqlServer(builder.Configuration.GetConnectionString("CollegeAppDBConnection"))//options is the DbContextOptions<DataContext> instance passed to the DataContext constructor
);

#region SiriLog settings
/*Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information() // Set the minimum log level to Debug
     .WriteTo.File("Log/log.txt",rollingInterval:RollingInterval.Minute)
    .CreateLogger();

//builder.Host.UseSerilog(); // This is used to configure Serilog as the logging provider for the application . it overrides the default logging provider with Serilog, allowing you to use Serilog for logging throughout your application.
builder.Logging.AddSerilog();// This is used to add Serilog as a logging provider to the application's logging pipeline
*/
#endregion

builder.Logging.AddLog4Net();
builder.Services.AddAutoMapper(typeof(AutoMapperConfig));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();// This is used to add support for Swagger/OpenAPI documentation


builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT authoriztion header using the bearer scheme.Enter Bearer[space] and your token in text input eg. Barear sarvada6786",
        Name = "Autorization",
        In = ParameterLocation.Header,
        Scheme = "Bearer"
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Id ="Bearer",
                    Type = ReferenceType.SecurityScheme
                },
                Scheme ="auth2",
                Name="Bearer",
                In=ParameterLocation.Header
            },
            new List<string>()
        }
    });
});// This is used to generate Swagger documentation for the API

var key = Encoding.ASCII.GetBytes(builder.Configuration.GetValue<string>("JWTSecretKey"));
string issuer = builder.Configuration.GetValue<string>("LocalIssuer");
string audience = builder.Configuration.GetValue<string>("LocalAudience");

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer("LocalUsers", options =>
{
    options.IncludeErrorDetails = true;
    //options.RequireHttpsMetadata = false;  This is used to specify whether HTTPS is required for the metadata address or authority
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = true,
        ValidIssuer = issuer,
        ValidateAudience = true,
        ValidAudience = audience

        //ValidAlgorithms = new[] { SecurityAlgorithms.HmacSha512 }
    };

    /*options.Events = new JwtBearerEvents
    {
        OnAuthenticationFailed = context =>
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("❌ JWT failed: " + context.Exception.GetType().Name);
            Console.WriteLine("❌ Message: " + context.Exception.Message);
            Console.ResetColor();
            return Task.CompletedTask;
        },
        OnChallenge = context =>
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("⚠️ JWT challenge: " + context.ErrorDescription);
            Console.ResetColor();
            return Task.CompletedTask;
        }
    };*/

});// This is used to add JWT authentication to the application

builder.Services.AddCors(Options =>
{
    Options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
    });
    Options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();// This is used to allow only the specified origin to access the API. // This is used to allow any header in the request
        // This is used to allow any HTTP method (GET, POST, PUT, DELETE, etc.) in the request
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseCors("AllowAll"); // This is used to enable CORS in the application with the default policy

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();// This is used to map the controllers to the routes defined in the controllers

/*app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers().RequireCors("AllowAll");
});*/

app.Run();// This is used to run the application and start listening for incoming HTTP requests
