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

        #region "初始化datagridview数据"
        private void Form1_Load(object sender, EventArgs e)
        {
            TableHelper.GetColumnField(AdoHelper.ConnectionString, "student");
            refreshData();
        }
        #endregion

        #region "查询所有数据"
        void refreshData()
        {
            int pageIndex = 1;
            int pageSize = 3;
            string strsql = "SELECT * FROM student where age < 30";
            //m_stuList = DB.FindBySql<Student>(strsql, pageIndex, pageSize);
            //dataGridView1.DataSource = m_stuList;
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
                    //Student student = m_stuList[e.RowIndex];
                }
            }
        }
        #endregion

        #region "根据字段名称和值查询"
        private void btnSearch_Click(object sender, EventArgs e)
        {
            
        }
        #endregion
    }
}
