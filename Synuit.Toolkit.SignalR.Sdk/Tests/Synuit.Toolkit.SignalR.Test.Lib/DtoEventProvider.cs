using System;
using System.Threading;
using ModelLib;
using Synuit.Toolkit.SignalR.Server.Services;

namespace DtoProviderLib
{
    public class DtoEventProvider : StreamingDataProvider<Dto>
    {
        private const int rndLowLimit = 0;
        private const int rndUpperLimit = 999;
        private const int intervalInMs = 3000;

        private readonly Timer _timer;
        private readonly Random _random = new Random(11);

        private static DtoEventProvider _helper;
        public static DtoEventProvider Instance => _helper ?? (_helper = new DtoEventProvider());

        private DtoEventProvider()
        {
            _timer = new Timer(_ => Current = new Dto { ClientId = $"{Guid.NewGuid()}", Data = _random.Next(rndLowLimit, rndUpperLimit) }, null, 0, intervalInMs);
        }
    }
}
