using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Editor.Inputs;
using Editor.Nodes;
using System.Xml.Linq;
using System.Windows.Forms;

namespace Editor
{
    public class Editor
    {
        public List<DistancePrimitive> DistanceFieldTypes { get; private set; }
        public List<DistanceOperation> DistanceOperations { get; private set; }
        public List<DomainOperation> DomainOperations { get; private set; }

        public Dictionary<string, HelperCode> Helpers { get; private set; }
        
        public PropertiesPanel PropertiesPanel { get; private set; }
        private TreeView tree;

        private string shaderSourceCode;
        public string SceneSourceCode { get; private set; }

        private List<string> errors = new List<string>();
        public string Errors { get { return string.Join(Environment.NewLine, errors.ToArray()); } }

        private int domainId = 0;
        private int primitiveCounter = 0; // used as id for click selection

        public Editor(TreeView treeView, PropertiesPanel propertiesPanel, string source)
        {
            tree = treeView;
            shaderSourceCode = source;
            PropertiesPanel = propertiesPanel;

            Helpers = new Dictionary<string, HelperCode>();
            DistanceFieldTypes = new List<DistancePrimitive>();
            DistanceOperations = new List<DistanceOperation>();
            DomainOperations = new List<DomainOperation>();

            Reset();
        }

        public bool Setup(string functionsXMLFilename)
        {
            try
            {
                XElement x = XElement.Load(functionsXMLFilename, LoadOptions.None);
                
                XElement helpers = x.Element("helpers");
                foreach(XElement h in helpers.Elements("code"))
                {
                    XAttribute comment = h.Attribute("comment");
                    HelperCode helper = new HelperCode(h.Attribute("name").Value, h.Value, comment == null ? "" : comment.Value);
                    Helpers.Add(helper.Name, helper);
                }

                XElement nodes = x.Element("nodes");

                foreach (XElement n in nodes.Elements("node"))
                {                    
                    string code = "";
                    string[] requires = null;

                    XElement func = n.Element("function");
                    if (func != null)
                    {
                        code = func.Value;
                        XAttribute requ = func.Attribute("requires");
                        if (requ != null) requires = requ.Value.Split(",".ToCharArray());
                    }

                    XAttribute com = n.Attribute("comment");
                    string comment = "";
                    if (com != null) comment = com.Value;

                    SceneNode node = null;

                    switch (n.Attribute("type").Value)
                    {
                        case "dp":
                            node = new DistancePrimitive(n.Attribute("name").Value, n.Attribute("caption").Value, n.Attribute("mask").Value, code, requires, comment);
                            DistanceFieldTypes.Add(node as DistancePrimitive);
                            break;

                        case "distop":
                            node = new DistanceOperation(n.Attribute("name").Value, n.Attribute("caption").Value, n.Attribute("mask").Value, code, requires, comment);
                            DistanceOperations.Add(node as DistanceOperation);
                            break;

                        case "domop":
                            node = new DomainOperation(n.Attribute("name").Value, n.Attribute("caption").Value, n.Attribute("mask").Value, code, requires, comment);
                            DomainOperations.Add(node as DomainOperation);
                            break;
                    }

                    if (node != null && n.Element("properties") != null)
                    {
                        foreach (XElement p in n.Element("properties").Elements("property"))
                        {
                            node.Properties.Add(createInputProperty(p.Attribute("type").Value, p.Attribute("name").Value, p.Attribute("default").Value));
                        }
                    }
                }
            }
            catch (Exception e)
            {
                errors.Add("Failed to load/parse "+functionsXMLFilename+":\n"+e.Message);
                return false;
            }

            return true;
        }

        private InputProperty createInputProperty(string type, string name, string value)
        {
            switch(type)
            {
                case "int":
                    return new InputInt(name, value);

                case "float":
                    return new InputFloat(name, value);

                case "vec2":
                    return new InputVec2(name, value);

                case "vec3":
                    return new InputVec3(name, value);

                case "vec4":
                    return new InputVec4(name, value);
            }

            return null;
        }
        
        public void Reset()
        {
            tree.Nodes.Clear();
            primitiveCounter = 0;
        }

        private DistancePrimitive insertDistancePrimitive(DistancePrimitive type, TreeNode parent)
        {
            DistancePrimitive n = new DistancePrimitive(type);
            primitiveCounter++;
            n.Id = primitiveCounter;

            if (parent == null) tree.Nodes.Add(n.TreeNode);
            else parent.Nodes.Add(n.TreeNode);
            return n;
        }

        private DistanceOperation insertDistanceOperation(DistanceOperation type, TreeNode parent)
        {
            DistanceOperation n = new DistanceOperation(type);
            if (parent == null) tree.Nodes.Add(n.TreeNode);
            else parent.Nodes.Add(n.TreeNode);
            return n;
        }

        private DomainOperation insertDomainOperation(DomainOperation type, TreeNode parent)
        {
            DomainOperation n = new DomainOperation(type);
            if (parent == null) tree.Nodes.Add(n.TreeNode);
            else parent.Nodes.Add(n.TreeNode);
            return n;
        }

        public SceneNode InsertNodeByType(string type, TreeNode parent)
        {
            foreach (DistancePrimitive d in DistanceFieldTypes)
            {
                if (d.Type == type) return insertDistancePrimitive(d, parent);
            }

            foreach (DistanceOperation d in DistanceOperations)
            {
                if (d.Type == type) return insertDistanceOperation(d, parent);
            }

            foreach (DomainOperation d in DomainOperations)
            {
                if (d.Type == type) return insertDomainOperation(d, parent);
            }

            return null;
        }

        public void SceneFromXElement(XElement x)
        {
            foreach(XElement cx in x.Elements())
            {
                addNodeFromXElement(cx, null);
            }
        }

        private bool addNodeFromXElement(XElement x, TreeNode parent)
        {
            SceneNode n = InsertNodeByType(x.Attribute("type").Value, parent);
            if (n == null) return false;

            n.ValuesFromXElement(x);

            foreach(XElement cx in x.Elements())
            {
                if (!addNodeFromXElement(cx, n.TreeNode)) return false;
            }

            return true;
        }

        public XElement SceneToXElement()
        {
            XElement scene = new XElement("scene");
            foreach(TreeNode node in tree.Nodes)
            {
                scene.Add(((SceneNode)node.Tag).ToXElement());
            }
            return scene;
        }

        private Dictionary<string, string> uniqueUsedFunctions;

        public string SceneToShaderCode()
        {
            errors.Clear();
            uniqueUsedFunctions = new Dictionary<string, string>();
            domainId = 0;
            SceneSourceCode = nodeToShaderCode(tree.Nodes, "", 0);

            string functionsCode = string.Join("\n\n", uniqueUsedFunctions.Values);
            string code = shaderSourceCode.Replace("#ifdef SCENE", "#ifdef SCENE\n" + Environment.NewLine + SceneSourceCode);
            code = code.Replace("#ifdef FUNCTIONS", "#ifdef FUNCTIONS" + Environment.NewLine + functionsCode);
            return code;
        }

        private SceneNode nodeByType(string type)
        {
            foreach (DistancePrimitive d in DistanceFieldTypes)
            {
                if (d.Type == type) return d;
            }

            foreach (DistanceOperation d in DistanceOperations)
            {
                if (d.Type == type) return d;
            }

            foreach (DomainOperation d in DomainOperations)
            {
                if (d.Type == type) return d;
            }

            return null;
        }

        private void collectUsedFunctions(SceneNode node)
        {
            string type = node.Type;

            // requires first
            if (node.Requires != null)
            {
                foreach (string require in node.Requires)
                {
                    // look in helpers list
                    if (Helpers.ContainsKey(require))
                    {
                        if (!uniqueUsedFunctions.ContainsKey(require)) uniqueUsedFunctions.Add(require, Helpers[require].Code);
                        continue;
                    }

                    // look for node
                    SceneNode requiredNode = nodeByType(require);
                    if (requiredNode != null) collectUsedFunctions(requiredNode);
                }
            }

            // add code (once)
            if (!uniqueUsedFunctions.ContainsKey(type)) uniqueUsedFunctions.Add(type, node.Code);
        }

        private string nodeToShaderCode(TreeNodeCollection nodes, string code, int domain)
        {
            foreach (TreeNode node in nodes)
            {
                SceneNode sceneNode = (SceneNode)node.Tag;
                if (!sceneNode.Enabled) continue;

                collectUsedFunctions(sceneNode);

                // get color property from node
                string color = "vec4(1.0)";
                InputProperty c = sceneNode.GetPropertyByName("color");
                if (c != null) color = "vec4(" + c.Value + ",1.0)";

                // primitive
                if (sceneNode is DistancePrimitive)
                {
                    code += "d1 = " + sceneNode.ToCode(domain) + "; ";
                    code += "if (d1 < d) { d = d1; col = " + color + "; id = " + ((DistancePrimitive)sceneNode).Id + "; }" + Environment.NewLine;
                    if (node.Nodes.Count > 0) code = nodeToShaderCode(node.Nodes, code, domain);
                }
                // distance operation
                else if (sceneNode is DistanceOperation)
                {
                    string[] values = new string[2 + sceneNode.Properties.Count];

                    int i = 0;
                    foreach (TreeNode child in node.Nodes)
                    {
                        SceneNode childSceneNode = (SceneNode)child.Tag;
                        if (!childSceneNode.Enabled) continue;

                        if (childSceneNode is DistancePrimitive)
                        {
                            if (i > 1)
                            {
                                errors.Add("Distance operation '" + sceneNode.Name + "' contains more than 2 subnodes." + Environment.NewLine + "Only the first two are used.");
                                break;
                            }

                            if (i == 0) // take color from first object
                            {
                                c = childSceneNode.GetPropertyByName("color");
                                if (c != null) color = "vec4(" + c.Value + ",1.0)";
                            }

                            collectUsedFunctions(childSceneNode);

                            values[i] = childSceneNode.ToCode(domain);
                            i++;
                        }
                    }

                    if (i == 2)
                    {
                        foreach (InputProperty p in sceneNode.Properties)
                        {
                            values[i] = p.Value;
                            i++;
                        }

                        code += "d1 = " + String.Format(sceneNode.CodeMask, values) + "; ";
                        code += "if (d1 < d) { d = d1; col = " + color + "; }" + Environment.NewLine;
                    }
                    else errors.Add("Distance operation '" + sceneNode.Name + "' requires 2 subnodes.");
                }
                // domain operation
                else if (sceneNode is DomainOperation)
                {
                    int nd = newDomain();
                    code += sceneNode.ToCode(domain, nd) + "; " + Environment.NewLine;
                    if (node.Nodes.Count > 0) code = nodeToShaderCode(node.Nodes, code, nd);
                    else errors.Add("Domain operation '" + sceneNode.Name + "' does not contain any nodes.");
                }
                else errors.Add("Node type '" + sceneNode.Type + "' not implemented.");
            }

            return code;
        }

        private int newDomain()
        {
            domainId++;
            return domainId;
        }
    }
}
