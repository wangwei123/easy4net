using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using EntityCodeBuilder.Entity;

namespace WindowsDemo
{
    public class CreateFileHelper
    {
        public static void Create(List<TableName> tables, string fileDir)
        {
            CreateDirectory(fileDir);
            foreach (TableName table in tables)
            {
                string entityName = GenVarName(table.Name);
                string filePath = fileDir + entityName + ".cs";
                bool exists = File.Exists(filePath);

                FileStream fs = new FileStream(filePath, exists ? FileMode.Open : FileMode.Create, FileAccess.Write);
                StreamWriter sw = new StreamWriter(fs);

                string code = CreateFileHelper.BuilderCode(table.Name);

                sw.WriteLine(code);

                sw.Close();
                fs.Close();
            }
        }

        private static void CreateDirectory(string targetDir)
        {
            DirectoryInfo dir = new DirectoryInfo(targetDir);
            if (!dir.Exists)
                dir.Create();
        }

        public static string BuilderCode(string tableName)
        {
            string entityName = GenVarName(tableName);

            StringBuilder sb = new StringBuilder();
            sb.Append("using System;").Append("\n");
            sb.Append("using System.Collections.Generic; ").Append("\n");
            sb.Append("using System.Linq;  ").Append("\n");
            sb.Append("using System.Text;  ").Append("\n");
            sb.Append("using System.Text;  ").Append("\n");
            sb.Append("namespace Easy4net.Entity  ").Append("\n");
            sb.Append("{  ").Append("\n");

            sb.Append("\t [Table(Name = \"").Append(tableName).Append("\")] ").Append("\n");
            sb.Append("\t public class ").Append(entityName).Append("\n");
            sb.Append("\t { ").Append("\n");

            List<TableColumn> columns = TableHelper.GetColumnField(tableName);
            foreach (TableColumn column in columns)
            {
                string type = TypeHelper.GetType(column.Type);
                if (column.IsPrimaryKey == "√")
                {
                    //[Id(Name = "UserID", Strategy = GenerationType.INDENTITY)]
                    string strategy = "GUID";
                    if (column.IsIdentity == "√")
                    {
                        strategy = "INDENTITY";
                    }

                    sb.Append("\t\t").Append("[Id(Name = \"").Append(column.Name).Append("\", Strategy = GenerationType.").Append(strategy).Append(")]").Append("\n");
                }
                else
                {
                    sb.Append("\t\t").Append("[Column(Name = \"").Append(column.Name).Append("\")]").Append("\n");
                }

                string fieldName = GenVarName(column.Name);
                sb.Append("\t\t").Append("public ").Append(type).Append(" ").Append(fieldName).Append("{ get; set; } \n\n");
            }

            sb.Append("\t } ").Append("\n");
            sb.Append("}    ").Append("\n");

            return sb.ToString();
        }

        public static string GenVarName(string name)
        {
            string first = name.Substring(0, 1);
            name = name.Substring(1, name.Length - 1);
            name = first.ToUpper() + name;

            int index = name.IndexOf("_");
            while (index != -1)
            {
                if (name.Length >= index + 2)
                {
                    first = name.Substring(index + 1, 1);
                    string start = name.Substring(0, index);
                    string end = name.Substring(index + 2, name.Length - index - 2);
                    name = start + first.ToUpper() + end;

                    index = name.IndexOf("_");
                }
            }

            name = name.Replace("_", "");

            return name;
        }
    }
}
