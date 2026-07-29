using FluentValidation;
using Microsoft.EntityFrameworkCore;
using OrderManagementBackend.Api.Filters;
using OrderManagementBackend.Api.Middlewares;
using OrderManagementBackend.Application.Interfaces;
using OrderManagementBackend.Application.Mappings;
using OrderManagementBackend.Application.Services;
using OrderManagementBackend.Application.Validators.Order;
using OrderManagementBackend.Domain.Common;
using OrderManagementBackend.Domain.Interfaces;
using OrderManagementBackend.Infrastructure.Common;
using OrderManagementBackend.Infrastructure.Data;
using OrderManagementBackend.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DbOrders");
builder.Services.AddDbContext<OrdersContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

builder.Services.AddScoped<ICurrentUserProvider, CurrentUserProvider>();

builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddProfile<OrderMappingProfile>();
    cfg.AddProfile<ProductMappingProfile>();
    cfg.AddProfile<OrderProductMappingProfile>();
});

builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();

builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IOrderService, OrderService>();

builder.Services.AddControllers(options => options.Filters.Add<ValidationFilter>());
builder.Services.AddValidatorsFromAssemblyContaining<CreateOrderDtoValidator>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        policy =>
        {
            policy
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowAnyOrigin();
        });
});

var app = builder.Build();

app.UseExceptionHandler();

app.UseCors("AllowFrontend");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
