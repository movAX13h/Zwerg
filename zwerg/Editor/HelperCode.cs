using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Editor
{
    public class HelperCode
    {
        public string Code { get; private set; }
        public string Name { get; private set; }
        public string Comment { get; private set; }

        public HelperCode(string name, string code, string comment)
        {
            Name = name;
            Code = code;
            Comment = comment;
        }
    }
}
