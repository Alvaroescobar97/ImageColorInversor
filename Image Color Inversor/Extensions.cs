using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Image_Color_Inversor
{
    static class Extensions
    {
        public static Rectangle ToRect(this Size size)
        {
            return new Rectangle(new Point(), size);
        }
    }
}
