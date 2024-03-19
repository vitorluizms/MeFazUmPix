using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Options;
using MyWallet.Config;
using MyWallet.DTOs;
using MyWallet.Models;
using RabbitMQ.Client;

public class MessageService(IOptions<QueueConfig> queueConfig)
{
    private readonly string _hostName = queueConfig.Value.HostName;
    private readonly string _userName = queueConfig.Value.UserName;
    private readonly string _password = queueConfig.Value.Password;
    private readonly string _queueName = queueConfig.Value.QueueName;

    public void SendMessage(Payments payment)
    {
        ConnectionFactory factory = new() { HostName = _hostName, UserName = _userName, Password = _password };
        IConnection _connection = factory.CreateConnection();
        IModel _channel = _connection.CreateModel();

        _channel.QueueDeclare(
            queue: _queueName,
            durable: false,
            exclusive: false,
            autoDelete: false,
            arguments: null
        );
 
        string json = JsonSerializer.Serialize(payment);
        var body = Encoding.UTF8.GetBytes(json);


        IBasicProperties properties = _channel.CreateBasicProperties();
        properties.Persistent = true;

        _channel.BasicPublish(
            exchange: String.Empty,
            routingKey: _queueName,
            basicProperties: properties,
            body: body
        );
    }
}
