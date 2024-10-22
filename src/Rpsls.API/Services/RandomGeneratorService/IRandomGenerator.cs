namespace Rpsls.API.Services.RandomGeneratorService
{
    public interface IRandomGenerator
    {
        int Next(int minValue, int maxValue);
    }
}
