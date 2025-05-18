using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using order_service.Contracts;
using order_service.repository;
using order_service.services;
using shared_library;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<OrderContext>(x => x.UseSqlServer(builder.Configuration.GetConnectionString("db")));
builder.Services.AddTransient<IOrderService, OrderService>();
builder.Services.AddScoped<orderPublisher>();
builder.Services.Configure<rabbitMQsettings>(builder.Configuration.GetSection("RabbitMQ"));
builder.Services.AddMassTransit(x =>
{ 
    x.UsingRabbitMq((context, cfg) =>
    {
        var settings = context.GetRequiredService<IOptions<rabbitMQsettings>>().Value;
        cfg.Host(settings.HostName, settings.VirtualHost, h =>
        {
            h.Username(settings.UserName);
            h.Password(settings.Password);
        });
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

app.UseAuthorization();

app.MapControllers();

app.Run();
