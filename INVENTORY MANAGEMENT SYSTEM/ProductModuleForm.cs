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
    public partial class ProductModuleForm : Form
    {
        SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\dell\Documents\dbMS.mdf;Integrated Security=True;Connect Timeout=30");
        SqlCommand cm = new SqlCommand();
        SqlDataReader dr;
        public ProductModuleForm()
        {
            InitializeComponent();
            LoadCategory();
        }

        public void LoadCategory()
        {
            comboCat.Items.Clear();
            cm = new SqlCommand("SELECT catname from tbCategory", con);
            con.Open();
            dr = cm.ExecuteReader();
            while(dr.Read())
            {
                comboCat.Items.Add(dr[0].ToString());
            }
            dr.Close();
            con.Close();
        }

        private void pictureBoxclose_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            try
            { 
                if (MessageBox.Show("Are you sure you want to save this product?", "Saving Record", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    cm = new SqlCommand("INSERT INTO tbProduct(pname,pqty,pprice,pdescription,pcategory)VALUES(@pname, @pqty, @pprice, @pdescription, @pcategory)", con);
                    cm.Parameters.AddWithValue("@pname", textPName.Text);
                    cm.Parameters.AddWithValue("@pqty",Convert.ToInt16(textPQty.Text));
                    cm.Parameters.AddWithValue("@pprice",Convert.ToInt16(textPPrice.Text));
                    cm.Parameters.AddWithValue("@pdescription", textPDes.Text);
                    cm.Parameters.AddWithValue("@pcategory", comboCat.Text);
                    con.Open();
                    cm.ExecuteNonQuery();
                    con.Close();
                    MessageBox.Show("Product has been SUCCESSFULLY saved!");
                    Clear();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void Clear()
        {
            textPName.Clear();
            textPQty.Clear();
            textPPrice.Clear();
            textPDes.Clear();
            comboCat.Text = "";

        }

        private void buttonClear_Click(object sender, EventArgs e)
        {
            Clear();
            buttonSave.Enabled = true;
            buttonUpdate.Enabled = false;
        }

        private void buttonUpdate_Click(object sender, EventArgs e)
        {
            try
            {
               
                if (MessageBox.Show("Are you sure you want to update this product?", "Updating Record", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    cm = new SqlCommand("UPDATE tbProduct SET pname= @pname ,pqty = @pqty ,pprice = @pprice ,pdescription = @pdescription ,pcategory = @pcategory WHERE pid LIKE '" + lblPid.Text + "'", con);
                    cm.Parameters.AddWithValue("@pname", textPName.Text);
                    cm.Parameters.AddWithValue("@pqty", Convert.ToInt16(textPQty.Text));
                    cm.Parameters.AddWithValue("@pprice", Convert.ToInt16(textPPrice.Text));
                    cm.Parameters.AddWithValue("@pdescription", textPDes.Text);
                    cm.Parameters.AddWithValue("@pcategory", comboCat.Text);
                    con.Open();
                    cm.ExecuteNonQuery();
                    con.Close();
                    MessageBox.Show("Product has been SUCCESSFULLY updated!");
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
