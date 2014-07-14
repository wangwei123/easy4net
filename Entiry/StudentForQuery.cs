using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Easy4net.CustomAttributes;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Entiry
{


    /*
     * 注意：
     * 
     * Easy4net查询可以不用配置任何信息，只需要保证属性名和数据库字段名称一致即可
     * 如果需要属性名和数据库名称不一致，则需要配置做映射配置
     * 如果需要做修改和删除等操作，则需要做配置
     **/

    //[Table(Name = "student")]
    public class StudentForQuery
    {
        //主键 INDENTITY自动增长标识
        //[Id(Name = "id", Strategy = GenerationType.INDENTITY)]
        public int? Id { get; set; }

        //[Column]
        public string No { get; set; }

        //对应数据库中的名字为U_Name
        //[Column]
        public string Name { get; set; }

        //[Column] // int? 允许int为NULL时不会报错
        public long? Age { get; set; }

        //[Column]
        public string Gender { get; set; }

        //[Column]
        public string Brithday { get; set; }

        //[Column]
        public string Address { get; set; }

        //[Column(Name = "created")]
        //public DateTime? CreateTime { get; set; }

        public DateTime? Created { get; set; }

        //[Column(Name = "class_id")]
        //public int? ClassID { get; set; }

        public int? Class_id { get; set; }
    }
}
