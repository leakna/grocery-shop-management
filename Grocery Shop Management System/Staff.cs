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

namespace Grocery_Shop_Management_System
{
    public partial class Staff : Form
    {
        private string connectionString = "Data Source=LAPTOP-6K25P4MH\\SQLEXPRESS;Initial Catalog=Admin;" +
                "Integrated Security=True";
        public Staff()
        {
            InitializeComponent();
            dataGridView1.CellContentClick += dataGridView1_CellContentClick;
        }

        private void btnSupplier_Click(object sender, EventArgs e)
        {
            Supplier supplier = new Supplier();
            supplier.Show();
            this.Hide();
        }

        private void btnStock_Click(object sender, EventArgs e)
        {
            AdminForm admin = new AdminForm();
            admin.Show();
            this.Show();
        }

        private void btnLogOut_Click(object sender, EventArgs e)
        {
            Login log = new Login();
            log.Show();
            this.Hide();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (int.TryParse(txtID.Text, out int id))
            {
                // Rest of the code...
                // Use the "id" variable in your SQL query or other operations.
            }
            else
            {
                MessageBox.Show("Invalid ID. Please enter a valid integer.");
            }
            string name = txtName.Text;
            string position = txtPosition.Text;
            string gender = txtGender.Text;
            string contact = txtContact.Text;
            string hire = txtHire.Text;
            using (SqlConnection connect = new SqlConnection(connectionString))
            {
                string query = "INSERT INTO Staff(ID,SName,Position,Gender,Contact,Hire)" +
                    "VALUES(@ID,@SName,@Position,@Gender,@Contact,@Hire)";
                SqlCommand command = new SqlCommand(query, connect);
                command.Parameters.AddWithValue("@ID", Convert.ToInt32(txtID.Text));
                command.Parameters.AddWithValue("@SName", txtName.Text);
                command.Parameters.AddWithValue("@Position", txtPosition.Text);
                command.Parameters.AddWithValue("@Gender", txtGender.Text);
                //command.Parameters.AddWithValue("@Category", cmbCategory.Text);
                //command.Parameters.AddWithValue("@Price", Convert.ToDecimal(txtPrice.Text));
               // command.Parameters.AddWithValue("@Qty", Convert.ToInt32(txtQty.Text));
                //command.Parameters.AddWithValue("@Total", Convert.ToDecimal(txtTotal.Text));
                command.Parameters.AddWithValue("@Contact", txtContact.Text);
                command.Parameters.AddWithValue("@Hire", txtHire.Text);
                //command.Parameters.AddWithValue("@Company", cmbCompany.Text);
                //command.Parameters.AddWithValue("@Address", cmbAddress.Text);
                //command.Parameters.AddWithValue("@PDate", Convert.ToDateTime(txtDate.Text));
                connect.Open();
                command.ExecuteNonQuery();
                connect.Close();
                LoadData();
                ClearFiled();
            }

        }
        private void ClearFiled()
        {
            txtID.Text = "";
            txtName.Text = "";
            txtPosition.Text = "";
            txtGender.Text = "";
            txtContact.Text = "";
            txtHire.Text = "";
        }
        private void LoadData()
        {
            using (SqlConnection connect = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM Staff";
                SqlDataAdapter adapter = new SqlDataAdapter(query, connect);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);
                dataGridView1.DataSource = dataTable;
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (int.TryParse(txtID.Text, out int id))
            {
                string name = txtName.Text;
                string position = txtPosition.Text;
                string gender = txtGender.Text;
                string contact = txtContact.Text;
                string hire = txtHire.Text;

                using (SqlConnection connect = new SqlConnection(connectionString))
                {
                    string query = "UPDATE Staff SET SName = @SName, Position = @Position, Gender = @Gender, Contact = @Contact, Hire = @Hire WHERE ID = @ID";
                    SqlCommand command = new SqlCommand(query, connect);
                    command.Parameters.AddWithValue("@SName", name);
                    command.Parameters.AddWithValue("@Position", position);
                    command.Parameters.AddWithValue("@Gender", gender);
                    command.Parameters.AddWithValue("@Contact", contact);
                    command.Parameters.AddWithValue("@Hire", hire);
                    command.Parameters.AddWithValue("@ID", id);

                    connect.Open();
                    int rowsAffected = command.ExecuteNonQuery();
                    connect.Close();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Staff member updated successfully.");
                        LoadData();
                        ClearFiled();
                    }
                    else
                    {
                        MessageBox.Show("Failed to update staff member.");
                    }
                }
            }
            else
            {
                MessageBox.Show("Invalid ID. Please enter a valid integer.");
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (int.TryParse(txtID.Text, out int id))
            {
                using (SqlConnection connect = new SqlConnection(connectionString))
                {
                    string query = "DELETE FROM Staff WHERE ID = @ID";
                    SqlCommand command = new SqlCommand(query, connect);
                    command.Parameters.AddWithValue("@ID", id);

                    connect.Open();
                    int rowsAffected = command.ExecuteNonQuery();
                    connect.Close();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Staff member deleted successfully.");
                        LoadData();
                        ClearFiled();
                    }
                    else
                    {
                        MessageBox.Show("Failed to delete staff member.");
                    }
                }
            }
            else
            {
                MessageBox.Show("Invalid ID. Please enter a valid integer.");
            }
        }

        

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow selectedRow = dataGridView1.Rows[e.RowIndex];
                txtID.Text = selectedRow.Cells["ID"].Value.ToString();
                txtName.Text = selectedRow.Cells["SName"].Value.ToString();
                txtPosition.Text = selectedRow.Cells["Position"].Value.ToString();
                txtGender.Text = selectedRow.Cells["Gender"].Value.ToString();
                txtContact.Text = selectedRow.Cells["Contact"].Value.ToString();
                txtHire.Text = selectedRow.Cells["Hire"].Value.ToString();
            }
        }

        private void Staff_Load(object sender, EventArgs e)
        {
            LoadData();
        }
    }
}
