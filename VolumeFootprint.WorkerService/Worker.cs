using Binance.Net.Interfaces.Clients;
using Binance.Net.Objects.Models.Spot.Socket;
using CryptoExchange.Net.Objects.Sockets;

namespace VolumeFootprint.WorkerService;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;

    private readonly IBinanceSocketClient _binanceSocketClient;

    private readonly IVolumeManager _volumeManager;
    
    public Worker(ILogger<Worker> logger, IBinanceSocketClient binanceSocketClient, IVolumeManager volumeManager)
    {
        _logger = logger;
        _binanceSocketClient = binanceSocketClient;
        _volumeManager = volumeManager;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await _binanceSocketClient.SpotApi.ExchangeData.SubscribeToTradeUpdatesAsync("BTCUSDT", OnTradeUpdate, stoppingToken);

        while (!stoppingToken.IsCancellationRequested)
        {
            _volumeManager.PrintVolumeList();
            await Task.Delay(1000, stoppingToken);
        }
    }

    private void OnTradeUpdate(DataEvent<BinanceStreamTrade> obj)
    {
        _volumeManager.UpdateVolumeList(obj.Data.Price, obj.Data.Quantity);
    }

    private void OnBookTickerUpdate(DataEvent<BinanceStreamBookPrice> obj)
    {
        Console.WriteLine("ASK : " + obj.Data.BestAskPrice + ", BID : " + obj.Data.BestBidPrice);
    }
}