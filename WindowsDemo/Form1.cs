using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Easy4net;
using Easy4net.DBUtility;
using Entiry;
using Easy4net.Common;

namespace WindowsDemo
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private DBHelper DB = new DBHelper();
        private Student updateStudent = new Student();
        private List<Student> m_stuList = new List<Student>();

        #region "初始化datagridview数据"
        private void Form1_Load(object sender, EventArgs e)
        {
            refreshData();
        }
        #endregion

        #region "查询所有数据"
        void refreshData()
        {
            m_stuList = DB.FindAll<Student>();
            dataGridView1.DataSource = m_stuList;
        }
        #endregion

        #region "新增学员信息"
        private void btnOK_Click(object sender, EventArgs e)
        {
            Student stu = new Student();
            stu.Name = txtName.Text;

            String age = txtAge.Text.Trim();
            if (age.Length == 0) stu.Age = null;
            if (age.Length > 0) stu.Age =  Convert.ToInt32(txtAge.Text);
            stu.Gender = txtGender.Text;
            stu.Address = txtAddress.Text;

            int count = DB.Save<Student>(stu);
            if (count > 0) {
                MessageBox.Show("新增成功！");
                refreshData();
            }
        }
        #endregion

        #region "修改学员信息"
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            updateStudent.Name = txtName.Text;
            updateStudent.Age = Convert.ToInt32(txtAge.Text);
            updateStudent.Gender = txtGender.Text;
            updateStudent.Address = txtAddress.Text;
            DB.Update<Student>(updateStudent);

            refreshData();
        }
        #endregion

        #region "删除学员信息"
        private void btnDelete_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                object value = dataGridView1.Rows[i].Cells["selectedRow"].Value;
                if (value != null && value.Equals(true))
                {
                    Student student = m_stuList[i];

                    //删除 2中方式删除，可根据ID删除，DB.Remove<Student>(student.UserID);
                    DB.Remove<Student>(student);
                }
            }

            refreshData();
        }
        #endregion

        #region "选择行，并填充到表单中，然后可做修改和删除操作"
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 0)//当单击复选框，同时处于组合编辑状态时 
            {
                DataGridViewCell cell = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex];
                bool ifcheck1 = Convert.ToBoolean(cell.FormattedValue);
                bool ifcheck2 = Convert.ToBoolean(cell.EditedFormattedValue);

                if (ifcheck1 != ifcheck2)
                {
                    Student student = m_stuList[e.RowIndex];
                    txtName.Text = student.Name;
                    txtAge.Text = student.Age.ToString();
                    txtGender.Text = student.Gender;
                    txtAddress.Text = student.Address;
                    updateStudent.UserID = student.UserID;
                }
            }
        }
        #endregion

        #region "根据字段名称和值查询"
        private void btnSearch_Click(object sender, EventArgs e)
        {
            string field = txtField.Text.Trim();
            string value = txtValue.Text.Trim();

            if (field == "" || value == "")
            {
                refreshData();
            }
            else
            {
                //查询所有学员信息
                List<Student> list = DB.FindAll<Student>();

                //根据ID查询
                Student student = DB.FindById<Student>(5);

                //自定义SQL查询
                List<Student> list1 = DB.FindBySql<Student>("SELECT * FROM U_Student WHERE U_Age < 28");

                //按某个列查询
                List<Student> list2 = DB.FindByProperty<Student>("U_Name", "张三");

                //按精确条件查询，这里是SELECT xxx FROM U_Student WHERE U_Name LIKE '%张%' OR U_Age < 28
                DbCondition cond1 = new DbCondition().Where().Like("U_Name", "张").OrLessThan("U_Age", 28);
                List<Student> list3 = DB.Find<Student>(cond1);

                //关联查询，这个不用多说了，会SQL的都知道，查询条件是 WHERE U_Name LIKE '张%'
                DbCondition cond2 = new DbCondition("SELECT s.*,c.teacher,c.className FROM U_Student s INNER JOIN U_Class c ON s.classID = c.ID").Where().RightLike("U_Name", "张");
                List<Student> list4 = DB.Find<Student>(cond2);

                //这里是查询 SELECT count(0) FROM U_Student WHERE U_Name = '张三' AND U_Age = 28
                DbCondition cond3 = new DbCondition().Where("U_Name", "张三").And("U_Age", 28);
                int count = DB.FindCount<Student>(cond3);

                DbCondition condition = new DbCondition();
                //模糊查找名称和或者年龄<21的人
                //condition.Where().Like("U_Name", value).OrLessThan("U_Age", 21);

                condition.Where(field, value).OrderByDESC("UserID");
                m_stuList = DB.Find<Student>(condition);

                dataGridView1.DataSource = m_stuList;
            }
        }
        #endregion
    }
}
