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
    public partial class UserForm : Form
    {
        SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\dell\Documents\dbMS.mdf;Integrated Security=True;Connect Timeout=30");
        SqlCommand cm = new SqlCommand();
        SqlDataReader dr;

        public UserForm()
        {
            InitializeComponent();
            LoadUser();
        }
    
    public void LoadUser()
        {
            int i = 0;
            dgvUser.Rows.Clear();
            cm = new SqlCommand("SELECT * FROM tbUser", con);
            con.Open();
            dr = cm.ExecuteReader();
            while(dr.Read())
            {
                i++;
                dgvUser.Rows.Add(i,dr[0].ToString(), dr[1].ToString(), dr[2].ToString(), dr[3].ToString());
            }
            dr.Close();
            con.Close();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            UserModuleForm userModule = new UserModuleForm();
            userModule.buttonSave.Enabled = true;
            userModule.buttonUpdate.Enabled = false;
            userModule.ShowDialog();
            LoadUser();
        }

        private void dgvUser_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string colName = dgvUser.Columns[e.ColumnIndex].Name;
            if(colName == "Edit")
            {
                UserModuleForm userModule = new UserModuleForm();
                userModule.textUsername.Text = dgvUser.Rows[e.RowIndex].Cells[1].Value.ToString();
                userModule.textFullname.Text = dgvUser.Rows[e.RowIndex].Cells[2].Value.ToString();
                userModule.textPass.Text = dgvUser.Rows[e.RowIndex].Cells[3].Value.ToString();
                userModule.textPhone.Text = dgvUser.Rows[e.RowIndex].Cells[4].Value.ToString();

                userModule.buttonSave.Enabled = false;
                userModule.buttonUpdate.Enabled = true;
                userModule.textUsername.Enabled = false;
                userModule.ShowDialog();
            }
              else if (colName == "Delete")
            {
                if(MessageBox.Show("Are you sure you want to delete this user?", "Delete Record",MessageBoxButtons.YesNo,MessageBoxIcon.Question)== DialogResult.Yes)
                {
                    con.Open();
                    cm = new SqlCommand("DELETE FROM tbUser WHERE username LIKE'" + dgvUser.Rows[e.RowIndex].Cells[1].Value.ToString() + "'", con);
                    cm.ExecuteNonQuery();
                    con.Close();
                    MessageBox.Show("Record has been SUCCESSFULLY deleted!");
                }
            }
            LoadUser();
                
        }
    }
}
