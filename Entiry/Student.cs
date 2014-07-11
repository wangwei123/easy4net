using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Easy4net.CustomAttributes;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Entiry
{
    [Serializable]
    [Table(Name = "student")]
    public class Student
    {
        //主键 INDENTITY自动增长标识
        [Id(Name = "id", Strategy = GenerationType.INDENTITY)]
        public int? UserID { get; set; }

        [Column(Name = "no")]
        public string No { get; set; }

        //对应数据库中的名字为U_Name
        [Column(Name = "name")]
        public string Name { get; set; }

        [Column(Name = "age")] // int? 允许int为NULL时不会报错
        public long? Age { get; set; }

        [Column(Name = "gender")]
        public string Gender { get; set; }

        [Column(Name = "brithday")]
        public string Brithday { get; set; }

        [Column(Name = "address")]
        public string Address { get; set; }

        [Column(Name = "created")]
        public DateTime? CreateTime { get; set; }

        [Column(Name = "class_id")]
        public int? ClassID { get; set; }
    }
}
