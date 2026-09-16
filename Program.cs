using AutoMapper;
using computerChip.Data;
using computerChip.Mappings;
using computerChip.Models;
using computerChip.Repositories.Implementations;
using computerChip.Repositories.Interfaces;
using computerChip.Services;
using computerChip.Services.Implementations;
using computerChip.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.Google;        // 🔥 NUEVO
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddAutoMapper(typeof(MappingProfile));

// 🔥 Configuración de Logging
builder.Services.AddLogging(loggingBuilder =>
{
    loggingBuilder.ClearProviders();
    loggingBuilder.AddConsole();
    loggingBuilder.AddDebug();
    loggingBuilder.SetMinimumLevel(LogLevel.Debug);
});

builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders =
        ForwardedHeaders.XForwardedFor |
        ForwardedHeaders.XForwardedProto;
    options.KnownNetworks.Clear();
    options.KnownProxies.Clear();
});

var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secretKey = jwtSettings["SecretKey"];

// ============================================
// 🔥 AUTENTICACIÓN: JWT + GOOGLE
// ============================================
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    var jwtSettings = builder.Configuration.GetSection("JwtSettings");

    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]))
    };

    // 🔥 DEBUG: Ver errores de JWT
    options.Events = new JwtBearerEvents
    {
        OnAuthenticationFailed = context =>
        {
            Console.WriteLine($"❌ JWT Error: {context.Exception.Message}");
            return Task.CompletedTask;
        },
        OnTokenValidated = context =>
        {
            Console.WriteLine($"✅ JWT OK: {context.Principal?.Identity?.Name}");
            return Task.CompletedTask;
        }
    };
})
// 🔥 NUEVO: Configuración de Google
.AddGoogle(options =>
{
    options.ClientId = builder.Configuration["Authentication:Google:ClientId"]
        ?? throw new InvalidOperationException("Authentication:Google:ClientId no está configurado");
    options.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"]
        ?? throw new InvalidOperationException("Authentication:Google:ClientSecret no está configurado");

    // Ruta de callback (debe coincidir con Google Cloud Console)
    options.CallbackPath = "/signin-google";

    // Scopes para obtener email y perfil
    options.Scope.Add("email");
    options.Scope.Add("profile");

    options.SaveTokens = true;

    // 🔥 Eventos de Google
    options.Events.OnCreatingTicket = context =>
    {
        Console.WriteLine($"✅ Google Auth OK: {context.Principal?.Identity?.Name}");
        return Task.CompletedTask;
    };

    options.Events.OnRemoteFailure = context =>
    {
        Console.WriteLine($"❌ Google Auth Error: {context.Failure?.Message}");
        context.Response.Redirect("/login?error=google_auth_failed");
        context.HandleResponse();
        return Task.CompletedTask;
    };
});

builder.Services.AddAuthorization();

// ===============================
// CORS 🔥
// ===============================

var origenes = builder.Configuration.GetSection("origenesPermitidos").Get<string[]>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("FrontendPolicy", policy =>
    {
        policy.WithOrigins(origenes)
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// Configurar DbContext con MySQL
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        new MySqlServerVersion(new Version(8, 0, 43))
    ));

// ============================================
// REPOSITORIOS
// ============================================
builder.Services.AddScoped(typeof(IRepository<>), typeof(GenericRepository<>));

builder.Services.AddScoped<IAdminRepository, AdminRepository>();
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<ILoginGoogleRepository, LoginGoogleRepository>();
builder.Services.AddScoped<ITokenRepository, TokenRepository>();
builder.Services.AddScoped<IPushTokenRepository, PushTokenRepository>();

builder.Services.AddScoped<IProductoRepository, ProductoRepository>();
builder.Services.AddScoped<ICategoriaRepository, CategoriaRepository>();
builder.Services.AddScoped<IMarcaRepository, MarcaRepository>();
builder.Services.AddScoped<IEspecificacionesRepository, EspecificacionesRepository>();

builder.Services.AddScoped<ICarritoRepository, CarritoRepository>();
builder.Services.AddScoped<IItemPedidoRepository, ItemPedidoRepository>();
builder.Services.AddScoped<IPedidoRepository, PedidoRepository>();
builder.Services.AddScoped<IZonaEnvioRepository, ZonaEnvioRepository>();
builder.Services.AddScoped<IOfertaRepository, OfertaRepository>();

// ============================================
// SERVICIOS
// ============================================
builder.Services.AddScoped<JwtService>();

builder.Services.AddScoped<IAdminService, AdminService>();
builder.Services.AddScoped<IAuthService, AuthService>();

builder.Services.AddScoped<IUsuarioService, UsuarioService>();
builder.Services.AddScoped<ILoginGoogleService, LoginGoogleService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IPushTokenService, PushTokenService>();

builder.Services.AddScoped<IProductoService, ProductoService>();
builder.Services.AddScoped<ICategoriaService, CategoriaService>();
builder.Services.AddScoped<IMarcaService, MarcaService>();
builder.Services.AddScoped<IEspecificacionesService, EspecificacionesService>();

builder.Services.AddScoped<ICarritoService, CarritoService>();
builder.Services.AddScoped<IItemPedidoService, ItemPedidoService>();
builder.Services.AddScoped<IPedidoService, PedidoService>();
builder.Services.AddScoped<IZonaEnvioService, ZonaEnvioService>();
builder.Services.AddScoped<IOfertaService, OfertaService>();

// ============================================
// FILE SERVICE
// ============================================
builder.Services.AddScoped<IFileService, FileService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// ============================================
// SEED: Crear administrador inicial
// ============================================
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    var adminExiste = await dbContext.Admins.AnyAsync(a => a.usuario == "admin");
    if (!adminExiste)
    {
        var admin = new Admin
        {
            usuario = "admin",
            password = "$2a$12$RJYapF.6ZAy.RtLswh9k0uKo6cqGKe7zYQSBv7Kl.R.WfR4VRgiGW"
        };
        await dbContext.Admins.AddAsync(admin);
        await dbContext.SaveChangesAsync();
        Console.WriteLine("✅ Administrador creado exitosamente (usuario: admin)");
    }
}

if (app.Environment.WebRootPath == null)
{
    app.Environment.WebRootPath =
        Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
}

// 🔥 Forwarded headers ANTES de todo
app.UseForwardedHeaders();

// Swagger solo en dev
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// 🔥 ORDEN CORRECTO
app.UseRouting();

app.UseCors("FrontendPolicy");

app.UseAuthentication();  // 🔥 Debe ir ANTES de UseAuthorization
app.UseAuthorization();

app.MapControllers();

// 🔥 Escuchar solo interno (nginx expone el 80)
app.Urls.Clear();
app.Urls.Add("http://0.0.0.0:5200");

app.Run();