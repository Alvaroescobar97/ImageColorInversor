using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;
using System.Diagnostics;

namespace Image_Color_Inversor
{
    class Program
    {
        static void Main(string[] args)
        {
            var times = new List<long>();
            var image = (Bitmap)Image.FromFile("C:/Users/crisf/Pictures/53389.jpg");
            Console.WriteLine(image.Height + "x" + image.Width);
            for (int k = 0; k < 1; k++)
            {
                Stopwatch sw = new Stopwatch();
                sw.Start();
                version5(image);
                sw.Stop();
                var time = (long)(sw.ElapsedMilliseconds * 1000);
                times.Add(time);
            }
            
            Console.WriteLine(times.Sum()/times.Count);
            image.Save("C:/Users/crisf/Pictures/Inverted.jpeg", ImageFormat.Jpeg);
            Console.ReadLine();
        }

        public static void version1(Bitmap image)
        {
            for (int i = 0; i < image.Height; i++)
            {
                for (int j = 0; j < image.Width; j++)
                {
                    Color c = image.GetPixel(j, i);
                    Color nc = Color.FromArgb(c.A, 255 - c.R, 255 - c.G, 255 - c.B);
                    image.SetPixel(j, i, nc);
                }
            }
        }

        public static void version2(Bitmap image)
        {
            for (int i = 0; i < image.Height; i++)
            {
                for (int j = 0; j < image.Width; j++)
                {
                    Color c = image.GetPixel(j, i);
                    Color nc = Color.FromArgb(c.A, 255 - c.R, c.G, c.B);
                    image.SetPixel(j, i, nc);
                }
            }
            for (int i = 0; i < image.Height; i++)
            {
                for (int j = 0; j < image.Width; j++)
                {
                    Color c = image.GetPixel(j, i);
                    Color nc = Color.FromArgb(c.A, c.R, 255 - c.G, c.B);
                    image.SetPixel(j, i, nc);
                }
            }
            for (int i = 0; i < image.Height; i++)
            {
                for (int j = 0; j < image.Width; j++)
                {
                    Color c = image.GetPixel(j, i);
                    Color nc = Color.FromArgb(c.A, c.R, c.G, 255 - c.B);
                    image.SetPixel(j, i, nc);
                }
            }
        }

        public static void version3(Bitmap image)
        {
            for (int i = 0; i < image.Width; i++)
            {
                for (int j = 0; j < image.Height; j++)
                {
                    Color c = image.GetPixel(i, j);
                    Color nc = Color.FromArgb(c.A, 255 - c.R, 255 - c.G, 255 - c.B);
                    image.SetPixel(i, j, nc);
                }
            }
        }

        public static void version4(Bitmap image)
        {
            for (int i = 0; i < image.Height; i++)
            {
                for (int j = 0; j < image.Width; j++)
                {
                    Color c = image.GetPixel(j, i);
                    Color nc = Color.FromArgb(c.A, 255 - c.R, c.G, c.B);
                    image.SetPixel(j, i, nc);
                }
            }
            for (int i = image.Height - 1; i > -1 ; i--)
            {
                for (int j = image.Width - 1; j > -1; j--)
                {
                    Color c = image.GetPixel(j, i);
                    Color nc = Color.FromArgb(c.A, c.R, 255 - c.G, 255 - c.B);
                    image.SetPixel(j, i, nc);
                }
            }
        }

        public static void version5(Bitmap image)
        {
            for (int i = 0; i < image.Height; i+=2)
            {
                for (int j = 0; j < image.Width; j+=2)
                {
                    Color c = image.GetPixel(j, i);
                    Color nc = Color.FromArgb(c.A, 255 - c.R, 255 - c.G, 255 - c.B);
                    image.SetPixel(j, i, nc);

                    c = image.GetPixel(j, i+1);
                    nc = Color.FromArgb(c.A, 255 - c.R, 255 - c.G, 255 - c.B);
                    image.SetPixel(j, i+1, nc);

                    c = image.GetPixel(j+1, i);
                    nc = Color.FromArgb(c.A, 255 - c.R, 255 - c.G, 255 - c.B);
                    image.SetPixel(j+1, i, nc);

                    c = image.GetPixel(j+1, i+1);
                    nc = Color.FromArgb(c.A, 255 - c.R, 255 - c.G, 255 - c.B);
                    image.SetPixel(j+1, i+1, nc);
                }
            }
        }
    }
}
