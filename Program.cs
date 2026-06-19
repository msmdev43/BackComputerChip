using computerChip.Repositories.Interfaces;
using computerChip.Repositories;
using computerChip.Data;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// ============================================
// INICIO REPOSITORIOS EN EL PROGRAM
// ============================================
builder.Services.AddScoped(typeof(IRepository<>), typeof(GenericRepository<>));

// Repositorios específicos
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<ILoginGoogleRepository, LoginGoogleRepository>();
builder.Services.AddScoped<ITokenRepository, TokenRepository>();

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
// FIN REPOSITORIOS EN EL PROGRAM
// ============================================

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
