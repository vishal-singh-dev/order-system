using kitchen_service.Service;
using MassTransit;
using Microsoft.Extensions.Options;
using shared_library;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.Configure<rabbitMQsettings>(builder.Configuration.GetSection("RabbitMQ"));
builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<OrderConsumer>();

    x.UsingRabbitMq((context, cfg) =>
    {
        var settings = context.GetRequiredService<IOptions<rabbitMQsettings>>().Value;
        cfg.Host(settings.HostName, settings.VirtualHost, h =>
        {
            h.Username(settings.UserName);
            h.Password(settings.Password);
        });

        cfg.ReceiveEndpoint("submit-order-queue", e =>
        {
            e.ConfigureConsumer<OrderConsumer>(context);
        });
    });
});
builder.Services.AddSignalR();
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials()
            .WithOrigins("http://localhost:5173"); 
    });
});

var app = builder.Build();
app.MapHub<OrderHub>("/orderHub");
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.UseCors();
app.Run();
