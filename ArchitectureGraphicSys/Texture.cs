using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using OpenTK.Graphics.OpenGL4;
using StbImageSharp;
using ImageMagick;
using PixelFormat = OpenTK.Graphics.OpenGL4.PixelFormat;


namespace ArchitectureGraphicSys
{
    public class Texture
    {
        private int _textureObject;
        private readonly string _filename;
        private readonly TextureTarget _textureTarget;

        public Texture(TextureTarget textureTarget, string filename)
        {
            _filename = filename;
            _textureTarget = textureTarget;
        }

        public bool Load()
        {
            try
            {

                GL.Hint(HintTarget.PerspectiveCorrectionHint, HintMode.Nicest);
                _textureObject = GL.GenTexture();
                GL.BindTexture(_textureTarget, _textureObject);

                using (Stream stream = File.OpenRead(_filename))
                {
                    ImageResult image = ImageResult.FromStream(stream, ColorComponents.RedGreenBlueAlpha);
                    
                    GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, image.Width, image.Height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, image.Data);
                }
                
                GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);
                
                GL.TexParameter(_textureTarget, TextureParameterName.TextureMinFilter, (float)TextureMinFilter.Linear);
                GL.TexParameter(_textureTarget, TextureParameterName.TextureMagFilter, (float)TextureMinFilter.Linear);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);

                GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);
                    
            }
            catch (FileNotFoundException)
            {
                return false;
            }

            return true;
        }


        public void Bind(TextureUnit unit)
        {
            GL.ActiveTexture(unit);
            GL.BindTexture(_textureTarget, _textureObject);
        }
    }
}