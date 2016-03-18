using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Editor.Nodes
{
    public class DistanceOperation : SceneNode
    {
        public DistanceOperation(string type, string name, string codeMask) : base(type, name, codeMask)
        { }

        public DistanceOperation(SceneNode ori) : base("", "", "")
        {
            CloneFrom(ori);
        }
    }
}
