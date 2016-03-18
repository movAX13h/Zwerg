using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

using OpenTK;
using OpenTK.Graphics.OpenGL;

using Utils;

namespace Core
{
    // http://www.opentk.com/node/2559?page=1

    abstract class Asset
    {
        public string Filename { get; protected set; }
        public string Name { get; protected set; }
        public TextureUnit Unit { get; protected set; }
        public virtual Size Size { get { return Size.Empty; } }

        public Asset(string name, string filename, TextureUnit unit)
        {
            Filename = filename;
            Name = name;
            Unit = unit;
        }

        public abstract bool Load();

        public abstract void Bind();
 
        public abstract bool Ready();
    }
}
