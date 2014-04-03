using System;
using System.Collections.Generic;
using System.Text;

namespace Easy4net.Common
{
    public class IdInfo
    {
        private String key;
        private Object value;

        public String Key
        {
            get { return key; }
            set { key = value; }
        }        

        public Object Value
        {
            get { return this.value; }
            set { this.value = value; }
        }
    }
}
