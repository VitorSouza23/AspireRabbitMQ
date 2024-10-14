using System.Text;
using Microsoft.AspNetCore.Mvc;
using RabbitMQ.Client;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.AddServiceDefaults();
builder.AddRabbitMQClient("messaging");

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapPost("/send", ([FromServices] IConnection connection, [FromServices] ILogger<Program> logger, string message) =>
{
    try
    {
        logger.LogInformation("Sending message");
        using var channel = connection.CreateModel();
        channel.QueueDeclare(queue: "message", durable: false, exclusive: false, autoDelete: false, arguments: null);
        var body = Encoding.UTF8.GetBytes(message);
        channel.BasicPublish(exchange: string.Empty, routingKey: "message", basicProperties: null, body: body);
        return Results.Ok();
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "Error sending message");
        return Results.Problem("Error sending message");
    }

});

app.Run();
