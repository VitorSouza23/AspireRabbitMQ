var builder = DistributedApplication.CreateBuilder(args);

var rabbitmq = builder.AddRabbitMQ("messaging")
                      .WithManagementPlugin();

builder.AddProject<Projects.AspireRabbitMQ_Receiver>("receiver")
    .WithReference(rabbitmq);

builder.AddProject<Projects.AspireRabbitMQ_Sender>("sender")
    .WithReference(rabbitmq);

builder.Build().Run();
