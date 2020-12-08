using System;

namespace Qwirkle_WPF
{
    class RandomGenerator
    {
        static readonly Random _random = new Random();
        public static int RandomNumber(int min, int max)
        {
            return _random.Next(min, max);
        }
    }
}
