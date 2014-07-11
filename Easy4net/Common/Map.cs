using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace Easy4net.Common
{
    public class Map : Hashtable
    {
        public void Put(object key,object value)
        {
            if (this.ContainsKey(key)) this.Remove(key);
            this.Add(key, value);
        }

        public void setParameter(string key, object value)
        {
            if (this.ContainsKey(key)) this.Remove(key);
            this.Add(key, value);
        }
    }
}
