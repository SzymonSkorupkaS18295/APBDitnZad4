
using APBDcw4.Warehouse.Interface;
using APBDcw4.Warehouse.Repository;
using APBDcw4.Warehouse.Service;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("WarehouseDatabase");

builder.Services.AddScoped<IOrderRepository>(_ => new OrderRepository(connectionString));
builder.Services.AddScoped<IProductWarehouseRepository>(_ => new ProductWarehouseRepository(connectionString));
builder.Services.AddScoped<IProductRepository>(_ => new ProductRepository(connectionString));
builder.Services.AddScoped<IWarehouseRepository>(_ => new WarehouseRepository(connectionString));
builder.Services.AddScoped<WarehouseService>();


// Searches through assembly for controllers
builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();
app.UseHttpsRedirection();
//Maps endpoints
app.MapControllers();
app.Run();