using BankServices.Context;
using BankServices.Services;
using Microsoft.EntityFrameworkCore;
using static BankServices.Entities.BankTransaction;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<BankContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"), npgsqlOptions =>
    {
        npgsqlOptions.MapEnum<TransactionTypes>("transactiontypes");
    }));

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.AllowTrailingCommas = true;
    });

builder.Services.AddScoped<IBankServices, BankServices.Services.BankServices>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
