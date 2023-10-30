using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace EasyWater.Domain.Extensions
{
    public static class MathExtensions
    {
        public static double Median(this IEnumerable<double> enu)
        {
            var xs = enu.ToArray();
            Array.Sort(xs);
            return xs[xs.Length / 2];
        }
    }
}
