using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace INVENTORY_MANAGEMENT_SYSTEM
{
    public partial class OrderModuleForm : Form
    {
        SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\dell\Documents\dbMS.mdf;Integrated Security=True;Connect Timeout=30");
        SqlCommand cm = new SqlCommand();
        SqlDataReader dr;

        int qty = 0;

        public OrderModuleForm()
        {
            InitializeComponent();
            LoadCustomer();
            LoadProduct();
        }

        private void pictureBoxclose_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        public void LoadCustomer()
        {
            int i = 0;
            dgvCustomer.Rows.Clear();
            cm = new SqlCommand("SELECT cid, cname FROM tbCustomer WHERE CONCAT(cid, cname) LIKE '%"+textSearchCust.Text+"%' ", con);
            con.Open();
            dr = cm.ExecuteReader();
            while (dr.Read())
            {
                i++;
                dgvCustomer.Rows.Add(i, dr[0].ToString(), dr[1].ToString());
            }
            dr.Close();
            con.Close();
        }

        public void LoadProduct()
        {
            int i = 0;
            dgvProduct.Rows.Clear();
            cm = new SqlCommand("SELECT * FROM tbProduct WHERE CONCAT(pid, pname, pprice, pdescription, pcategory) LIKE '%" + textSearchProd.Text + "%' ", con);
            con.Open();
            dr = cm.ExecuteReader();
            while (dr.Read())
            {
                i++;
                dgvProduct.Rows.Add(i, dr[0].ToString(), dr[1].ToString(), dr[2].ToString(), dr[3].ToString(), dr[4].ToString(), dr[5].ToString());
            }
            dr.Close();
            con.Close();
        }

        private void textSearchCust_TextChanged(object sender, EventArgs e)
        {
            LoadCustomer();
        }

        private void textSearchProd_TextChanged(object sender, EventArgs e)
        {
            LoadProduct();
        }

        

        private void buttonClear_Click(object sender, EventArgs e)
        {
            Clear();
            buttonInsert.Enabled = true;
        }


        

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            GetQty();
            if (Convert.ToInt16(UDQty.Value)>qty)
            {
                MessageBox.Show("Instock quantity is not enough!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                UDQty.Value = UDQty.Value - 1;
                return;
            }
            if (Convert.ToInt16(UDQty.Value) > 0)
            {
                int total = Convert.ToInt16(textPrice.Text) * Convert.ToInt16(UDQty.Value);
                textTotal.Text = total.ToString();
            }
        }

        private void dgvCustomer_CellClick(object sender, DataGridViewCellEventArgs e)
        {

            textCid.Text = dgvCustomer.Rows[e.RowIndex].Cells[1].Value.ToString();
            textCname.Text = dgvCustomer.Rows[e.RowIndex].Cells[2].Value.ToString();
        }

        private void dgvProduct_CellClick(object sender, DataGridViewCellEventArgs e)
        {

            textPid.Text = dgvProduct.Rows[e.RowIndex].Cells[1].Value.ToString();
            textPname.Text = dgvProduct.Rows[e.RowIndex].Cells[2].Value.ToString();
            textPrice.Text = dgvProduct.Rows[e.RowIndex].Cells[4].Value.ToString();

        }

        private void buttonInsert_Click(object sender, EventArgs e)
        {
             try
            {   
                if(textCid.Text == "")
                {
                    MessageBox.Show("Please select Customer", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (textPid.Text == "")
                {
                    MessageBox.Show("Please select Product", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (MessageBox.Show("Are you sure you want to insert this order?", "Saving Record", MessageBoxButtons.YesNo, MessageBoxIcon.Question)== DialogResult.Yes)
                {
                    cm = new SqlCommand("INSERT INTO tbOrder(odate, pid, cid, qty, price, total)VALUES(@odate, @pid, @cid, @qty, @price, @total)", con);
                    cm.Parameters.AddWithValue("@odate", dtOrder.Value);
                    cm.Parameters.AddWithValue("@pid", Convert.ToInt32(textPid.Text));
                    cm.Parameters.AddWithValue("@cid", Convert.ToInt32(textCid.Text));
                    cm.Parameters.AddWithValue("@qty", Convert.ToInt32(UDQty.Value));
                    cm.Parameters.AddWithValue("@price", Convert.ToInt32(textPrice.Text));
                    cm.Parameters.AddWithValue("@total", Convert.ToInt32(textTotal.Text));


                    con.Open();
                    cm.ExecuteNonQuery();
                    con.Close();
                    MessageBox.Show("Order has been SUCCESSFULLY inserted!");
                    

                    cm = new SqlCommand("UPDATE tbProduct SET pqty=(pqty-@pqty) WHERE pid LIKE '" + textPid.Text + "'", con);
                   
                    cm.Parameters.AddWithValue("@pqty", Convert.ToInt16(UDQty.Text));
                    con.Open();
                    cm.ExecuteNonQuery();
                    con.Close();
                    Clear();
                    LoadProduct();

                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void Clear()
        {
            textCid.Clear();
            textCname.Clear();

            textPid.Clear();
            textPname.Clear();

            textPrice.Clear();
            UDQty.Value = 1;
            textTotal.Clear();
            dtOrder.Value = DateTime.Now;

        }

        public void GetQty()
        {
            cm = new SqlCommand("SELECT pqty FROM tbProduct WHERE pid ='"+ textPid.Text +"' ", con);
            con.Open();
            dr = cm.ExecuteReader();
            while (dr.Read())
            {
                qty =Convert.ToInt32(dr[0].ToString());
            }
            dr.Close();
            con.Close();
        }
    }
}
