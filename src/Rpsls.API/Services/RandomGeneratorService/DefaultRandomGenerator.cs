namespace Rpsls.API.Services.RandomGeneratorService
{
    public class DefaultRandomGenerator : IRandomGenerator
    {
        private readonly Random _random;

        public DefaultRandomGenerator()
        {
            _random = new Random();
        }

        public int Next(int minValue, int maxValue)
        {
            return _random.Next(minValue, maxValue);
        }
    }
}
