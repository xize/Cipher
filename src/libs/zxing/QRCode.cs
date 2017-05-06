/*
    Chiper - A program to chiper passwords based on rijndael encryption.

    Copyright(C) 2017 Guido Lucassen

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program.If not, see<http://www.gnu.org/licenses/>.
*/
using Cipher;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZXing;
using ZXing.Common;

namespace Cipher.src.libs.QRnet
{
    class QRCode
    {

        private static QRCode qr;

        private QRCode(){}

        public Image generateQR(string text, int width, int height)
        {
            if(text.Length == 0)
            {
                return null;
            }
            BarcodeWriter writer = new BarcodeWriter
            {
                Format = BarcodeFormat.QR_CODE,
                Options = new EncodingOptions
                {
                    Height = height,
                    Width = width,
                    Margin = 0
                }
            };

            return writer.Write(text);
        }

        public void setQR(Window w)
        {
            if(w.textBox1.Text.Length > 0 && w.textBox2.Text.Length > 0)
            {

            }

            w.BackgroundImage = generateQR(w.textBox1.Text, 100, 100);
        }

        public string readQR(Image image)
        {
            Bitmap img = new Bitmap(image);
            BarcodeReader reader = new BarcodeReader();

            Result result = reader.Decode(img);

            return result.ToString().Trim();
        }

    public static QRCode getFactory()
        {
            if(qr == null)
            {
                qr = new QRCode();
            }
            return qr;
        }

    }
}
