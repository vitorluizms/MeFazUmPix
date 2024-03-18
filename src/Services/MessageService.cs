using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Options;
using MyWallet.Config;
using RabbitMQ.Client;

public class MessageService(IOptions<QueueConfig> queueConfig)
{
    private readonly string _hostName = queueConfig.Value.HostName;
    private readonly string _userName = queueConfig.Value.UserName;
    private readonly string _password = queueConfig.Value.Password;
    private readonly string _queueName = queueConfig.Value.QueueName;

    public void SendMessage(string message)
    {
        ConnectionFactory factory = new() { HostName = _hostName, UserName = _userName, Password = _password };
        IConnection _connection = factory.CreateConnection();
        IModel _channel = _connection.CreateModel();

        _channel.QueueDeclare(queue: _queueName,
            durable: false,
            exclusive: false,
            autoDelete: false,
            arguments: null
        );

        string json = JsonSerializer.Serialize(message);

        var body = Encoding.UTF8.GetBytes(message);
        _channel.BasicPublish(
            exchange: String.Empty,
            routingKey: _queueName,
            basicProperties: null,
            body: body
        );
    }
}