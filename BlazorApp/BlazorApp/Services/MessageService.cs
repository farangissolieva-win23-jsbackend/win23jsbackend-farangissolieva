using Azure.Messaging.ServiceBus;
using Newtonsoft.Json;
using System.Text;

namespace BlazorApp.Services;

public class MessageService(ServiceBusClient serviceBusClient)
{
    private readonly ServiceBusClient _serviceBusClient = serviceBusClient;

    public async Task SendMessageAsync(string queueName, string message)
    {
        // send verification code
        var sender = _serviceBusClient.CreateSender(queueName);
        var jsonData = JsonConvert.SerializeObject(new { Email = message });
        var servicebusMessage = new ServiceBusMessage(Encoding.UTF8.GetBytes(jsonData)) { ContentType = "application/json" };

        await sender.SendMessageAsync(servicebusMessage);
        await sender.DisposeAsync();
        await _serviceBusClient.DisposeAsync();
       
    }
}