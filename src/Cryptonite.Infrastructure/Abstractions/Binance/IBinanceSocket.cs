namespace Cryptonite.Infrastructure.Abstractions.Binance
{
    public interface IBinanceSocket
    {
        void StartMiniTickerConnection();
        void StopMiniTickerConnection();
        bool IsAlive();
    }
}