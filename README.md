easy4net
========

easy4net is a simple orm framework for .net，He can fast to create、update、query、delete，it support database MSSQL、Oracle、MySQL。easy4net QQ group：162695864

insert:
-------
```c#
DBHelper db = DBHelper.getInstance();

Student stu = new Student();
stu.Name = "Lily";
stu.Gender = "Female";
stu.Age = 23;
stu.Address = "Shanghai Zhongsan Road No.305";

int count = db.Save<Student>(stu);
if (count > 0)
{
　　MessageBox.Show("create success！");
}
```

update:
------
```c#
stu.UserID = 1;
stu.Name = "Andy";
stu.Age = 22;
db.Update<Student>(stu);
```

remove:
------

```c#
Student student = m_stuList[i];
//remove a object
db.Remove<Student>(student);
//remove by id
db.Remove<Student>(student.UserID);
```

query:
------

```c#
//query all
List<Student> list = DB.FindAll<Student>();

//query by id
Student student = DB.FindById<Student>(5);

//custom sql command
List<Student> list1 = DB.FindBySql<Student>("SELECT * FROM U_Student WHERE U_Age < 28");

//query by a field
List<Student> list2 = DB.FindByProperty<Student>("U_Name", "Lily Mary");

// query by conditions 
// SELECT xxx FROM U_Student WHERE U_Name LIKE '%Lily%' OR U_Age < 28
DbCondition cond1 = new DbCondition().Where().Like("U_Name", "Lily").OrLessThan("U_Age", 28);
List<Student> list3 = DB.Find<Student>(cond1);

// query for join 
DbCondition cond2 = new DbCondition("SELECT s.*,c.teacher,c.className FROM U_Student s INNER JOIN U_Class c ON s.classID = c.ID").Where().RightLike("U_Name","Lil");
List<Student> list4 = DB.Find<Student>(cond2);

//query count for conditions
//SELECT count(0) FROM U_Student WHERE U_Name = 'Lily Mary' AND U_Age = 28
DbCondition cond3 = new DbCondition().Where("U_Name", "Andy").And("U_Age", 28);
int count = DB.FindCount<Student>(cond3);


```

Entity Mapping Table
------

```c#
namespace Entiry
{
    [Serializable]
    [Table(Name = "U_Student")]
    public class Student
    {
        //primary key INDENTITY 
        [Id(Name = "UserID", Strategy = GenerationType.INDENTITY)]
        public int UserID { get; set; }

        //database column name is U_Name
        [Column(Name = "U_Name")]
        public string Name { get; set; }

        [Column(Name = "U_Age")] // int? allow int is null
        public int? Age { get; set; }

        [Column(Name = "U_Gender")]
        public string Gender { get; set; }

        [Column(Name = "U_Address")]
        public string Address { get; set; }
        
        [Column(Name = "U_CreateTime")]
        public DateTime? CreateTime { get; set; }

        [Column(Name = "ClassID")]
        public int? ClassID { get; set; }

        // don't update and insert to database
        [Column(Name = "ClassName",IsInsert=false,IsUpdate=false)]
        public string ClassName { get; set; }

        // don't update and insert to database
        [Column(Name = "Teacher", IsInsert = false, IsUpdate = false)]
        public string Teacher { get; set; }
    }
}
```


Config database connection
------

```xml
<configuration>
　　　　<appSettings>
            <add key="DbType" value="sqlserver"/>
　　　　　　<add key="connectionString" value="Data Source=.;Initial Catalog=OrmDB;User ID=test;Password=test;Trusted_Connection=no;Min Pool Size=10;Max Pool Size=100;"/>
　　
</appSettings>
```





