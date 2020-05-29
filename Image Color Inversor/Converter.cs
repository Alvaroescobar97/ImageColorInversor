using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Image_Color_Inversor
{
    class Converter
    {
        public RGB[,] convertToRGB(Image image, PixelFormat p)
        {

            using (Bitmap bitmap = (Bitmap)image)
            using (Bitmap bitmapCopy = new Bitmap(bitmap.Width, bitmap.Height, p))
            {
                using (Graphics gr = Graphics.FromImage(bitmapCopy))
                {
                    gr.DrawImage(bitmap, bitmapCopy.Size.ToRect());
                }

                Console.WriteLine($"Original PixelFormat: {bitmap.PixelFormat}");
                Console.WriteLine($"Copy PixelFormat: {bitmapCopy.PixelFormat}");
                if(p == PixelFormat.Format48bppRgb)
                {
                    return GetValues48(bitmapCopy);
                }
                else if(p == PixelFormat.Format24bppRgb)
                {
                    return GetValues24(bitmapCopy);
                }
                else
                {
                    return GetValues16(bitmapCopy);
                }
            }
        }

        public void convertToImage(Image image, PixelFormat p, RGB[,] matrix)
        {

            using (Bitmap bitmap = (Bitmap)image)
            using (Bitmap bitmapCopy = new Bitmap(bitmap.Width, bitmap.Height, p))
            {
                using (Graphics gr = Graphics.FromImage(bitmapCopy))
                {
                    gr.DrawImage(bitmap, bitmapCopy.Size.ToRect());
                }

                Console.WriteLine($"Original PixelFormat: {bitmap.PixelFormat}"); // Format32bppArgb
                Console.WriteLine($"Copy PixelFormat: {bitmapCopy.PixelFormat}"); //Format48bppRgb
                if(p == PixelFormat.Format48bppRgb)
                {
                    SetValues48(bitmapCopy, matrix);
                    bitmapCopy.Save("../Images/Inverted16bpp.jpeg", ImageFormat.Jpeg);
                    
                }
                else if(p == PixelFormat.Format24bppRgb)
                {
                    SetValues24(bitmapCopy, matrix);
                    bitmapCopy.Save("../Images/Inverted16bpp.jpeg", ImageFormat.Jpeg);
                }
                else
                {
                    SetValues16(bitmapCopy, matrix);
                    bitmapCopy.Save("../Images/Inverted16bpp.jpeg", ImageFormat.Jpeg);
                }
            }
        }

        private static RGB[,] GetValues48(Bitmap bitmap)
        {
            RGB[,] ar = new RGB[bitmap.Height, bitmap.Width];
            BitmapData bitmapData = bitmap.LockBits(bitmap.Size.ToRect(), ImageLockMode.ReadOnly, bitmap.PixelFormat);

            try
            {
                unsafe
                {
                    byte* ppixelRow = (byte*)bitmapData.Scan0;

                    for (int y = 0; y < bitmap.Height; y++)
                    {
                        ushort* ppixelData = (ushort*)ppixelRow;
                        //byte* ppixelData = ppixelRow;

                        for (int x = 0; x < bitmap.Width; x++)
                        {
                            // components are stored in BGR order, i.e. red component last
                            RGB c = new RGB();
                            c.B = ppixelData[0];
                            c.G = ppixelData[1];
                            c.R = ppixelData[2];
                            ar[y, x] = c;
                            //int i = ppixelData[0];
                            ppixelData += 3;
                        }

                        ppixelRow += bitmapData.Stride;
                    }
                }

                return ar;
            }
            finally
            {
                bitmap.UnlockBits(bitmapData);
            }
        }

        private static RGB[,] GetValues24(Bitmap bitmap)
        {
            RGB[,] ar = new RGB[bitmap.Height, bitmap.Width];
            BitmapData bitmapData = bitmap.LockBits(bitmap.Size.ToRect(), ImageLockMode.ReadOnly, bitmap.PixelFormat);

            try
            {
                unsafe
                {
                    byte* ppixelRow = (byte*)bitmapData.Scan0;

                    for (int y = 0; y < bitmap.Height; y++)
                    {
                        //ushort* ppixelData = (ushort*)ppixelRow;
                        byte* ppixelData = ppixelRow;

                        for (int x = 0; x < bitmap.Width; x++)
                        {
                            // components are stored in BGR order, i.e. red component last
                            RGB c = new RGB();
                            c.B = ppixelData[0];
                            c.G = ppixelData[1];
                            c.R = ppixelData[2];
                            ar[y, x] = c;
                            //int i = ppixelData[0];
                            ppixelData += 3;
                        }

                        ppixelRow += bitmapData.Stride;
                    }
                }

                return ar;
            }
            finally
            {
                bitmap.UnlockBits(bitmapData);
            }
        }

        private static RGB[,] GetValues16(Bitmap bitmap)
        {
            RGB[,] ar = new RGB[bitmap.Height, bitmap.Width];
            BitmapData bitmapData = bitmap.LockBits(bitmap.Size.ToRect(), ImageLockMode.ReadOnly, bitmap.PixelFormat);

            try
            {
                unsafe
                {
                    byte* ppixelRow = (byte*)bitmapData.Scan0;

                    for (int y = 0; y < bitmap.Height; y++)
                    {
                        //ushort* ppixelData = (ushort*)ppixelRow;
                        ushort* ppixelData = (ushort*)ppixelRow;

                        for (int x = 0; x < bitmap.Width; x++)
                        {
                            // components are stored in BGR order, i.e. red component last
                            RGB c = new RGB();
                            ushort s = ppixelData[0];
                            var l = divideShort(s);
                            c.B = l[0];
                            c.G = l[1];
                            c.R = l[2];
                            ar[y, x] = c;
                            //int i = ppixelData[0];
                            ppixelData += 1;
                        }

                        ppixelRow += bitmapData.Stride;
                    }
                }

                return ar;
            }
            finally
            {
                bitmap.UnlockBits(bitmapData);
            }
        }

        private static void SetValues48(Bitmap bitmap, RGB[,] mat)
        {
            BitmapData bitmapData = bitmap.LockBits(bitmap.Size.ToRect(), ImageLockMode.ReadWrite, bitmap.PixelFormat);

            try
            {
                unsafe
                {
                    byte* ppixelRow = (byte*)bitmapData.Scan0;

                    for (int y = 0; y < bitmap.Height; y++)
                    {
                        ushort* ppixelData = (ushort*)ppixelRow;
                        //byte* ppixelData = ppixelRow;

                        for (int x = 0; x < bitmap.Width; x++)
                        {
                            // components are stored in BGR order, i.e. red component last
                            ppixelData[0] = (ushort)mat[y, x].B;
                            ppixelData[1] = (ushort)mat[y, x].G;
                            ppixelData[2] = (ushort)mat[y, x].R;
                            //int i = ppixelData[0];
                            ppixelData += 3;
                        }

                        ppixelRow += bitmapData.Stride;
                    }
                }
            }
            finally
            {
                bitmap.UnlockBits(bitmapData);
            }
        }

        private static void SetValues24(Bitmap bitmap, RGB[,] mat)
        {
            BitmapData bitmapData = bitmap.LockBits(bitmap.Size.ToRect(), ImageLockMode.ReadWrite, bitmap.PixelFormat);

            try
            {
                unsafe
                {
                    byte* ppixelRow = (byte*)bitmapData.Scan0;

                    for (int y = 0; y < bitmap.Height; y++)
                    {
                        byte* ppixelData = ppixelRow;
                        //byte* ppixelData = ppixelRow;

                        for (int x = 0; x < bitmap.Width; x++)
                        {
                            // components are stored in BGR order, i.e. red component last
                            ppixelData[0] = (byte)mat[y, x].B;
                            ppixelData[1] = (byte)mat[y, x].G;
                            ppixelData[2] = (byte)mat[y, x].R;
                            var i = ppixelData[0];
                            //int i = ppixelData[0];
                            ppixelData += 3;
                        }

                        ppixelRow += bitmapData.Stride;
                    }
                }
            }
            finally
            {
                bitmap.UnlockBits(bitmapData);
            }
        }

        private static void SetValues16(Bitmap bitmap, RGB[,] mat)
        {
            BitmapData bitmapData = bitmap.LockBits(bitmap.Size.ToRect(), ImageLockMode.ReadWrite, bitmap.PixelFormat);

            try
            {
                unsafe
                {
                    byte* ppixelRow = (byte*)bitmapData.Scan0;

                    for (int y = 0; y < bitmap.Height; y++)
                    {
                        ushort* ppixelData = (ushort*)ppixelRow;
                        //byte* ppixelData = ppixelRow;

                        for (int x = 0; x < bitmap.Width; x++)
                        {
                            // components are stored in BGR order, i.e. red component last
                            ppixelData[0] = mergeBits(mat[y, x].B, mat[y, x].G, mat[y, x].R);
                            //int i = ppixelData[0];
                            ppixelData += 1;
                        }

                        ppixelRow += bitmapData.Stride;
                    }
                }
            }
            finally
            {
                bitmap.UnlockBits(bitmapData);
            }
        }

        public static int[] divideShort(ushort num)
        {
            int[] l = new int[3];
            ushort aux = (ushort)(num + 0);
            l[0] = (aux >> 11) & 0b11111;
            aux = (ushort)(num + 0);
            l[1] = (aux >> 5) & 0b111111;
            aux = (ushort)(num + 0);
            l[2] = aux & 0b11111;
            return l;
        }

        public static ushort mergeBits(int a, int b, int c)
        {
            ushort ret = 0;
            ret += (ushort)(a << 11);
            ret += (ushort)(b << 5);
            ret += (ushort)c;
            return ret;
        }
    }
}
