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
using EntityCodeBuilder.Entity;
using Kevin.SyntaxTextBox;

namespace WindowsDemo
{
    public partial class Form1 : Form
    {
        private SyntaxTextBox txtSyntax;
        public Form1()
        {
            InitializeComponent();
        }

        private DBHelper DB = new DBHelper();
        private List<TableName> m_tables = new List<TableName>();
        private List<TableName> m_SelTables = new List<TableName>();
        private List<TableColumn> m_tableColumns = new List<TableColumn>();


        #region "初始化datagridview数据"
        private void Form1_Load(object sender, EventArgs e)
        {
            m_tables = TableHelper.GetTables();
            dataGridView1.DataSource = m_tables;

            reLoadColumns("student");
        }
        #endregion



        #region "查询所有数据"
        void reLoadColumns(string tablename)
        {
            m_tableColumns.Clear();
            m_tableColumns = TableHelper.GetColumnField(tablename);
            dataGridView2.DataSource = m_tableColumns;
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
                    TableName tb = m_tables[e.RowIndex];
                    reLoadColumns(tb.Name);
                    string code = CreateFileHelper.BuilderCode(tb.Name);
                    code = code.Replace("\n", "\r\n");
                    txtSyntax.Text = code;
                }
            }
        }
        #endregion

        private void btnGen_Click(object sender, EventArgs e)
        {
            m_SelTables.Clear();
            int iRow = 0;
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                int iCol = 0;
                foreach (DataGridViewCell cell in row.Cells)
                {
                    if (iCol == 0)
                    {
                        bool ifcheck1 = Convert.ToBoolean(cell.FormattedValue);
                        if (ifcheck1)
                        {
                            TableName tb = m_tables[iRow];
                            m_SelTables.Add(tb);
                        }
                    }

                    iCol++;
                }

                iRow++;
            }


            CreateFileHelper.Create(m_SelTables, txtPath.Text.Trim());

            MessageBox.Show("代码类文件生成完成，请到文件夹：" + txtPath.Text.Trim() + "中查看");

        }
    }
}
