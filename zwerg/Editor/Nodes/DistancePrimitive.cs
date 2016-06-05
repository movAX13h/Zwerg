using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Editor.Nodes
{
    public class DistancePrimitive : SceneNode
    {
        public int Id = 0;

        public DistancePrimitive(string type, string name, string codeMask, string code, string[] requires, string comment) : base(type, name, codeMask, code, requires, comment)
        { }

        public DistancePrimitive(SceneNode ori) : base("", "", "", "", null, "")
        {
            CloneFrom(ori);
        }

        public Vector3 Position()
        {
            InputProperty prop = GetPropertyByName("position");
            if (prop == null) return Vector3.Zero;

            string[] values = prop.Value.Split(',');
            if (values.Length != 3) return Vector3.Zero;

            return new Vector3(float.Parse(values[0]), float.Parse(values[1]), float.Parse(values[2]));
        }
    }
}
