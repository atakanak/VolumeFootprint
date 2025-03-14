namespace VolumeFootprint.WorkerService;

public interface IVolumeManager
{
    Task UpdateVolumeList(decimal price, decimal quantity);
    void PrintVolumeList();
}