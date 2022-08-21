using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
namespace INVENTORY_MANAGEMENT_SYSTEM
{
    public partial class CategoryForm : Form
    {
        SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\dell\Documents\dbMS.mdf;Integrated Security=True;Connect Timeout=30");
        SqlCommand cm = new SqlCommand();
        SqlDataReader dr;

        public CategoryForm()
        {
            InitializeComponent();
            LoadCategory();
        }

        public void LoadCategory()
        {
            int i = 0;
            dgvCategory.Rows.Clear();
            cm = new SqlCommand("SELECT * FROM tbCategory", con);
            con.Open();
            dr = cm.ExecuteReader();
            while (dr.Read())
            {
                i++;
                dgvCategory.Rows.Add(i, dr[0].ToString(), dr[1].ToString());
            }
            dr.Close();
            con.Close();
        }

        private void dgvCategory_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string colName = dgvCategory.Columns[e.ColumnIndex].Name;
            if (colName == "Edit")
            {
                CategoryModuleForm formmodule = new CategoryModuleForm();
                formmodule.labelCatid.Text = dgvCategory.Rows[e.RowIndex].Cells[1].Value.ToString();
                formmodule.textCatName.Text = dgvCategory.Rows[e.RowIndex].Cells[2].Value.ToString();


                formmodule.buttonSave.Enabled = false;
                formmodule.buttonUpdate.Enabled = true;
                formmodule.ShowDialog();
            }
            else if (colName == "Delete")
            {
                if (MessageBox.Show("Are you sure you want to delete this Category?", "Delete Record", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    con.Open();
                    cm = new SqlCommand("DELETE FROM tbCategory WHERE catid LIKE'" + dgvCategory.Rows[e.RowIndex].Cells[1].Value.ToString() + "'", con);
                    cm.ExecuteNonQuery();
                    con.Close();
                    MessageBox.Show("Category has been SUCCESSFULLY deleted!");
                }
            }
            LoadCategory();
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            CategoryModuleForm formmodule = new CategoryModuleForm();
            formmodule.buttonSave.Enabled = true;
            formmodule.buttonUpdate.Enabled = false;
            formmodule.ShowDialog();
            LoadCategory();
           
        }
    }
}
