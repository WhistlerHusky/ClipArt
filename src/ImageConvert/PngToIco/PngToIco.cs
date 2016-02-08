using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AllInOne
{
    public static class PngToIco
    {
        /// <summary>
        /// 원래 Png 파일의 Headfile byte는 은 아래와 같이 시작
        /// </summary>
        private static readonly byte[] PngFileHeaderByte = new byte[] { 137, 80, 78, 71, 13, 10, 26, 10 };

        static PngToIco(){}

        public static Icon ConverPngToIco(string filePath)
        {
            /// 파일 존재 유무 확인
            if (File.Exists(filePath))
            {
                // 파일 확장자 png인지 확인
                if (true == Path.GetExtension(filePath).Equals(".png", StringComparison.OrdinalIgnoreCase))
                {
                    byte[] data = FileToByte(filePath);

                    if (9 > data.Length)
                    {
                        return null;
                    }

                    byte[] pngHeader = new byte[8];

                    Array.Copy(data, 0, pngHeader, 0, 8); //(data, pngHeader, 0, 7);

                    if (true == pngHeader.SequenceEqual(PngFileHeaderByte))
                    {
                        return ConvertPngToIcon(filePath);
                    }
                }
            }

            return null;
        }

        private static Icon ConvertPngToIcon(string filePath)
        {
            Bitmap bitmap = (Bitmap) Image.FromFile(filePath);
            
            return Icon.FromHandle(bitmap.GetHicon());
        }

        private static Byte[] FileToByte(string filePath)
        {
            byte[] fileHeader;

            using (FileStream fs = File.OpenRead(filePath))
            {
                fileHeader = new byte[fs.Length];
                fs.Read(fileHeader, 0, fileHeader.Length);
            }

            return fileHeader;
        }
    }
}
