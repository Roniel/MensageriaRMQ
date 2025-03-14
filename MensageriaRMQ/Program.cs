
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

const string EXCHANGE_NAME = "curso-rabbitmq";
var person = new Person("Fulano de Tal", "123456789", new DateTime(1990, 1, 1));

var factory = new ConnectionFactory()
{
    HostName = "localhost",
    Port = 5672,
};

var connection = await factory.CreateConnectionAsync();

var channel =  await connection.CreateChannelAsync();

await channel.QueueDeclareAsync(queue: "person-created",
                                    durable: true,
                                    exclusive: false,
                                    autoDelete: false,
                                    arguments: null);

var json = JsonConvert.SerializeObject(person);
var body = Encoding.UTF8.GetBytes(json);

//await channel.BasicPublishAsync(exchange: EXCHANGE_NAME,
//                                routingKey: "hr.person-created",
//                                body: body);

//Console.WriteLine($"Mensagem enviada: {json}");

var consumerChannel = await connection.CreateChannelAsync();

var consume = new AsyncEventingBasicConsumer(consumerChannel);

consume.ReceivedAsync += async (sender, eventArgs) =>
{
    var message = Encoding.UTF8.GetString(eventArgs.Body.ToArray());
    var person = JsonConvert.DeserializeObject<Person>(message);

    Console.WriteLine($"Mensagem recebida: {message}");
    Console.WriteLine($"Nome: {person.FullName}");
    Console.WriteLine($"Documento: {person.Documento}");
    Console.WriteLine($"Data de Nascimento: {person.BirthDate}");
    
    await consumerChannel.BasicAckAsync(eventArgs.DeliveryTag, false);
};

await consumerChannel.BasicConsumeAsync(queue: "person-created",
                            autoAck: false,
                            consumer: consume);
Console.ReadLine();

class Person
    {
        public Person(string fullName, string documento, DateTime birthDate)
        {
            FullName = fullName;
            Documento = documento;
            BirthDate = birthDate;
        }
        public string FullName { get; set; }
        public string Documento { get; set; }
        public DateTime BirthDate { get; set; }
    }
