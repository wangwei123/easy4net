using System;
using System.Collections.Generic;
using System.Text;

namespace Easy4net.CustomAttributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class TableAttribute : Attribute
    {
        private string _Name = string.Empty;
        
        public TableAttribute() { }

        public string Name
        {
            get { return _Name; }
            set { _Name = value; }
        }        
    }
}
