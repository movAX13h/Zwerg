using System;

namespace Editor.Nodes
{
    public class DomainOperation : SceneNode
    {
        public DomainOperation(string type, string name, string codeMask, string code) : base(type, name, codeMask, code)
        { }

        public DomainOperation(SceneNode ori) : base("", "", "", "")
        {
            CloneFrom(ori);
        }
    }
}
