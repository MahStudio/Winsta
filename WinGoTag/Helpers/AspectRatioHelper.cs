using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinGoTag.Helpers
{
    class AspectRatioHelper
    {
        public static float GetAspectRatioForMedia(int Height, int Width)
        {
            float ratio = (float)Width / Height;
            return ratio;
        }
        public static int GCD(int a, int b)
        {
            int Remainder;

            while (b != 0)
            {
                Remainder = a % b;
                a = b;
                b = Remainder;
            }

            return a;
        }

        public static string Aspect(int x, int y)
        {
            return string.Format("{0}:{1}", x / GCD(x, y), y / GCD(x, y));
        }
    }
}
