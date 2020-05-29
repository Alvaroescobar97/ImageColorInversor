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
        public static Dictionary<int, int> max = new Dictionary<int, int>();
        public static Dictionary<int, PixelFormat> bpp = new Dictionary<int, PixelFormat>();
        static void Main(string[] args)
        {
            max.Add(16, 31);
            max.Add(24, 255);
            max.Add(48, 8192);
            bpp.Add(16, PixelFormat.Format16bppRgb565);
            bpp.Add(24, PixelFormat.Format24bppRgb);
            bpp.Add(48, PixelFormat.Format48bppRgb);
            Converter c = new Converter();
            //BitsPerPixel
            var bp = 16;
            //File
            var filepath = filenames[3];
            var times = new List<double>();
            var image = c.convertToRGB(Image.FromFile("../Images/" + filepath + ".jpg"), bpp[bp]);

            Console.WriteLine("Image size: " + filepath + ". " + image.GetLength(0) + "x" + image.GetLength(1));
            StreamWriter writer = new StreamWriter("../Data/times.txt");
            for (int k = 0; k < 3; k++)
            {
                Stopwatch sw = new Stopwatch();
                //Console.WriteLine(image.Cast<RGB>().ToList().Select(x=> x.B).Max());
                //Console.WriteLine(image.Cast<RGB>().ToList().Select(x => x.G).Max());
                //Console.WriteLine(image.Cast<RGB>().ToList().Select(x => x.R).Max());
                sw.Start();
                //Version
                version5(image, max[bp]);
                sw.Stop();
                //Console.WriteLine(image.Cast<RGB>().ToList().Select(x => x.B).Average());
                double time = sw.ElapsedMilliseconds * 1000000;
                times.Add(time);
            }
            Console.WriteLine("Mean time: {0}", times.Sum()/times.Count);
            writer.Close();
            c.convertToImage(Image.FromFile("../Images/" + filepath + ".jpg"), bpp[bp], image);
        }

        public static void version1(RGB[,] image, int max)
        {
            for (int i = 0; i < image.GetLength(0); i++)
            {
                for (int j = 0; j < image.GetLength(1); j++)
                {
                    image[i, j].R = max - image[i, j].R;
                    if(max == 31)
                    {
                        image[i, j].G = 63 - image[i, j].G;
                    }
                    else
                    {
                        image[i, j].G = max - image[i, j].G;
                    }
                    
                    image[i, j].B = max - image[i, j].B;
                }
            }
        }

        public static void version2(RGB[,] image, int max)
        {
            for (int i = 0; i < image.GetLength(0); i++)
            {
                for (int j = 0; j < image.GetLength(1); j++)
                {
                    image[i, j].R = max - image[i, j].R;
                }
            }
            for (int i = 0; i < image.GetLength(0); i++)
            {
                for (int j = 0; j < image.GetLength(1); j++)
                {
                    if (max == 31)
                    {
                        image[i, j].G = 63 - image[i, j].G;
                    }
                    else
                    {
                        image[i, j].G = max - image[i, j].G;
                    }
                }
            }
            for (int i = 0; i < image.GetLength(0); i++)
            {
                for (int j = 0; j < image.GetLength(1); j++)
                {
                    image[i, j].B = max - image[i, j].B;
                }
            }
        }

        public static void version3(RGB[,] image, int max)
        {
            for (int i = 0; i < image.GetLength(1); i++)
            {
                for (int j = 0; j < image.GetLength(0); j++)
                {
                    image[j, i].R = max - image[j, i].R;
                    if (max == 31)
                    {
                        image[j, i].G = 63 - image[j, i].G;
                    }
                    else
                    {
                        image[j, i].G = max - image[j, i].G;
                    }

                    image[j, i].B = max - image[j, i].B;
                }
            }
        }

        public static void version4(RGB[,] image, int max)
        {
            for (int i = 0; i < image.GetLength(0); i++)
            {
                for (int j = 0; j < image.GetLength(1); j++)
                {
                    image[i, j].R = max - image[i, j].R;
                }
            }
            for (int i = image.GetLength(0) - 1; i > -1 ; i--)
            {
                for (int j = image.GetLength(1) - 1; j > -1; j--)
                {
                    if (max == 31)
                    {
                        image[i, j].G = 63 - image[i, j].G;
                    }
                    else
                    {
                        image[i, j].G = max - image[i, j].G;
                    }

                    image[i, j].B = max - image[i, j].B;
                }
            }
        }

        public static void version5(RGB[,] image, int max)
        {
            for (int i = 0; i < image.GetLength(0); i+=2)
            {
                for (int j = 0; j < image.GetLength(1); j+=2)
                {
                    image[i, j].R = max - image[i, j].R;
                    image[i+1, j].R = max - image[i+1, j].R;
                    image[i, j+1].R = max - image[i, j+1].R;
                    image[i+1, j+1].R = max - image[i+1, j+1].R;
                    if (max == 31)
                    {
                        image[i, j].G = 63 - image[i, j].G;
                        image[i + 1, j].G = 63 - image[i + 1, j].G;
                        image[i, j + 1].G = 63 - image[i, j + 1].G;
                        image[i + 1, j + 1].G = 63 - image[i + 1, j + 1].G;
                    }
                    else
                    {
                        image[i, j].G = max - image[i, j].G;
                        image[i + 1, j].G = max - image[i + 1, j].G;
                        image[i, j + 1].G = max - image[i, j + 1].G;
                        image[i + 1, j + 1].G = max - image[i + 1, j + 1].G;
                    }

                    image[i, j].B = max - image[i, j].B;
                    image[i + 1, j].B = max - image[i + 1, j].B;
                    image[i, j + 1].B = max - image[i, j + 1].B;
                    image[i + 1, j + 1].B = max - image[i + 1, j + 1].B;
                }
            }
        }
    }
}
