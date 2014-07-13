using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EntityCodeBuilder.Entity
{
    public class TableColumn
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public string IsIdentity { get; set; }
        public string IsPrimaryKey { get; set; }
        public string IsNull { get; set; }
        
    }
}
