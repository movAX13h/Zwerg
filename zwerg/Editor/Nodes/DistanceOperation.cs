using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Editor.Nodes
{
    public class DistanceOperation : SceneNode
    {
        public DistanceOperation(string type, string name, string codeMask, string code, string[] requires, string comment) : base(type, name, codeMask, code, requires, comment)
        { }

        public DistanceOperation(SceneNode ori) : base("", "", "", "", null, "")
        {
            CloneFrom(ori);
        }
    }
}
