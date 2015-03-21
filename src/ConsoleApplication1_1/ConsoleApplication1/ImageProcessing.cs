using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace ConsoleApplication1
{
    class ImageProcessing
    {

        public Bitmap desaturate(Bitmap bitmap)
        {
            int width = bitmap.Width;
            int height = bitmap.Height;

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    Color c = bitmap.GetPixel(i, j);

                    int c_out = (c.R+c.G+c.B)/3;
                    bitmap.SetPixel(i, j, Color);
                    
                }
            }

            return null;
        }


    }
}
