using Editor.Inputs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace Editor
{
    public class SceneNode
    {
        public string Type;
        private string name;
        public string Name
        {
            get { return name; }
            set
            {
                TreeNode.Text = value;
                name = value;
            }
        }
        public string CodeMask;
        public TreeNode TreeNode { get; private set; }
        public bool Enabled = true;

        public List<InputProperty> Properties { get; private set; }

        public SceneNode(string type, string name, string codeMask)
        {
            this.name = name;
            Type = type;
            CodeMask = codeMask;

            Properties = new List<InputProperty>();

            TreeNode = new TreeNode(Name);
            TreeNode.Tag = this;
            TreeNode.Checked = Enabled;
        }

        public string ToCode(int domain)
        {
            return ToCode(domain, domain);
        }

        public string ToCode(int domain, int newDomain)
        {
            if (CodeMask.Length < 1) return "";

            string code = CodeMask.Replace("{A}", "p" + domain.ToString()); // insert domain variable
            if (domain != newDomain) code = code.Replace("{B}", "p" + newDomain.ToString()); // insert domain variable

            if (Properties.Count == 0) return code; // return code if there is no property

            string[] values = new string[Properties.Count];
            int i = 0;

            foreach (InputProperty p in Properties)
            {
                values[i] = p.Value;
                i++;
            }

            return String.Format(code, values);
        }

        public XElement ToXElement()
        {
            XElement x = new XElement("node",
                new XAttribute("name", Name),
                new XAttribute("type", Type),
                new XAttribute("enabled", Enabled)
                );

            foreach (InputProperty prop in Properties)
            {
                x.Add(new XAttribute(prop.Name, prop.Value));
            }

            foreach(TreeNode child in TreeNode.Nodes)
            {
                x.Add((child.Tag as SceneNode).ToXElement());
            }

            return x;
        }

        public void ValuesFromXElement(XElement x)
        {
            Name = x.Attribute("name").Value;
            Type = x.Attribute("type").Value;
            Enabled = bool.Parse(x.Attribute("enabled").Value);
            TreeNode.Checked = Enabled;

            foreach (InputProperty prop in Properties)
            {
                if (x.Attribute(prop.Name) != null) prop.Value = x.Attribute(prop.Name).Value;
            }
        }

        protected void CloneFrom(SceneNode ori)
        {
            Type = ori.Type;
            Name = ori.Name;
            CodeMask = ori.CodeMask;
            Properties = new List<InputProperty>();
            
            foreach(InputProperty prop in ori.Properties)
            {
                Properties.Add(prop.Clone());
            }
        }

        public InputProperty GetPropertyByName(string name)
        {
            foreach (InputProperty prop in Properties)
            {
                if (prop.Name == name) return prop;
            }
            return null;
        }
    }

}
