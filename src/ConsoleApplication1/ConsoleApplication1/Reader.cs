using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace ConsoleApplication1
{
    class Reader
    {

        public static int[,] fromFile(String path)
        {
            int[, ,] bmp = Reader.readBitmap(new Bitmap(path));
            return Reader.readGrayscaledBitmap(bmp);
        }

        public static int[, ,] readBitmap(Bitmap bmp)
        {
            int width = bmp.Width;
            int height = bmp.Height;
            int[,,] tab = new int[width, height,3];
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    Color pixel = bmp.GetPixel(i, j);
                    tab[i, j, 0] = pixel.R;
                    tab[i, j, 1] = pixel.G;
                    tab[i, j, 2] = pixel.B;
                }
            }
            
            return tab;
        }

        public static int[,] readGrayscaledBitmap(int [,,] bmp)
        {
            int width = bmp.GetLength(0);
            int height = bmp.GetLength(1);
            int[,] tab = new int[width, height];
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    tab[i, j] = bmp[i, j, 0];
                }
            }
            
            return tab;
        }

        public static int[][,] readBitmapSet(String path)
        {
            string[] files = System.IO.Directory.GetFiles(path, "*.bmp");

            List<int[,]> list = new List<int[,]>();
            list.Clear();

            foreach (string file in files) {

                int[,] bmp = Reader.fromFile(file);
                list.Add(bmp);
            }
            
            return list.ToArray();
        }

    }
}
