using GoodBooksAPI.DataAccess;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddDbContext<GoodBooksApiContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("GoodBooksApiDb") ?? throw new InvalidOperationException("Connection string 'GoodBooksApiDb' not found.")).UseSnakeCaseNamingConvention());

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
