
namespace TaskBoard
{
    using System;
    using System.Drawing;
    using System.Drawing.Imaging;

    public class OCRPreprocessor
    {
        public static Bitmap PreprocessForOCR(Bitmap bitmap, int threshold)
        {
            // Convert the bitmap to grayscale
            Bitmap grayscaleBitmap = ConvertToGrayscale(bitmap);

            // Apply binarization to convert the image to binary (black and white)
            Bitmap binaryBitmap = Binarize(grayscaleBitmap, threshold);

            // Apply noise reduction to the binary image
            //Bitmap noiseReducedBitmap = ReduceNoise(binaryBitmap);

            // Enhance contrast to make text more prominent
            //Bitmap enhancedBitmap = EnhanceContrast(noiseReducedBitmap);

            return binaryBitmap;
        }

        private static Bitmap ConvertToGrayscale(Bitmap bitmap)
        {
            Bitmap grayscaleBitmap = new Bitmap(bitmap.Width, bitmap.Height);

            for (int y = 0; y < bitmap.Height; y++)
            {
                for (int x = 0; x < bitmap.Width; x++)
                {
                    Color pixel = bitmap.GetPixel(x, y);
                    int grayscaleValue = (int)(pixel.R * 0.3 + pixel.G * 0.59 + pixel.B * 0.11);
                    grayscaleBitmap.SetPixel(x, y, Color.FromArgb(grayscaleValue, grayscaleValue, grayscaleValue));
                }
            }

            return grayscaleBitmap;
        }

        private static Bitmap Binarize(Bitmap grayscaleBitmap, int threshold)
        {
            Bitmap binaryBitmap = new Bitmap(grayscaleBitmap.Width, grayscaleBitmap.Height);

            for (int y = 0; y < grayscaleBitmap.Height; y++)
            {
                for (int x = 0; x < grayscaleBitmap.Width; x++)
                {
                    Color pixel = grayscaleBitmap.GetPixel(x, y);
                    //int threshold = 105; // You can adjust the threshold as needed
                    Color binaryColor = pixel.R > threshold ? Color.White : Color.Black;
                    binaryBitmap.SetPixel(x, y, binaryColor);
                }
            }

            return binaryBitmap;
        }

        private static Bitmap ReduceNoise(Bitmap binaryBitmap)
        {
            // Apply a simple median filter for noise reduction

            Bitmap noiseReducedBitmap = new Bitmap(binaryBitmap.Width, binaryBitmap.Height);

            for (int y = 1; y < binaryBitmap.Height - 1; y++)
            {
                for (int x = 1; x < binaryBitmap.Width - 1; x++)
                {
                    int[] values = new int[9];

                    // Get the values of the current pixel and its neighbors
                    values[0] = (int)binaryBitmap.GetPixel(x - 1, y - 1).R;
                    values[1] = (int)binaryBitmap.GetPixel(x, y - 1).R;
                    values[2] = (int)binaryBitmap.GetPixel(x + 1, y - 1).R;
                    values[3] = (int)binaryBitmap.GetPixel(x - 1, y).R;
                    values[4] = (int)binaryBitmap.GetPixel(x, y).R;
                    values[5] = (int)binaryBitmap.GetPixel(x + 1, y).R;
                    values[6] = (int)binaryBitmap.GetPixel(x - 1, y + 1).R;
                    values[7] = (int)binaryBitmap.GetPixel(x, y + 1).R;
                    values[8] = (int)binaryBitmap.GetPixel(x + 1, y + 1).R;

                    // Sort the values
                    Array.Sort(values);

                    // Set the median value as the new pixel value
                    int medianValue = values[4];
                    noiseReducedBitmap.SetPixel(x, y, Color.FromArgb(medianValue, medianValue, medianValue));
                }
            }

            return noiseReducedBitmap;
        }

        private static Bitmap EnhanceContrast(Bitmap bitmap)
        {
            // Apply histogram equalization for contrast enhancement

            BitmapData bitmapData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height),
                                                    ImageLockMode.ReadWrite,
                                                    bitmap.PixelFormat);

            int bytesPerPixel = Bitmap.GetPixelFormatSize(bitmap.PixelFormat) / 8;
            int byteCount = bitmapData.Stride * bitmap.Height;
            byte[] pixels = new byte[byteCount];
            IntPtr ptrFirstPixel = bitmapData.Scan0;

            // Copy the RGB values into the array
            System.Runtime.InteropServices.Marshal.Copy(ptrFirstPixel, pixels, 0, pixels.Length);

            // Calculate histogram
            int[] histogram = new int[256];
            for (int i = 0; i < pixels.Length; i += bytesPerPixel)
            {
                int gray = (int)(pixels[i] * 0.3 + pixels[i + 1] * 0.59 + pixels[i + 2] * 0.11);
                histogram[gray]++;
            }

            // Calculate cumulative distribution function (CDF)
            int sum = 0;
            int[] cdf = new int[256];
            for (int i = 0; i < 256; i++)
            {
                sum += histogram[i];
                cdf[i] = sum;
            }

            // Normalize the CDF
            int minValue = cdf.Min();
            int maxValue = cdf.Max();
            int[] normalizedCdf = new int[256];
            for (int i = 0; i < 256; i++)
            {
                normalizedCdf[i] = (int)(((double)(cdf[i] - minValue) / (maxValue - minValue)) * 255);
            }

            // Apply histogram equalization to each pixel
            for (int i = 0; i < pixels.Length; i += bytesPerPixel)
            {
                int gray = (int)(pixels[i] * 0.3 + pixels[i + 1] * 0.59 + pixels[i + 2] * 0.11);
                byte newValue = (byte)normalizedCdf[gray];
                pixels[i] = newValue;
                pixels[i + 1] = newValue;
                pixels[i + 2] = newValue;
            }

            // Copy the modified pixels array back to the bitmap
            System.Runtime.InteropServices.Marshal.Copy(pixels, 0, ptrFirstPixel, pixels.Length);

            bitmap.UnlockBits(bitmapData);

            return bitmap;
        }
    }

}
