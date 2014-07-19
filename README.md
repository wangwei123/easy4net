easy4net
========

easy4net是一个轻量级orm框架，灵活在于可以自己编写复杂的SQL语句查询，简单在于几分钟内便能上手使用，并支持mysql, mssql, oracle。 easy4net技术QQ 群：162695864

**分页查询：**
* 1. 命名参数， ParamMap传参方式：
* 2. 支持多层嵌套查询自动分页功能。

***

```c#
int pageIndex = 1;
int pageSize = 3;
string sql = "SELECT * FROM (select * from student where age < @age or address= @address) as v";
ParamMap param = ParamMap.newMap();
param.setParameter("age",24);
param.setParameter("address", "上海市");
param.setPageIndex(pageIndex);
param.setPageSize(pageSize);
List<Student> student = DB.FindBySql<Student>(sql, param);
```

**分页查询：**

***

```c#
int pageIndex = 1;
int pageSize = 3;
string sql = "SELECT * FROM student WHERE age < 28";
List<Student> list1 = DB.FindBySql<Student>(sql , pageIndex, pageSize);
```

**更多查询方式：**

***

```c#
//查询所有
List list = DB.FindAll();
 
//通过ID主键查询
Student student = DB.FindById<Student>(5);
 
//通过SQL语句查询
List<Student> list1 = DB.FindBySql<Student><span></span>("SELECT * FROM U_Student WHERE U_Age < 28");
 
//查询某个字段
List<Student> list2 = DB.FindByProperty<Student>("U_Name", "Lily Mary");
 
// 通过自定义条件查询
// SELECT xxx FROM U_Student WHERE U_Name LIKE '%Lily%' OR U_Age < 28
DbCondition cond1 = new DbCondition().Where().Like("U_Name", "Lily").OrLessThan(
 "U_Age", 28);
List<Student> list3 = DB.Find<Student>(cond1);

// 多表关联查询
DbCondition cond2 = new DbCondition("SELECT s.*,c.teacher,c.className FROM U_Student s "
 "INNER JOIN U_Class c ON s.classID = c.ID").Where().RightLike("U_Name","Lil");
List<Student> list4 = DB.Find<Student>(cond2);

//通过条件查询数量
//SELECT count(0) FROM U_Student WHERE U_Name = 'Lily Mary' AND U_Age = 28
DbCondition cond3 = new DbCondition().Where("U_Name", "Andy").And("U_Age", 28);
int count = DB.FindCount<Student>(cond3);

```


**新增：**
* 1. 新增后返回新增记录的主键id值
* 2. 主键id值已经自动填充到新增的对象entity中

***

```c#
DBHelper db = DBHelper.getInstance();
Student entity = new Student();
entity.Name = "Lily";
entity.Gender = "女";
entity.Age = 23;
entity.Address = "上海市徐汇区中山南二路918弄";
int id = db.Save(entity);
```

**修改：**

***

```c#
Student entity = new Student();
entity.UserID = 1;
entity.Name = "Andy";
entity.Age = 22;
db.Update(entity);
```

**删除：**
* 1. 按对象方式删除数据
* 2. 按主键id方式删除数据

***

```c#
Student student = m_stuList[i];
//remove a object
db.Remove(student);
//remove by id
db.Remove(student.UserID);
```

**数据库与JAVA对象映射关系配置：**

***

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

**数据库连接配置 web.config中： **
* dbType中配置sqlserver, mysql, oracle来支持不同的数据库

***

```xml
<configuration>
  <appSettings>
    <add key="DbType" value="sqlserver"/>
    <add key="connectionString" value="Data Source=127.0.0.1;Initial Catalog=test;User ID=test;Password=test123;Trusted_Connection=no;Min Pool Size=10;Max Pool Size=100;"/>

    <!--<add key="DbType" value="mysql"/>
    <add key="connectionString" value="Data Source=127.0.0.1;port=8001;User ID=test;Password=123456;DataBase=test;Min Pool Size=10;Max Pool Size=100;"/>-->
  </appSettings>
</configuration>
```
