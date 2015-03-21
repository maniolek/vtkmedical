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
                    int r = (c.R+c.G+c.B)/3;
                    bitmap.SetPixel(i, j, Color.FromArgb(r, r, r));
                }
            }

            return null;
        }

        public static Bitmap filterAverage(Bitmap bitmap, int mask_size)
        {
            int width = bitmap.Width;
            int height = bitmap.Height;

            int[,] tab = new int[width, height];
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    tab[i, j] = bitmap.GetPixel(i, j).R;
                }
            }


            int R, Rc;
            mask_size = (int)(mask_size / 2);

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    R = 0;
                    Rc = 0;

                    for (int x = -mask_size; x <= mask_size; ++x)
                    {
                        for (int y = -mask_size; y <= mask_size; ++y)
                        {
                            if ((i + x >= 0 && i + x < width) && (j + y >= 0 && j + y < height))
                            {
                                R = tab[i+x,j+y];
                                Rc++;
                            }
                        }
                    }
                    R /= Rc;

                    bitmap.SetPixel(i, j, Color.FromArgb(R, R, R));
                }
            }
            tab = null;
            return bitmap;
        }

        public static Bitmap getCut(Bitmap bm, int width, int height, int w_start, int h_start)
        {
            Bitmap newBitmap = new Bitmap(width, height);
            
            for (int i = 0; i < width; i++) {
                for (int j = 0; j < height; j++) {
                    newBitmap.SetPixel(i, j, bm.GetPixel(w_start + i, h_start + j));
                }
            }
            bm = null;
            return newBitmap;
        }

        public static Bitmap filterMedian2(Bitmap bitmap, int mask_size)
        {
            int width = bitmap.Width;
            int height = bitmap.Height;
            int rgb, nR;
            int[] R;
            int c;
            int mx, my;

            int msize = (int)(mask_size / 2);

            int[,] tab = new int[width, height];

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    tab[i, j] = bitmap.GetPixel(i, j).R;
                }
            }

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    c = 0;
                    R = new int[mask_size*mask_size];

                    for (int x = -msize; x <= msize; ++x)
                    {
                        for (int y = -msize; y <= msize; ++y)
                        {
                            if (i + x <= 0)
                            {
                                mx = 0;
                            }
                            else if (i + x >= width)
                            {
                                mx = width - 1;
                            }
                            else
                            {
                                mx = i + x;
                            }

                            if (j + y <= 0)
                            {
                                my = 0;
                            }
                            else if (j + y >= height)
                            {
                                my = height - 1;
                            }
                            else
                            {
                                my = j + y;
                            }

                            R[c] = tab[mx, my];
                            c++;
                        }
                    }

                    Array.Sort(R);
                    nR = (int)((double)R[c / 2] + (double)R[c / 2 - 1]) / 2;
                    rgb = tab[i, j];

                    int s_range = (int)(c * 0.2);

                    for (int s = 0; s < s_range; ++s)
                    {
                        if (nR == R[s] || nR == R[c - 1 - s])
                        {
                            nR = (int)(R[c / 2 + 1]);
                        }

                        rgb = nR;
                        break;
                    }

                    bitmap.SetPixel(i, j, Color.FromArgb(rgb, rgb, rgb));
                    R = null;
                    //System.Diagnostics.Debug.WriteLine("[" + i + "," + j + "]");

                } 
                //Console.WriteLine("["+i+"]");
            }
            tab = null;
            return bitmap;
        }

        public static Bitmap thresholdBernsen(Bitmap bitmap, int size, int delta) {
        
            int pixelAverage, tmp_average;
            int i_left, i_right, j_up, j_down;
        
            int width = bitmap.Width;
            int height = bitmap.Height;
        
            int average = 0;
            int pixel;
            
            int[,] tab = new int[width, height];

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    tab[i, j] = bitmap.GetPixel(i, j).R;
                }
            }

            for (int i = 0; i < width; i++) {
                for (int j = 0; j < height; j++) {
                
                    average += tab[i, j];
                
                }
            }
        
            average = (int)(average / (width * height));
        
            for (int i = 0; i < width; i++) {
                for (int j = 0; j < height; j++) {
                
                    int minPix = 255;
                    int maxPix = 0;
        
                    i_right = i + size;
                    i_left = i - size;
                    j_up = j - size;
                    j_down = j + size;
                
                    j_down = (j_down > height ? height : j_down);
                    i_right = (i_right > width ? width : i_right);
                    j_up = (j_up < 0 ? 0 : j_up);
                    i_left = (i_left < 0 ? 0 : i_left);
               
                    for (int x = i_left; x < i_right; x++) {
                        for (int y = j_up; y < j_down; y++) {
                            pixel = tab[x, y];

                            if(pixel > maxPix) maxPix = pixel;
                            if(pixel < minPix) minPix = pixel;
                        }
                    }
                
                    pixelAverage = (minPix+maxPix)/2;
                
                    if(pixelAverage > average + delta)
                        tmp_average = average+delta;
                    else if(pixelAverage < average - delta)
                        tmp_average = average-delta;
                    else 
                        tmp_average = pixelAverage;
                
                
                    if(tab[i, j] >= tmp_average){
                        bitmap.SetPixel(i, j, Color.White);
                    } else {
                        bitmap.SetPixel(i, j, Color.Black);
                    }
                
                }
            }
            tab = null;
            return bitmap;
        }

        public static Bitmap thresholdAverageGlobal(Bitmap bitmap)
        {
            int width = bitmap.Width;
            int height = bitmap.Height;
            int[,] tab = new int[width, height];
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    tab[i, j] = bitmap.GetPixel(i, j).R;
                }
            }

            int avg = 0;
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    avg += tab[i, j];
                }
            }
            avg = (int)(avg / (width * height));

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    int r = tab[i, j];
                    if (r >= avg)
                    {
                        bitmap.SetPixel(i, j, Color.White);
                    }
                    else
                    {
                        bitmap.SetPixel(i, j, Color.Black);
                    }
                }
            }
            tab = null;
            return bitmap;
        }

        public static Bitmap scale(Bitmap bitmap, float scale)
        {
            int new_width = (int)scale * bitmap.Width;
            int new_height = (int)scale * bitmap.Width;
            Bitmap bm = new Bitmap(new_width, new_height);
            int R, G, B, height, width;
            width = bm.Width;
            height = bm.Height;
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {

                    R = bitmap.GetPixel((int)(i / scale), (int)(j / scale)).R;
                    G = bitmap.GetPixel((int)(i / scale), (int)(j / scale)).G;
                    B = bitmap.GetPixel((int)(i / scale), (int)(j / scale)).B;
                    bm.SetPixel(i, j, Color.FromArgb(R, G, B));

                }
            }
            return bm;
        }

        public static Bitmap markDark(Bitmap bitmap, int diff, Color color)
        {
            for (int i = 0; i < bitmap.Width; i++) {
                for (int j = 0; j < bitmap.Height; j++) {
                    int c = bitmap.GetPixel(i, j).R;
                    
                    if (c < diff) {
                        bitmap.SetPixel(i, j, color);
                    }

                }
            }
            return bitmap;
        }

        public static Bitmap startBlob(Bitmap bitmap, int width, int height)
        {

            int R = 10;

            Boolean[,] blobPoints = new Boolean[width, height];
            Boolean[,] lastAdded = new Boolean[width, height];

            for (int i = 0; i < bitmap.Width; i++) {
                for (int j = 0; j < bitmap.Height; j++) {
                    if (Math.Pow(i, 2) + Math.Pow(j, 2) <= Math.Pow(R, 2)) {
                        blobPoints[i, j] = true;
                        bitmap.SetPixel(i, j, Color.Blue);
                    }
                    if (Math.Pow(i, 2) + Math.Pow(j, 2) == Math.Pow(R, 2)) {
                        lastAdded[i, j] = true;
                    }
                }
            }



            return bitmap;
        }

        public static Bitmap multiplyBy(Bitmap bitmap, float scale)
        {
            int R, height, width;
            width = bitmap.Width;
            height = bitmap.Height;
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    R = (int)(bitmap.GetPixel(i, j).R * scale);
                    if (R > 255) R = 255;
                    if (R < 0) R = 0;
                    bitmap.SetPixel(i, j, Color.FromArgb(R, R, R));

                }
            }
            return bitmap;
        }

        public static Bitmap thresholdBasic(Bitmap bitmap, int p)
        {
            int R, height, width;
            width = bitmap.Width;
            height = bitmap.Height;
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    R = bitmap.GetPixel(i, j).R;
                    
                    if (R > p)
                        R = 255;
                    else
                        R = 0;
                    
                    bitmap.SetPixel(i, j, Color.FromArgb(R, R, R));

                }
            }
            return bitmap;
        }

        public static Point findDarkestInRadius(Bitmap bitmap, int x, int y, int radius)
        {
            int darkestValue = bitmap.GetPixel(x, y).R;
            Point darkestPoint = new Point(x, y); 

            for (int i = 0; i < bitmap.Width; ++i)
            {
                for (int j = 0; j < bitmap.Height; ++j)
                {
                    if (Math.Pow(i-x, 2) + Math.Pow(j-y, 2) <= Math.Pow(radius, 2))
                    {
                        if (bitmap.GetPixel(i, j).R < darkestValue)
                        {
                            darkestValue = bitmap.GetPixel(i, j).R;
                            darkestPoint.x = i;
                            darkestPoint.y = j;
                        }
                    }
                }
            }

            return darkestPoint;
        }
        
        public static Bitmap seedRegionGrowing(Bitmap bitmap, int x, int y)
        {
            List<Point> lists = new List<Point>();
            int[,,] tab = new int[bitmap.Width, bitmap.Height, 3];

            for (var i = 0; i < bitmap.Width; i++)
            {
                for (var j = 0; j < bitmap.Height; j++)
                {
                    tab[i, j, 0] = bitmap.GetPixel(i, j).R;
                    tab[i, j, 1] = bitmap.GetPixel(i, j).G;
                    tab[i, j, 2] = bitmap.GetPixel(i, j).B;
                }
            }

            int threshold = 20;
            //findDarkestInRadius(bitmap, x, y, 10)
            lists.Add(new Point(x, y));
                        
            Point p;
            
            int s = 0;
            while (lists.Count != 0)
            {
                s++;
                p = lists.First();
                bitmap.SetPixel(p.x, p.y, Color.Blue);
                tab[p.x, p.y, 0] = Color.Blue.R;
                tab[p.x, p.y, 1] = Color.Blue.G;
                tab[p.x, p.y, 2] = Color.Blue.B;

                for (var i = -1; i <= 1; ++i)
                {
                    if (p.x + i < 0 || p.x + i > bitmap.Width)
                    {
                        continue;
                    }
                    for (var j = -1; j <= 1; ++j)
                    {
                        if (p.y + j < 0 || p.y + j > bitmap.Height || (i == 0 && j == 0))
                        {
                            continue;
                        }

                        if (Math.Abs(tab[p.x + i, p.y + j, 0] - tab[p.x, p.y, 0]) < threshold &&
                            (tab[p.x + i, p.y + j, 0] != Color.Blue.R && tab[p.x + i, p.y + j, 1] != Color.Blue.G && tab[p.x + i, p.y + j, 2] != Color.Blue.B) &&
                            (tab[p.x + i, p.y + j, 0] != Color.Yellow.R && tab[p.x + i, p.y + j, 1] != Color.Yellow.G && tab[p.x + i, p.y + j, 2] != Color.Yellow.B)
                        ) {
                            lists.Add(new Point(p.x + i, p.y + j));
                            tab[p.x + i, p.y + j, 0] = Color.Yellow.R;
                            tab[p.x + i, p.y + j, 1] = Color.Yellow.G;
                            tab[p.x + i, p.y + j, 2] = Color.Yellow.B;
                        }

                    }
                }
                lists.RemoveAt(0);
                //Console.WriteLine(lists.Count);
                //if (s == 1500000) break;
            }


            return bitmap;
        }
        
        public static Point[] getVertex(int [,] image) 
        {
            List<Point> list = new List<Point>();
            
            for (int x = 0; x < image.GetLength(0); x++)
            {
                for (int y = -1; y < image.GetLength(1); y++)
                {
                    int neighbours = ImageProcessing.getNeighbours(image, new Point(x,y));
                    
                    if (neighbours == 1 || neighbours == 3 || neighbours == 5 || neighbours == 8)
                    {
                        list.Add(new Point(x, y));
                    }
                }
            }
            
            return list.ToArray();
        }

        public static int[,] Otsu(int[,] image)
        {
            int[] histogram = new int[255];
            int width = image.GetLength(0);
            int height = image.GetLength(1);
            for (var i = 0; i < width; i++)
            {
                for (var j = 0; j < height; j++)
                {
                    histogram[image[i, j]]++;// = (histogram[image[i, j]] + 1) ?? 1;
                }
            }

            int total_pixel = width * height;
            int sum = 0;

            for (var i = 0; i < histogram.GetLength(0); i++)
            {
                sum += i * histogram[i];
            }

            float backgroundSum = 0;
            int wageBack = 0;
            int wageFore = 0;

            float varMax = 0;
            int threshold = 0;

            for (var i = 0; i < histogram.GetLength(0); i++)
            {
                wageBack += histogram[i];
                if (wageBack == 0) continue;

                wageFore = total_pixel - wageBack;
                if (wageFore == 0) continue;

                float meanBack = backgroundSum / wageBack;
                float meanFore = (sum - backgroundSum) / wageFore;

                float varBeetwen = (float)wageBack * (float)wageFore * (meanBack - meanFore) * (meanBack - meanFore);

                if (varBeetwen > varMax)
                {
                    varMax = varBeetwen;
                    threshold = i;
                }

            }

            Boolean[,] mapped = new Boolean[width, height];

            for (var i = 0; i < width; i++)
            {
                for (var j = 0; j < height; j++)
                {
                    if (image[i, j] < threshold)
                    {
                        mapped[i, j] = false;
                    }
                    else
                    {
                        mapped[i, j] = true;
                    }

                }
            }

            return null;
        }

        private static int getNeighbours(int[,] image, Point px)
        {
            int c = 0;
            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    if (x == 0 && y == 0)
                    {
                        continue;
                    }

                    if (image[px.x + x, px.y + y] == 1)
                    {
                        c++;
                    }
                }
            }

            return c;
        }
    }
}
