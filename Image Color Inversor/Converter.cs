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
                //Converts the bitmap to a rgb matrix depending on the color depth
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
            //creates the matrix
            RGB[,] ar = new RGB[bitmap.Height, bitmap.Width];
            BitmapData bitmapData = bitmap.LockBits(bitmap.Size.ToRect(), ImageLockMode.ReadOnly, bitmap.PixelFormat);

            try
            {
                unsafe
                {
                    byte* ppixelRow = (byte*)bitmapData.Scan0;

                    for (int y = 0; y < bitmap.Height; y++)
                    {
                        //creates a pointer of 16 bits
                        ushort* ppixelData = (ushort*)ppixelRow;

                        for (int x = 0; x < bitmap.Width; x++)
                        {
                            // components are stored in BGR order, i.e. red component last
                            //ppixelData has 48 bits, 16 for each color
                            RGB c = new RGB();
                            c.B = ppixelData[0];
                            c.G = ppixelData[1];
                            c.R = ppixelData[2];
                            //add the rgb object to the matrix
                            ar[y, x] = c;
                            //Goes to the next pixel
                            ppixelData += 3;
                        }
                        //Goes to the next row of pixels
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
        //Is the same process of 48 bpp but with a byte instead a short
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
                        //Creates a pointer of 16 bits
                        //In this case, the pointer will not point to a single color but a whole pixel
                        ushort* ppixelData = (ushort*)ppixelRow;

                        for (int x = 0; x < bitmap.Width; x++)
                        {
                            // components are stored in BGR order, i.e. red component last
                            RGB c = new RGB();
                            //Extract the bits of pixel
                            ushort s = ppixelData[0];
                            //Extract the bits of each color
                            int[] l = divideShort(s);
                            c.B = l[0];
                            c.G = l[1];
                            c.R = l[2];
                            ar[y, x] = c;
                            //Goes to the next pixel
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
        //The same process of reading but writing
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

                        for (int x = 0; x < bitmap.Width; x++)
                        {
                            // components are stored in BGR order, i.e. red component last
                            //We have 3 numbers but must be merged in a single short
                            //First 5 bits of blue, 6 bits of green and 5 bits of red
                            //Then writes in the bitmap
                            ppixelData[0] = mergeBits(mat[y, x].B, mat[y, x].G, mat[y, x].R);
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
            //Copy the real number in an auxiliar variable
            ushort aux = (ushort)(num + 0);
            //Shift to the right 11 times and does AND to extract the 5 most significant bits
            //Store it in the first position
            l[0] = (aux >> 11) & 0b11111;
            aux = (ushort)(num + 0);
            //Shift to the right 5 times and does AND to extract the next 6 most significant bits
            l[1] = (aux >> 5) & 0b111111;
            aux = (ushort)(num + 0);
            //Does AND to extract the 5 less significant bits
            l[2] = aux & 0b11111;
            return l;
        }

        public static ushort mergeBits(int a, int b, int c)
        {
            ushort ret = 0;
            //Shift 11 times to the left and sum to add the 5 most siginificant bits
            ret += (ushort)(a << 11);
            //Shift 5 times to the left and sum to add the next 6 most siginificant bits
            ret += (ushort)(b << 5);
            //Sum to add the 5 less siginificant bits
            ret += (ushort)c;
            return ret;
        }
    }
}
