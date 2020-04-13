﻿using Microsoft.AspNetCore.SignalR.Client;
using ModelLib;
using Synuit.Toolkit.SignalR.Client;
using System;
using System.Threading;

namespace SignalRClientTest
{
   internal class Program
   {
      private const string URL =
#if HTTPS
                        "https://localhost:15001/hub/the1st";
#else
                        "http://localhost:15000/hub/the1st";

#endif

      private static HubClient _hubClient;
      private static AutoResetEvent _ev = new AutoResetEvent(false);

      private static void Main(string[] args)
      {
         MainAsync();

         while (true)
         {
            Console.WriteLine("Press any key \"q\" to quit or any other key to call server...");
            var ch = Console.ReadKey().KeyChar;
            if (ch == 'q' || ch == 'Q')
            {
               _hubClient.Cancel();
               _ev.WaitOne();
               break;
            }

            var now = $"{DateTime.Now}";
            var br0 = _hubClient.InvokeAsync("ProcessDto",
                new[]
                {
                        new Dto {ClientId = ".NETCoreClient", Data = 10},
                        new Dto {ClientId = ".NETCoreClient", Data = 11},
                        new Dto {ClientId = ".NETCoreClient", Data = 12},
                }).Result;
         }
      }

      private static async void MainAsync()
      {
         // Create hub client and connect to server
         //using (_hubClient = await new HubClient(URL)
         //           .StartConnectionAsync(retryIntervalInMs: 1000, numOfAttempts: 15))
         using (_hubClient = await HubClient.CreateHubClient(URL))
         {
            var result = await _hubClient.ConnectWithRetryAsync();
            if (result)
            {
               // Client provides handler for server's call of method ReceiveMessage
               _hubClient.Connection.On("ReceiveMessage", (string s0, string s1) => Console.WriteLine($"{s0} {s1}"));

               // Client calls server's method ProcessDto
               var br0 = await _hubClient.InvokeAsync("ProcessDto",
                   new[]
                   {
                        new Dto {ClientId = ".NETCoreClient", Data = 91},
                        new Dto {ClientId = ".NETCoreClient", Data = 92},
                   });

               // Client subscribes for stream of Dto objects providing appropriate handler
               if (!await _hubClient.SubscribeAsync<Dto>(arg => Console.WriteLine(arg)))
                  _ev.Set();
            }
         }
      }
   }
}