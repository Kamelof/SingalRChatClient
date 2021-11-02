using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;

namespace SingalRChatClient
{
    class Program
    {
        static void Main(string[] args)
        {
            StartChatAsync().ConfigureAwait(false).GetAwaiter();
            Console.ReadKey();
        }

        static async Task StartChatAsync()
        {
            HubConnection connection
               = new HubConnectionBuilder()
               .WithUrl("https://localhost:5001/chat")
               .Build();

            connection.On<string, string>("ReceiveMessage",
                (user, message) =>
                {
                    var newMessage = $"{user}: {message}";
                    Console.WriteLine(newMessage);
                });

            await connection.StartAsync();

            string message;
            do
            {
                message = Console.ReadLine();
                await connection.InvokeAsync("SendMessage",
                      "ITEA", message);
            } while (!string.IsNullOrEmpty(message));
        }
    }
}
