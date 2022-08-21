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
    public partial class UserModuleForm : Form
    {
        SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\dell\Documents\dbMS.mdf;Integrated Security=True;Connect Timeout=30");
        SqlCommand cm = new SqlCommand();

        public UserModuleForm()
        {
            InitializeComponent();
        }

        private void pictureBoxclose_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            try
            {   
                if(textPass.Text != textconPass.Text)
                {
                    MessageBox.Show("Password does not match", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if(MessageBox.Show("Are you sure you want to save this user?", "Saving Record", MessageBoxButtons.YesNo, MessageBoxIcon.Question)== DialogResult.Yes)
                {
                    cm = new SqlCommand("INSERT INTO tbUser(username,fullname,password,phone)VALUES(@username,@fullname,@password,@phone)", con);
                    cm.Parameters.AddWithValue("@username", textUsername.Text);
                    cm.Parameters.AddWithValue("@fullname", textFullname.Text);
                    cm.Parameters.AddWithValue("@password", textPass.Text);
                    cm.Parameters.AddWithValue("@phone", textPhone.Text);
                    con.Open();
                    cm.ExecuteNonQuery();
                    con.Close();
                    MessageBox.Show("User has been SUCCESSFULLY saved!");
                    Clear();
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void buttonClear_Click(object sender, EventArgs e)
        {
            Clear();
            buttonSave.Enabled = true;
            buttonUpdate.Enabled = false;
        }

        public void Clear()
        {

            textUsername.Clear();
            textFullname.Clear();
            textPass.Clear();
            textconPass.Clear();
            textPhone.Clear();

        }

        private void buttonUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (textPass.Text != textconPass.Text)
                {
                    MessageBox.Show("Password does not match", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (MessageBox.Show("Are you sure you want to update this user?", "Updating Record", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    cm = new SqlCommand("UPDATE tbUser SET fullname = @fullname ,password = @password ,phone = @phone WHERE username LIKE '" + textUsername.Text + "'", con);
                    cm.Parameters.AddWithValue("@fullname", textFullname.Text);
                    cm.Parameters.AddWithValue("@password", textPass.Text);
                    cm.Parameters.AddWithValue("@phone", textPhone.Text);
                    con.Open();
                    cm.ExecuteNonQuery();
                    con.Close();
                    MessageBox.Show("User has been SUCCESSFULLY updated!");
                    this.Dispose();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

       
    }
}
