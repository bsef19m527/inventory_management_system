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
    public partial class LoginForm : Form
    {

        SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\dell\Documents\dbMS.mdf;Integrated Security=True;Connect Timeout=30");
        SqlCommand cm = new SqlCommand();
        SqlDataReader dr;

        public LoginForm()
        {
            InitializeComponent();
        }

        
        private void checkBoxpass_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxpass.Checked == false)
                textBoxpass.UseSystemPasswordChar = true;
            else
                textBoxpass.UseSystemPasswordChar = false;
        }

        private void clear_Click(object sender, EventArgs e)
        {
            textBoxname.Clear();
            textBoxpass.Clear();
        }

        private void pictureBoxclose_Click(object sender, EventArgs e)
        {
            if(MessageBox.Show("Exit Application","Confirm",MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void loginbutton_Click(object sender, EventArgs e)
        {
            try
            {
                cm = new SqlCommand("SELECT * FROM tbUser WHERE username=@username AND password=@password", con);
                cm.Parameters.AddWithValue("@username", textBoxname.Text);
                cm.Parameters.AddWithValue("@password", textBoxpass.Text);
                con.Open();
                dr = cm.ExecuteReader();
                dr.Read();
                if(dr.HasRows)
                {
                    MessageBox.Show("Welcome " + dr["fullname"].ToString() + " | ", "ACCESS GRANTED", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    MainForm main = new MainForm();
                    this.Hide();
                    main.ShowDialog();

                }
                else
                {
                    MessageBox.Show("Invalid username or password!", "ACCESS DENITED", MessageBoxButtons.OK, MessageBoxIcon.Information);

                }
                con.Close();
            }
            catch(Exception ex)
            {

            }
        }
    }
}

