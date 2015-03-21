using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace ConsoleApplication1
{
    class RGB : Color
    {
        protected int R;
        protected int G;
        protected int B;

        public int getR()
        {
            return (int)this.R;
        }
        public int getG()
        {
            return (int)this.G;
        }
        public int getB()
        {
            return (int)this.B;
        }
    }
}
