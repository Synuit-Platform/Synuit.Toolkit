﻿using System.Reactive.Linq;
using System.Threading;
using System.Threading.Channels;
using Microsoft.AspNetCore.SignalR;
using Synuit.Toolkit.SignalR.Server.Extensions;
using Synuit.Toolkit.SignalR.Server.Types;

namespace Synuit.Toolkit.SignalR.Server.Services
{
    public class StreamingHub<T> : Hub, ISetEvent
    {
        protected readonly IStreamingDataProvider<T> _streamingDataProvider;
        private readonly AsyncAutoResetEvent _aev = new AsyncAutoResetEvent();

        private int _isValid = 0;

        protected StreamingHub(StreamingDataProvider<T> streamingDataProvider)
        {
            IsValid = true;
            streamingDataProvider.Add(this);
            _streamingDataProvider = streamingDataProvider;          
        }

        public ChannelReader<T> StartStreaming()
        {
            return Observable.Create<T>(async observer =>
            {
                while (!Context.ConnectionAborted.IsCancellationRequested)
                {               
                    await _aev.WaitAsync();
                    observer.OnNext(_streamingDataProvider.Current);
                }
            }).AsChannelReader();
        }

        public bool IsValid
        {
            get => Interlocked.Exchange(ref _isValid, _isValid) == 1;
            private set => Interlocked.Exchange(ref _isValid, value ? 1 : 0);
        }
        
        public void SetEvent()
        {
            _aev.Set();
        }

        protected override void Dispose(bool disposing)
        {
            IsValid = false;
            base.Dispose(disposing);
        }
    }
}
