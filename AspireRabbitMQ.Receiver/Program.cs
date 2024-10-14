
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
builder.AddRabbitMQClient("messaging");
builder.Services.AddHostedService<Receiver>();

var app = builder.Build();


app.Run();

public class Receiver(ILogger<Receiver> logger, IConnection connection) : BackgroundService
{
    private IModel? _messageChannel;
    private EventingBasicConsumer? _consumer;

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            string queueName = "message";
            _messageChannel = connection.CreateModel();
            _messageChannel.QueueDeclare(queue: queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);

            _consumer = new EventingBasicConsumer(_messageChannel);
            _consumer.Received += OnMessageReceived;

            _messageChannel.BasicConsume(queue: queueName, autoAck: true, consumer: _consumer);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error receiving message");
        }

        return Task.CompletedTask;
    }

    public override async Task StopAsync(CancellationToken stoppingToken)
    {
        await base.StopAsync(stoppingToken);
        if (_consumer is not null)
            _consumer.Received -= OnMessageReceived;
        _messageChannel?.Dispose();
    }

    private void OnMessageReceived(object? sender, BasicDeliverEventArgs e)
    {
        var message = Encoding.UTF8.GetString(e.Body.ToArray());
        logger.LogInformation("Received message: {message}", message);
    }
}
