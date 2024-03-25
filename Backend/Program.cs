using Backend.Automappers;
using Backend.DTOs;
using Backend.Models;
using Backend.Repository;
using Backend.Services;
using Backend.Validators;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddKeyedScoped<ICommonService<BeerDto, BeerInsertDto, BeerUpdateDto>, BeerService>("beerService");
builder.Services.AddKeyedScoped<ICommonService<SaleDto, SaleInsertDto, SaleUpdateDto>, SaleService>("saleService");


//Repository
builder.Services.AddScoped<IRepository<Beer>, BeerRepository>();
builder.Services.AddScoped<IRepository<Sale>, SaleRepository>();

//Automappers
builder.Services.AddAutoMapper(typeof(MappingProfile));
// Inyeccion base de datos / Entity Framework
builder.Services.AddDbContext<StoreContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("StoreConnection"));
});

//Validadores
builder.Services.AddScoped<IValidator<BeerInsertDto>, BeerInsertValidator>(); //Valida BeerInsertDto con BeerInsertValidator
builder.Services.AddScoped<IValidator<BeerUpdateDto>, BeerUpdateValidator>();
builder.Services.AddScoped<IValidator<SaleInsertDto>, SaleInsertValidator>();
builder.Services.AddScoped<IValidator<SaleUpdateDto>, SaleUpdateValidator>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
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
