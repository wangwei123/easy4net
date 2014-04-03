using System;
using System.Collections.Generic;
using System.Text;

namespace Easy4net.CustomAttributes
{
    public class GenerationType 
    {        
        public const int INDENTITY = 1;//自动增长
        public const int SEQUENCE = 2;//序列
        public const int TABLE = 3;//TABLE

        private GenerationType() { }//私有构造函数，不可被实例化对象
    }
}
