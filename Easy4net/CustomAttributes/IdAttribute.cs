using System;
using System.Collections.Generic;
using System.Text;

namespace Easy4net.CustomAttributes
{
    [AttributeUsage(AttributeTargets.Field|AttributeTargets.Property, 
        AllowMultiple = false, Inherited = false)]
    public class IdAttribute : Attribute
    {
        private string _Name = string.Empty;
        private int _Strategy = GenerationType.INDENTITY;

        public string Name
        {
            get { return _Name; }
            set { _Name = value; }
        }
        
        public int Strategy
        {
            get { return _Strategy; }
            set { _Strategy = value; }
        }        
    }
}
