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
using System.Web.Compilation;
using System.Globalization;
using System.Drawing.Printing;

namespace Grocery_Shop_Management_System
{
    public partial class AdminForm : Form
    {
        //Connectiong to database
        private string connectionString = "Data Source=LAPTOP-6K25P4MH\\SQLEXPRESS;Initial Catalog=Admin;" +
                "Integrated Security=True";
        public AdminForm()
        {
            InitializeComponent();
        }
        //Button add item
        private void btnAdd_Click(object sender, EventArgs e)
        {
            int id = int.Parse(txtID.Text);
            string name = txtName.Text;
            string category = cmbCategory.Text;
            decimal price = decimal.Parse(txtPrice.Text);
            int qty = int.Parse(txtQty.Text);
            decimal total = price * qty;
            txtTotal.Text = total.ToString();
            string date = txtDate.Text;
            //database statement
            using(SqlConnection connect = new SqlConnection(connectionString))
            {
                string query = "INSERT INTO Stock(ID,PName,Category,Price,Qty,Total,PDate)" +
                    "VALUES(@ID,@PName,@Category,@Price,@Qty,@Total,@PDate)";
                SqlCommand command = new SqlCommand(query, connect);
                command.Parameters.AddWithValue("@ID", Convert.ToInt32(txtID.Text));
                command.Parameters.AddWithValue("@PName", txtName.Text);
                command.Parameters.AddWithValue("@Category", cmbCategory.Text);
                command.Parameters.AddWithValue("@Price", Convert.ToDecimal(txtPrice.Text));
                command.Parameters.AddWithValue("@Qty", Convert.ToInt32(txtQty.Text));
                command.Parameters.AddWithValue("@Total", Convert.ToDecimal(txtTotal.Text));
                command.Parameters.AddWithValue("@PDate", Convert.ToDateTime(txtDate.Text));
                connect.Open();
                command.ExecuteNonQuery();
                connect.Close();
                LoadData();
                ClearFiled();
            }
        }
        private void Calculate()
        {
            if(decimal.TryParse(txtPrice.Text,out decimal price)&&
                int.TryParse(txtQty.Text,out int quantity))
            {
                decimal total = price * quantity;
                txtTotal.Text = total.ToString("c");
            }
        }

        private void AdminForm_Load(object sender, EventArgs e)
        {
            //load data to data gridview
            LoadData();
            //load category option to cobobox search
            LoadCategoryOption();
        }
        private void LoadData()
        {
            using (SqlConnection connect = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM Stock";
                SqlDataAdapter adapter = new SqlDataAdapter(query, connect);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);
                dataGridView1.DataSource = dataTable;
                //CalculateTotalAmount();
                //CalculateKhmerAmount();
            }
            CalculateTotalAmount();
            CalculateKhmerAmount();
        }
        private void CalculateTotalAmount()
        {
            decimal totalAmount = 0;
            foreach(DataGridViewRow row in dataGridView1.Rows)
            {
                if (!row.IsNewRow)
                {
                    decimal total = Convert.ToDecimal(row.Cells["Total"].Value);
                    totalAmount += total;
                }
            }
            txtDollar.Text = totalAmount.ToString("c");
        }
        private void CalculateKhmerAmount()
        {
            if (decimal.TryParse(txtDollar.Text, NumberStyles.Currency, CultureInfo.CurrentCulture, out decimal amount))
            {
                decimal khmerAmount = amount * 4000;
                txtKhmer.Text = khmerAmount.ToString()+"R";
            }
            else
            {
                txtKhmer.Text = "";
            }
        }
        //Method clear Input
        private void ClearFiled()
        {
            txtID.Text = "";
            txtName.Text = "";
            cmbCategory.Text = "";
            txtPrice.Text = "";
            txtQty.Text = "";
            txtPrice.Text = "";
            txtDate.Text = "";
            txtTotal.Text = "";
            txtDollar.Text = "";
            txtKhmer.Text = "";
        }

        private void txtPrice_TextChanged(object sender, EventArgs e)
        {
            Calculate();
        }

        private void txtQty_TextChanged(object sender, EventArgs e)
        {
            Calculate();
        }
        //button edit item
        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = dataGridView1.SelectedRows[0];
                string selectedID = selectedRow.Cells["ID"].Value.ToString();

                int id = int.Parse(txtID.Text);
                string name = txtName.Text;
                string category = cmbCategory.Text;
                decimal price = decimal.Parse(txtPrice.Text);
                int qty = int.Parse(txtQty.Text);
                decimal total = price * qty;
                txtTotal.Text = total.ToString();
                string date = txtDate.Text;
                //database statement
                using(SqlConnection connect = new SqlConnection(connectionString))
                {
                    string query = "UPDATE Stock SET ID=@ID, PName=@PName,Category=@Category," +
                        "Price=@Price,Qty=@Qty,Total=@Total,PDate=@PDate WHERE ID=@SelectedID";
                    SqlCommand command = new SqlCommand(query, connect);
                    command.Parameters.AddWithValue("@ID", Convert.ToInt32(txtID.Text));
                    command.Parameters.AddWithValue("@PName", txtName.Text);
                    command.Parameters.AddWithValue("@Category", cmbCategory.Text);
                    command.Parameters.AddWithValue("@Price", Convert.ToDecimal(txtPrice.Text));
                    command.Parameters.AddWithValue("@Qty", Convert.ToInt32(txtQty.Text));
                    command.Parameters.AddWithValue("@Total", Convert.ToDecimal(txtTotal.Text));
                    command.Parameters.AddWithValue("@PDate", Convert.ToDateTime(txtDate.Text));
                    command.Parameters.AddWithValue("@SelectedID", selectedID);
                    connect.Open();
                    command.ExecuteNonQuery();
                    connect.Close();
                    MessageBox.Show("Edit Successfully!");
                    LoadData();
                    ClearFiled();
                }

            }
            else
            {
                MessageBox.Show("Please select a row to edit");
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if(e.RowIndex >=0 && e.RowIndex < dataGridView1.Rows.Count)
            {
                DataGridViewRow selectedRow = dataGridView1.Rows[e.RowIndex];
                txtID.Text = selectedRow.Cells["ID"].Value.ToString();
                txtName.Text = selectedRow.Cells["PName"].Value.ToString();
                cmbCategory.Text = selectedRow.Cells["Category"].Value.ToString();
                txtPrice.Text = selectedRow.Cells["Price"].Value.ToString();
                txtQty.Text = selectedRow.Cells["Qty"].Value.ToString();
                //Calculate the total based on price and qty
                if(decimal.TryParse(txtPrice.Text,out decimal price) && int.TryParse(txtQty.Text,out int quantity))
                {
                    decimal total = price * quantity;
                    txtTotal.Text = total.ToString("c");
                }
                else
                {
                    txtTotal.Text = "";
                }
                txtDate.Text = selectedRow.Cells["PDate"].Value.ToString();
            }
        }
        //button delete item
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = dataGridView1.SelectedRows[0];
                string selectedID = selectedRow.Cells["ID"].Value.ToString();
                DialogResult result = MessageBox.Show("Are you sure want to delete?",
                    "Confirmation", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    using(SqlConnection connect = new SqlConnection(connectionString))
                    {
                        string query = "DELETE FROM Stock WHERE ID=@SelectedID";
                        SqlCommand command = new SqlCommand(query, connect);
                        command.Parameters.AddWithValue("@SelectedID", selectedID);
                        connect.Open();
                        command.ExecuteNonQuery();
                        connect.Close();
                        LoadData();
                        ClearFiled();
                    }
                }

            }
            else
            {
                MessageBox.Show("Select a row for delete!");
            }
        }

       
        private void LoadCategoryOption()
        {
            using(SqlConnection connect = new SqlConnection(connectionString))
            {
                string query = "SELECT DISTINCT Category FROM Stock";
                SqlDataAdapter adapter = new SqlDataAdapter(query,connect);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);
                //Clear existing category
                cmbSearch.Items.Clear();
                //Add category option to search
                foreach(DataRow row in dataTable.Rows)
                {
                    cmbSearch.Items.Add(row["Category"].ToString());
                }
            }
        }

        private void cmbSearch_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            string selectedCategory = cmbSearch.Text;
            using (SqlConnection connect = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM Stock WHERE Category=@Category";
                SqlDataAdapter adapter = new SqlDataAdapter(query, connect);
                adapter.SelectCommand.Parameters.AddWithValue("@Category", selectedCategory);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);
                dataGridView1.DataSource = dataTable;
            }
            CalculateTotalAmount();
            decimal amount = decimal.Parse(txtDollar.Text, NumberStyles.Currency);
            decimal khmerAmount = amount * 4000;
            txtKhmer.Text = khmerAmount.ToString()+"R";

        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadData();
            LoadCategoryOption();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {

            PrintDocument printDocument = new PrintDocument();
            printDocument.PrintPage += printDocument1_PrintPage;

            PrintPreviewDialog printPreviewDialog = new PrintPreviewDialog();
            printPreviewDialog.Document = printDocument;

            printPreviewDialog.ShowDialog();
        }

        private void printDocument1_PrintPage(object sender, PrintPageEventArgs e)
        {

            // Set up the printing content
            string headerText = "Management Stock List";

            Font headerFont = new Font("Arial", 16, FontStyle.Bold);
            StringFormat headerFormat = new StringFormat();
            headerFormat.Alignment = StringAlignment.Center;

            Font contentFont = new Font("Arial", 12);
            Brush contentBrush = Brushes.Black;

            float lineHeight = contentFont.GetHeight();

            // Set up the printing area
            float startX = e.MarginBounds.Left;
            float startY = e.MarginBounds.Top;
            float currentY = startY;
            float availableHeight = e.MarginBounds.Height;

            // Print the header
            e.Graphics.DrawString(headerText, headerFont, contentBrush, new RectangleF(startX, currentY, e.MarginBounds.Width, lineHeight * 2), headerFormat);
            currentY += lineHeight * 2;
            availableHeight -= lineHeight * 2;

            // Print the table headers
            string[] columnHeaders = { "ID", "Product Name", "Category", "Price", "Quantity", "Total", "Date" };
            float[] columnWidths = { 50, 150, 100, 80, 80, 100, 100 };

            Color headerBackColor = Color.LightBlue;
            Brush headerBrush = new SolidBrush(headerBackColor);

            for (int i = 0; i < columnHeaders.Length; i++)
            {
                RectangleF headerRect = new RectangleF(startX, currentY, columnWidths[i], lineHeight);
                e.Graphics.FillRectangle(headerBrush, headerRect);

                // Add bold formatting to specific column headers
                if (columnHeaders[i] == "ID" || columnHeaders[i] == "Product Name" || columnHeaders[i] == "Category" || columnHeaders[i] == "Price" || columnHeaders[i] == "Quantity" || columnHeaders[i] == "Total" || columnHeaders[i] == "Date")
                {
                    Font boldHeaderFont = new Font(contentFont, FontStyle.Bold);
                    e.Graphics.DrawString(columnHeaders[i], boldHeaderFont, contentBrush, headerRect);
                }
                else
                {
                    e.Graphics.DrawString(columnHeaders[i], contentFont, contentBrush, headerRect);
                }

                startX += columnWidths[i];
            }

            currentY += lineHeight;
            availableHeight -= lineHeight;

            // Print the table data
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (!row.IsNewRow)
                {
                    startX = e.MarginBounds.Left;

                    for (int i = 0; i < columnHeaders.Length; i++)
                    {
                        string cellValue = row.Cells[i].Value.ToString();
                        e.Graphics.DrawString(cellValue, contentFont, contentBrush, new RectangleF(startX, currentY, columnWidths[i], lineHeight));
                        startX += columnWidths[i];
                    }

                    currentY += lineHeight;
                    availableHeight -= lineHeight;

                    if (currentY > e.MarginBounds.Bottom)
                    {
                        // There is no more space on the current page, so we need to print on the next page
                        e.HasMorePages = true;
                        return;
                    }
                }
            }
            // Print the Amount and Reil values
            string amountText = "Amount: " + txtDollar.Text;
            string reilText = "Reil: " + txtKhmer.Text;

            float amountWidth = TextRenderer.MeasureText(amountText, contentFont).Width;
            float reilWidth = TextRenderer.MeasureText(reilText, contentFont).Width;

            float amountX = e.MarginBounds.Left;
            float reilX = e.MarginBounds.Left + e.MarginBounds.Width - reilWidth;

            currentY += lineHeight * 2; // Add some vertical spacing before printing Amount and Reil
            e.Graphics.DrawString(amountText, contentFont, contentBrush, new PointF(amountX, currentY));
            e.Graphics.DrawString(reilText, contentFont, contentBrush, new PointF(reilX, currentY));
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure you want to remove all data?",
             "Confirmation", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                using (SqlConnection connect = new SqlConnection(connectionString))
                {
                    string query = "DELETE FROM Stock";
                    SqlCommand command = new SqlCommand(query, connect);
                    connect.Open();
                    command.ExecuteNonQuery();
                    connect.Close();
                    LoadData(); // Assuming you have a method to reload the data in the DataGridView
                    ClearFiled(); // Assuming you have a method to clear the input fields
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSupplier_Click(object sender, EventArgs e)
        {
            Supplier supplier = new Supplier();
            supplier.Show();
            this.Hide();
        }

        private void StaffBtn_Click(object sender, EventArgs e)
        {
            Staff staff = new Staff();
            staff.Show();
            this.Hide();
        }

        private void btnLogOut_Click(object sender, EventArgs e)
        {
            Login log = new Login();
            log.Show();
            this.Hide();


        }
    }
}
