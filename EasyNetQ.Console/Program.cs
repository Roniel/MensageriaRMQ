using EasyNetQ;
using EasyNetQ.Console;
using Newtonsoft.Json;

const string EXCHANGE = "curso-rabbitmq";
const string QUEUE = "person-created";
const string ROUTING_KEY = "hr.person-created";

var person = new Person("Fulano de Tal", "123456789", new DateTime(1990, 1, 1));

var bus = RabbitHutch.CreateBus("host=localhost");

await bus.PubSub.PublishAsync(person, ROUTING_KEY);

await bus.PubSub.SubscribeAsync<Person>("marketing",msg =>
{
    var json = JsonConvert.SerializeObject(msg);
    Console.WriteLine(json);
});

Console.ReadLine();