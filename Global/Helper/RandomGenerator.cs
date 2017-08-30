using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HELP.GlobalFile.Global.Helper
{
    public static class RandomGenerator
    {
        /// <param name="length">how many numbers the string should have</param>
        /// <returns>a string combined with only numbers</returns>
        public static string GetNumbers(int length)
        {
            double value = new Random().NextDouble();
            return string.Format("{0:N6}", value).Substring(2, length);
        }
    }
}
