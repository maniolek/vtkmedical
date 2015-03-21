using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            String file = "assets/slice-001.png";

            Bitmap bm = null;
            using (var image = new Bitmap(file))
            {
                bm = new Bitmap(image);
            }

            bm = ImageProcessing.desaturate(bm);

            if (bm != null)
            {
                bm.Save("output/slice_text.png");
            }

        }
    }
}
