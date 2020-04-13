using System;
using System.Linq;
using System.Threading.Tasks;
using System.Text;
using Microsoft.AspNetCore.SignalR;
using ModelLib;
using Synuit.Toolkit.SignalR.Server.Services;
using DtoProviderLib;

namespace Synuit.Toolkit.SignalR.Test.Hubs
{
    // The hub class provides data streaming to client subscriber.
    // This is implemented with its base class.
    // The base class takes event provider as a ctor parameter. 
    public class TheFirstHub : StreamingHub<Dto>
    {
        public TheFirstHub() : base(DtoEventProvider.Instance)
        {
        }

        public async Task ProcessDto(Dto[] args)
        {
            var sbClients = new StringBuilder();
            var sbData = new StringBuilder();

            if (args != null && args.Length > 0)
            {
                sbClients.Append($"{Environment.NewLine}Clients: ");
                foreach (var clientId in args.Select(dto => dto.ClientId).Distinct())
                    sbClients.Append($"{clientId} ");

                sbData.Append("--> Data: ");
                foreach (var dto in args)
                    sbData.Append($"{dto.Data} ");
            }
            else
            {
                sbClients.Append("No clients");
                sbData.Append("No data available");
            }

            await Clients.All.SendAsync("ReceiveMessage", sbClients.ToString(), sbData.ToString());
        }
    }
}
