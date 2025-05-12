using System.Net;
using System.Text;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using FERSOFT.ERP.Infrastructure.Data;
using FERSOFT.ERP.Domain.Entities;
using FERSOFT.ERP.Application.Mappings;
using FERSOFT.ERP.Domain.Interfaces;
using FERSOFT.ERP.Application.Interfaces;
using FERSOFT.ERP.Application.Services;
using FERSOFT.ERP.Infrastructure.Repositorios;
using FERSOFT.ERP.Infrastructure.Repositorios.GenericRepository;
using FERSOFT.ERP.Infrastructure.Repositorios.Cinema;
using FERSOFT.ERP.Application.Interfaces.Cinema;
using FERSOFT.ERP.Application.Services.Cinema;
using FERSOFT.ERP.Infrastructure.Handlers;    // <— tu GlobalExceptionHandler
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Hellang.Middleware.ProblemDetails;
using FERSOFT.ERP.Application.Exceptions;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// 1. DbContext
builder.Services.AddDbContext<ApplicationDbContext>(opt =>
    opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// 2. Identity con AppUsuario
builder.Services.AddIdentity<AppUsuario, IdentityRole>(opt =>
    opt.SignIn.RequireConfirmedAccount = false)
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

// 3. Repositorios y servicios
builder.Services.AddScoped<IUsuarioRepositorio, UsuarioRepositorio>();
builder.Services.AddScoped<IUsuarioService, UsuarioService>();
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped(typeof(IRepositoryGeneric<>), typeof(RepositoryGeneric<>));
builder.Services.AddScoped<IReportRepository, ReportRepository>();
builder.Services.AddScoped<IBillboardRepository, BillboardRepository>();
builder.Services.AddScoped<IBookingRepository, BookingRepository>();
builder.Services.AddScoped<IBillboardService, BillboardService>();
builder.Services.AddScoped<IBookingService, BookingService>();
builder.Services.AddScoped<ISeatService, SeatService>();

// 4. AutoMapper
builder.Services.AddAutoMapper(typeof(ERPMapper));

// 5. Authentication & Authorization (JWT)
var key = builder.Configuration["Jwt:Key"];
builder.Services.AddAuthentication(opt =>
{
    opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(opt =>
{
    opt.RequireHttpsMetadata = false;
    opt.SaveToken = true;
    opt.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key)),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});
builder.Services.AddAuthorization();

// 6. Controllers + Swagger con JWT
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "FERSOFT.ERP API", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Bearer. Ejemplo: 'Bearer {token}'",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme {
                Reference = new OpenApiReference {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

// 7. GlobalExceptionHandler + ProblemDetails
builder.Services.AddSingleton<GlobalExceptionHandler>();
builder.Services.AddProblemDetails(opts =>
{
    opts.IncludeExceptionDetails = (ctx, ex) => builder.Environment.IsDevelopment();
    // Mapea excepciones personalizadas:
    opts.Map<UnauthorizedAccessException>(ex => new ProblemDetails
    {
        Status = StatusCodes.Status401Unauthorized,
        Title = "Unauthorized",
        Detail = "No estás autorizado para acceder a este recurso."
    });
    opts.Map<NotFoundException>(ex => new ProblemDetails
    {
        Status = StatusCodes.Status404NotFound,
        Title = "Resource Not Found",
        Detail = ex.Message
    });
    // Cualquier otra excepción:
    opts.Map<Exception>(ex => new ProblemDetails
    {
        Status = StatusCodes.Status500InternalServerError,
        Title = "An Unexpected Error Occurred",
        Detail = "Por favor, inténtalo más tarde."
    });
});

var app = builder.Build();

// 8. Middleware pipeline

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Usa primero el manejador global de excepciones
app.UseExceptionHandler(errApp =>
{
    errApp.Run(async context =>
    {
        var handler = context.RequestServices.GetRequiredService<GlobalExceptionHandler>();
        var cancellation = context.RequestAborted;
        await handler.HandleAsync(context, cancellation);
    });
});

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// 9. Seed inicial
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var db = services.GetRequiredService<ApplicationDbContext>();
    await FERSOFT.ERP.Infrastructure.Seeding.ApplicationDbInitializer.SeedAsync(db);
}

app.Run();
