
namespace Synuit.Toolkit.SignalR.Server.Types
{
    public interface IStreamingDataProvider<T>
    {
        T Current { get; }
    }

  
}
