using System;

namespace Editor.Nodes
{
    public class DomainOperation : SceneNode
    {
        public DomainOperation(string type, string name, string codeMask, string code, string[] requires, string comment) : base(type, name, codeMask, code, requires, comment)
        { }

        public DomainOperation(SceneNode ori) : base("", "", "", "", null, "")
        {
            CloneFrom(ori);
        }
    }
}
