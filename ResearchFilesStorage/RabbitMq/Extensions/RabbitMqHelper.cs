using RabbitMq.Configuration;
using RabbitMQ.Client;

namespace RabbitMq.Extensions;

public static class RabbitMqHelper
{
    public static IConnection GetConnection(RabbitMqConfiguration rabbitMqConfiguration)
    {
        var connection = new ConnectionFactory
        {
            HostName = rabbitMqConfiguration.HostName,
            UserName = rabbitMqConfiguration.UserName,
            Password = rabbitMqConfiguration.Password,
            Port = rabbitMqConfiguration.Port
        }.CreateConnection();

        return connection;
    }
}