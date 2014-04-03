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
    [Table(Name = "U_Student")]
    public class Student
    {
        //主键 INDENTITY自动增长标识
        [Id(Name = "UserID", Strategy = GenerationType.INDENTITY)]
        public int UserID { get; set; }

        //对应数据库中的名字为U_Name
        [Column(Name = "U_Name")]
        public string Name { get; set; }

        [Column(Name = "U_Age")] // int? 允许int为NULL时不会报错
        public int? Age { get; set; }

        [Column(Name = "U_Gender")]
        public string Gender { get; set; }

        [Column(Name = "U_Address")]
        public string Address { get; set; }

        [Column(Name = "U_CreateTime")]
        public DateTime? CreateTime { get; set; }

        [Column(Name = "ClassID")]
        public int? ClassID { get; set; }

        //下面2列 ClassName和Teacher字段是属于班级表中的班级名称和班主任
        //但是因为是外键表，关联的班级编号：ClassID，所以做关联查询可以加这2个属性
        //但是修改和插入则不需要这2列，只做查询，所以加上IsInsert=false,IsUpdate=false
        [Column(Name = "ClassName",IsInsert=false,IsUpdate=false)]
        public string ClassName { get; set; }

        [Column(Name = "Teacher", IsInsert = false, IsUpdate = false)]
        public string Teacher { get; set; }
    }
}
