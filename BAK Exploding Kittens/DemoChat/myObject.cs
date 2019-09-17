using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoChat
{
    [Serializable]
    class myObject
    {
        private string str;
        private string id;
        
        public myObject()
        { }

        public string Str { get => str; set => str = value; }
        public string Id { get => id; set => id = value; }
    }
}
