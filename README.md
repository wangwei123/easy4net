easy4net
========

easy4net 是一个轻量级的ORM框架，能够方便的提供增删查改和复杂的SQL语句查询，目前支持MSSQL、Oracle、MySQL、Access数据库。 easy4net技术QQ 群：162695864

新增
-------
```c#
DBHelper db = DBHelper.getInstance();

Student stu = new Student();
stu.Name = "Lily";
stu.Gender = "女";
stu.Age = 23;
stu.Address = "上海市徐汇区中山南二路918弄";

int count = db.Save<Student>(stu);
if (count > 0)
{
　　MessageBox.Show("create success！");
}
```

修改：
------
```c#
stu.UserID = 1;
stu.Name = "Andy";
stu.Age = 22;
db.Update<Student>(stu);
```

删除:
------

```c#
Student student = m_stuList[i];
//remove a object
db.Remove<Student>(student);
//remove by id
db.Remove<Student>(student.UserID);
```

查询:
------

```c#
//查询所有
List<Student> list = DB.FindAll<Student>();

//通过ID主键查询
Student student = DB.FindById<Student>(5);

//通过SQL语句查询
List<Student> list1 = DB.FindBySql<Student>("SELECT * FROM U_Student WHERE U_Age < 28");

//查询某个字段
List<Student> list2 = DB.FindByProperty<Student>("U_Name", "Lily Mary");

// 通过自定义条件查询
// SELECT xxx FROM U_Student WHERE U_Name LIKE '%Lily%' OR U_Age < 28
DbCondition cond1 = new DbCondition().Where().Like("U_Name", "Lily").OrLessThan("U_Age", 28);
List<Student> list3 = DB.Find<Student>(cond1);

// 多表关联查询
DbCondition cond2 = new DbCondition("SELECT s.*,c.teacher,c.className FROM U_Student s INNER JOIN U_Class c ON s.classID = c.ID").Where().RightLike("U_Name","Lil");
List<Student> list4 = DB.Find<Student>(cond2);

//通过条件查询数量
//SELECT count(0) FROM U_Student WHERE U_Name = 'Lily Mary' AND U_Age = 28
DbCondition cond3 = new DbCondition().Where("U_Name", "Andy").And("U_Age", 28);
int count = DB.FindCount<Student>(cond3);


```

实体类
------

```c#
namespace Entiry
{
    [Serializable]
    [Table(Name = "U_Student")]
    public class Student
    {
        //主键自增长
        [Id(Name = "UserID", Strategy = GenerationType.INDENTITY)]
        public int UserID { get; set; }

        //数据库字段U_Name
        [Column(Name = "U_Name")]
        public string Name { get; set; }

        [Column(Name = "U_Age")] // int? 允许int类型为空
        public int? Age { get; set; }

        [Column(Name = "U_Gender")]
        public string Gender { get; set; }

        [Column(Name = "U_Address")]
        public string Address { get; set; }
        
        [Column(Name = "U_CreateTime")]
        public DateTime? CreateTime { get; set; }

        [Column(Name = "ClassID")]
        public int? ClassID { get; set; }

        // 不保存该属性值到数据库库，忽略新增和修改
        [Column(Name = "ClassName",IsInsert=false,IsUpdate=false)]
        public string ClassName { get; set; }

        // 不保存该属性值到数据库库，忽略新增和修改
        [Column(Name = "Teacher", IsInsert = false, IsUpdate = false)]
        public string Teacher { get; set; }
    }
}
```


配置数据库连接
------

```xml
<configuration>
  <appSettings>
    <add key="DbType" value="sqlserver"/>
    <add key="connectionString" value="Data Source=127.0.0.1;Initial Catalog=test;User ID=test;Password=test123;Trusted_Connection=no;Min Pool Size=10;Max Pool Size=100;"/>

    <!--<add key="DbType" value="mysql"/>
    <add key="connectionString" value="Data Source=127.0.0.1;port=8001;User ID=test;Password=123456;DataBase=test;Min Pool Size=10;Max Pool Size=100;"/>-->

    <!--<add key="DbType" value="access"/>
    <add key="connectionString" value="Provider=Microsoft.Jet.OLEDB.4.0;Data Source=|DataDirectory|\tj.mdb"/>-->
  </appSettings>
</configuration>
```
