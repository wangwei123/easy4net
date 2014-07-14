using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Easy4net.CustomAttributes;

namespace Entiry
{
    [Serializable]
    [Table(Name = "student")]
    public class Student
    {
        //主键 INDENTITY自动增长标识
        [Id(Name = "id", Strategy = GenerationType.INDENTITY)]
        public int? Id { get; set; }

        [Column]
        public string No { get; set; }

        //对应数据库中的名字为U_Name
        [Column]
        public string Name { get; set; }

        [Column] // int? 允许int为NULL时不会报错
        public long? Age { get; set; }

        [Column]
        public string Gender { get; set; }

        [Column]
        public string Brithday { get; set; }

        [Column]
        public string Address { get; set; }

        [Column(Name = "created")]
        public DateTime? CreateTime { get; set; }

        [Column(Name = "class_id")]
        public int? ClassID { get; set; }
    }
}
