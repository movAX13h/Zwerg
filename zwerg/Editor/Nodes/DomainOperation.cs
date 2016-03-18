using System;

namespace Editor.Nodes
{
    public class DomainOperation : SceneNode
    {
        public DomainOperation(string type, string name, string codeMask) : base(type, name, codeMask)
        { }

        public DomainOperation(SceneNode ori) : base("", "", "")
        {
            CloneFrom(ori);
        }
    }
}
