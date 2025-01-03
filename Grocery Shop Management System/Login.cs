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
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string connectionString = "Data Source=LAPTOP-6K25P4MH\\SQLEXPRESS;Initial Catalog=Admin;" +
                "Integrated Security=True";
           // string connectionString1 = ;
            string userType = cmbSelect.SelectedItem.ToString();
            string username = txtUsername.Text;
            string password = txtPassword.Text;
            using (SqlConnection connect = new SqlConnection(connectionString))
            {
                connect.Open();
                string query = "";
                if(userType == "ADMIN")
                {
                    query = "SELECT * FROM MyAdmin WHERE username = @username AND password = @password";
                }
                else if(userType == "CASHIER")
                {
                    query = "SELECT * FROM Casheir WHERE username = @username AND password = @password";
                }
                using (SqlCommand command = new SqlCommand(query, connect))
                {
                    command.Parameters.AddWithValue("@username", username);
                    command.Parameters.AddWithValue("@password", password);
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        MessageBox.Show("Login success!");
                        if (userType == "ADMIN")
                        {
                            AdminForm admin = new AdminForm();
                            admin.Show();
                        }
                        else if (userType == "CASHIER")
                        {
                            CasheirForm cashier = new CasheirForm();
                            cashier.Show();
                        }

                        this.Hide();

                    }
                    else
                    {
                        MessageBox.Show("Invalid username or password");
                    }
                }
                
            }

            


        }
    }
}
