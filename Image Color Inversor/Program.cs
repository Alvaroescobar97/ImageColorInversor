using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;
using System.Diagnostics;
using System.IO;

namespace Image_Color_Inversor
{
    class Program
    {
        public static string[] filenames = { "64x64", "160x160", "512x512", "1500x1500" };
        static void Main(string[] args)
        {
            var filepath = filenames[0];
            var times = new List<double>();
            var image = ConvertTo16bpp(Image.FromFile("../Images/" + filepath + ".jpg"));
            Console.WriteLine(image.Height + "x" + image.Width);
            StreamWriter writer = new StreamWriter("../Data/times.txt");
            for (int k = 0; k < 60; k++)
            {
                Stopwatch sw = new Stopwatch();
                sw.Start();
                version1(image);
                sw.Stop();
                double time = sw.ElapsedMilliseconds * 1000;
                Console.WriteLine(times.Count + 1 + ". " + time);
                writer.WriteLine(Math.Round(time / (image.Height * image.Width), 4));
                times.Add(time/(image.Height*image.Width));
            }
            
            Console.WriteLine("Mean time: {0}", times.Sum()/times.Count);
            writer.Close();
            image.Save("../Images/" + filepath + "Inverted.jpeg", ImageFormat.Jpeg);
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

        public static Bitmap ConvertTo16bpp(Image img)
        {
            var bmp = new Bitmap(img.Width, img.Height, PixelFormat.Format16bppRgb555);
            using (var gr = Graphics.FromImage(bmp))
                gr.DrawImage(img, new Rectangle(0, 0, img.Width, img.Height));
            return bmp;
        }

        public static Bitmap ConvertTo24bpp(Image img)
        {
            var bmp = new Bitmap(img.Width, img.Height, PixelFormat.Format24bppRgb);
            using (var gr = Graphics.FromImage(bmp))
                gr.DrawImage(img, new Rectangle(0, 0, img.Width, img.Height));
            return bmp;
        }

        public static Bitmap ConvertTo48bpp(Image img)
        {
            var bmp = new Bitmap(img.Width, img.Height, PixelFormat.Format48bppRgb);
            using (var gr = Graphics.FromImage(bmp))
                gr.DrawImage(img, new Rectangle(0, 0, img.Width, img.Height));
            return bmp;
        }
    }
}
