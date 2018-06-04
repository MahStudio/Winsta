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
    }
}
