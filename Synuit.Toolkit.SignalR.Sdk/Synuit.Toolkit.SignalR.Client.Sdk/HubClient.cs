using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Synuit.Toolkit.SignalR.Client
{
   public class HubClient : IDisposable
   {
      public string Url { get; }

      public HubConnection Connection { get; protected set; }
      protected CancellationTokenSource _cts = new CancellationTokenSource();

      public HubClient(string url)
      {
         Url = url;
      }

      public async Task<bool> ConnectWithRetryAsync()
      {
         // Keep trying to until we can start or the token is canceled.
         while (true)
         {
            try
            {
               await Connection.StartAsync(_cts.Token);
               //Debug.Assert(connection.State == HubConnectionState.Connected);
               return true;
            }
            catch when (_cts.Token.IsCancellationRequested)
            {
               return false;
            }
            catch
            {
               // Failed to connect, trying again in 5000 ms.
               //Debug.Assert(connection.State == HubConnectionState.Disconnected);
               await Task.Delay(5000);
            }
         }
      }

      public async Task<HubClient> StartConnectionAsync(int retryIntervalInMs = 0, int numOfAttempts = 0)
      {
         Connection = new HubConnectionBuilder()
         .WithUrl(Url)
         .AddMessagePackProtocol()
         //.WithAutomaticReconnect()
         .Build();

         for (var i = 0; i < numOfAttempts; i++)
         {
            try
            {
               await Connection.StartAsync();
               return this;
            }
            catch (Exception e)
            {
               if (i < numOfAttempts - 1)
                  await Task.Delay(numOfAttempts);
               else
                  throw new Exception($"Hub connection on \"{Url}\" had failed. ", e);
            }
         }

         return null;
      }

      public async Task<bool> InvokeAsync(string methodName, params object[] args)
      {
         if (Connection == null || _cts.Token.IsCancellationRequested || args == null)
            return false;

         try
         {
            await Connection.InvokeAsync(methodName, args, _cts.Token);
            return true;
         }
         catch (Exception e)
         {
            throw new Exception($"Hub\"{Url}\" InvokeAsync() of method \"{methodName}()\" had failed. ", e);
         }
      }

      public async Task<bool> SubscribeAsync<T>(Action<T> callback)
      {
         if (Connection == null || _cts.Token.IsCancellationRequested || callback == null)
            return false;

         try
         {
            var channel = await Connection.StreamAsChannelAsync<T>("StartStreaming", _cts.Token);
            while (await channel.WaitToReadAsync())
               while (channel.TryRead(out var t))
               {
                  try
                  {
                     callback(t);
                  }
                  catch (Exception e)
                  {
                     throw new Exception($"Hub \"{Url}\" Subscribe(): callback had failed. ", e);
                  }
               }

            return true;
         }
         catch (OperationCanceledException)
         {
            return false;
         }
      }

      public void Cancel()
      {
         _cts.Cancel();
      }

      public void Dispose()
      {
         Connection.DisposeAsync().Wait();
      }

      public async static Task<HubClient> CreateHubClient(string url)
      {
         var client = new HubClient(url)
         {
            Connection = new HubConnectionBuilder()
            .AddMessagePackProtocol()
            .WithUrl(url)
            //.WithAutomaticReconnect()
            .Build()
         };

         client.Connection.Reconnecting += error =>
         {
            //Debug.Assert(connection.State == HubConnectionState.Reconnecting);

            // Notify users the connection was lost and the client is reconnecting.
            // Start queuing or dropping messages.

            return Task.CompletedTask;
         };
         client.Connection.Reconnected += connectionId =>
         {
            //Debug.Assert(Connection.State == HubConnectionState.Connected);

            // Notify users the connection was reestablished.
            // Start dequeuing messages queued while reconnecting if any.

            return Task.CompletedTask;
         };

         client.Connection.Closed += async (error) =>
         {
            // --> attempt to reconnect if cancel hasn't been requested
            if (!client._cts.IsCancellationRequested)
            {
               await Task.Delay(new Random().Next(0, 5) * 1000);
               await client.Connection.StartAsync();
            }
         };

         return await Task.FromResult(client);
      }
   }
}