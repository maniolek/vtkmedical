using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    class Edge
    {
        public Point x;
        public Point y;

        public Edge(Point a, Point b)
        {
            x = a;
            y = b;
        }

        public Edge()
        {
            x = new Point();
            y = new Point();
        }
    }
}
