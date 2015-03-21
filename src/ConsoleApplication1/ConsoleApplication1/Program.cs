using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Diagnostics;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            
            String path = "C:/Users/Mateusz/Documents/Visual Studio 2012/Projects/ConsoleApplication1/ConsoleApplication1/bin/Debug/assets/p01";
            
            int [][,] image = Reader.readBitmapSet(path);

            foreach (int[,] bmp in image)
            {
                int[,] res = ImageProcessing.Otsu(bmp);

            }

            Console.ReadLine();
        }
        static void Main2(string[] args)
        {
            String projectPath = "C:/Users/Mateusz/Documents/Visual Studio 2012/Projects/ConsoleApplication1/ConsoleApplication1/bin/Debug/assets/p01/";
            String dirPath = "";
            String filenamePrefix = "out_";
            String filenameExt = "bmp";
            String path;
            int t0 = 1;
            int tk = 3;

            List<List<Point>> _points = new List<List<Point>>();
            List<Edge> _edges = new List<Edge>();

            int slices_count = tk - t0 + 1;

            for (int t = t0; t <= tk; ++t)
            {
                path = projectPath + dirPath + filenamePrefix + t + "." + filenameExt;
                int[,] image = Reader.fromFile(path);
                Point[] points = ImageProcessing.getVertex(image);

                int image_w = image.GetLength(0);
                int image_h = image.GetLength(1);

                SortedDictionary<String, Point> dict = new SortedDictionary<string, Point>();
                foreach (Point p in points)
                {
                    double angle = Math.Atan2(p.y - image_h/2, p.x - image_w/2);
                    dict.Add(angle.ToString(), p);
                }

                /**
                 * Save each slice vertexes at list, contains of connected points (understand as edges)
                 */
                String prevKey = "";
                String firstKey = null;
                List<Point> slicePoints = new List<Point>();
                foreach (KeyValuePair<String, Point> entry in dict)
                {
                    if(firstKey == null) {
                        firstKey = entry.Key;
                    }
                    if (!prevKey.Equals(""))
                    {
                        Point beforePoint;
                        dict.TryGetValue(prevKey, out beforePoint);

                        _edges.Add(new Edge(beforePoint, entry.Value));
                    }
                    slicePoints.Add(entry.Value);
                    prevKey = entry.Key;
                }
                _edges.Add(new Edge(dict[prevKey], dict[firstKey]));
                _points.Add(slicePoints);
                slicePoints.Clear();
            }

            /**
             * 
             * Now _edges stores all edges for loaded object, retrives by alone points. What about two white blobs? Region growing based algorithm.
             * 
             */
            

            /**
             * 
             * normalization
             * 
             */
            //coordinates goes to around 0,0

            /**
             * 
             * interpolation on z axis
             * 
             */
            

            

        }

        private void firstStage()
        {
            String projectPath = "C:/Users/Mateusz/Documents/Visual Studio 2012/Projects/ConsoleApplication1/ConsoleApplication1/bin/Debug/";
            String dirPath = "assets/p01/";
            String fileName = "out_";
            String fileExtension = ".png";
            String path;
            int t0 = 219;
            int tk = 319;
            //int t0 = 219;
            //int tk = 219;

            Bitmap bm = null;
            int timeElapsed = 0;

            Stopwatch sw = Stopwatch.StartNew();

            for (int t = t0; t <= tk; ++t)
            {
                path = projectPath + dirPath + fileName + t + fileExtension;
                Console.WriteLine(fileName + t + fileExtension + " -> output/out-" + t + ".png");

                using (var image = new Bitmap(path))
                {
                    bm = new Bitmap(image);
                }

                //bm = ImageProcessing.getCut(bm, 610, 510, 160, 100);

                bm = ImageProcessing.filterMedian2(bm, 3);
                bm = ImageProcessing.seedRegionGrowing(bm, 510, 325);
                //bm = ImageProcessing.thresholdBasic(bm, 40);
                //bm = ImageProcessing.startBlob(bm, 370, 220);
                //bm = ImageProcessing.markDark(bm, 10, Color.Blue);
                //bm = ImageProcessing.thresholdAverageGlobal(bm);

                if (bm != null)
                {
                    bm.Save("output/seed-out-" + t + ".png");
                }

                timeElapsed += (int)sw.Elapsed.TotalMilliseconds;

                bm = null;
            }


            sw.Stop();

            Console.WriteLine("Time taken: {0}ms", sw.Elapsed.TotalMilliseconds);
            Console.ReadLine();
        }
    }
}
