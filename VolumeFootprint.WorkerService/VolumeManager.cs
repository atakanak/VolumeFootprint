using System.Collections.Concurrent;

namespace VolumeFootprint.WorkerService;

public class VolumeManager : IVolumeManager
{
    private ConcurrentDictionary<decimal, decimal> _volumeMap;
    
    public VolumeManager()
    {
        _volumeMap = new ConcurrentDictionary<decimal, decimal>();
    }
    public Task UpdateVolumeList(decimal price, decimal quantity)
    {
        var tempKey = decimal.Round(price, 0, MidpointRounding.AwayFromZero);
        if (_volumeMap.TryAdd(tempKey, quantity))
        {
            return Task.CompletedTask;
        }
        else
        {
            var currentValue = _volumeMap[tempKey];
            _volumeMap.TryUpdate(tempKey, currentValue + quantity, currentValue);
            return Task.CompletedTask;
        }
    }

    public void PrintVolumeList()
    {
        Console.Clear();
        Console.WriteLine("-------------------");
        foreach (var key in _volumeMap)
        {
            Console.WriteLine($"{key.Key}: {key.Value}");
        }
        Console.WriteLine("-------------------");
    }
}