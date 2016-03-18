using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

using OpenTK;
using OpenTK.Graphics.OpenGL;

using Utils;

namespace Core.Assets
{
    class TextureAsset : Asset
    {
        private Bitmap bitmap;
        private int handle;
        public override Size Size { get { return bitmap.Size; } }

        public TextureAsset(string name, string filename, TextureUnit unit) : base(name, filename, unit)
        {
            
        }

        public override bool Load()
        {
            bitmap = null;

            if (File.Exists(Filename))
            {
                bitmap = new Bitmap(Filename);
                CreateTexture();

                Log.WriteLine(Log.LOG_INFO, "asset ready '" + Name + "', " + Filename);
                return true;
            }
            else Log.WriteLine(Log.LOG_ERROR, "asset not found '" + Name + "', " + Filename);
            return false;
        }

        private void CreateTexture()
        {
            Log.WriteLine(Log.LOG_INFO, "creating texture '" + Name + "'");

            // load texture 
            GL.GenTextures(1, out handle);
            GL.BindTexture(TextureTarget.Texture2D, handle);

            BitmapData data = bitmap.LockBits(new System.Drawing.Rectangle(0, 0, bitmap.Width, bitmap.Height),
                ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, data.Width, data.Height, 0,
                OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);

            bitmap.UnlockBits(data);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
        }

        public override void Bind()
        {
            GL.ActiveTexture(Unit);
            GL.BindTexture(TextureTarget.Texture2D, handle);
        }

        public override bool Ready()
        {
            return bitmap != null;
        }

        /*
         public void Paint(float time)
         {
             Bind();

             int num = 512;
             byte[] freqData = new byte[num];
             byte[] waveData = new byte[num];

             for (var j = 0; j < num; j++)
             {
                 var x = j / num;
                 var f = (0.75 + 0.25 * Math.Sin(10.0 * j + 13.0 * time)) * Math.Exp(-3.0 * x);

                 if (j < 3)
                     f = Math.Pow(0.50 + 0.5 * Math.Sin(6.2831 * time), 4.0) * (1.0 - j / 3.0);

                 freqData[j] = (byte)((byte)Math.Floor(255.0 * f) | 0);
             }

             for (var j = 0; j < num; j++)
             {
                 var f = 0.5 + 0.15 * Math.Sin(17.0 * time + 10.0 * 6.2831 * j / num) * Math.Sin(23.0 * time + 1.9 * j / num);
                 waveData[j] = (byte)((byte)Math.Floor(255.0 * f) | 0);
             }

             GL.TexSubImage2D(TextureTarget.Texture2D, 0, 0, 0, num, 1, OpenTK.Graphics.OpenGL.PixelFormat.Luminance, PixelType.UnsignedByte, freqData);
             GL.TexSubImage2D(TextureTarget.Texture2D, 0, 0, 1, num, 1, OpenTK.Graphics.OpenGL.PixelFormat.Luminance, PixelType.UnsignedByte, waveData);

             //GL.TexSubImage2D( gl.TEXTURE_2D, 0, 0, 0, num, 1, gl.LUMINANCE, gl.UNSIGNED_BYTE, inp.audio.mSound.mFreqData);
             //GL.TexSubImage2D(gl.TEXTURE_2D, 0, 0, 1, num, 1, gl.LUMINANCE, gl.UNSIGNED_BYTE, inp.audio.mSound.mWaveData);
         }
         */
    }
}
