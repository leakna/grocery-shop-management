using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Printing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Grocery_Shop_Management_System
{
    public partial class Supplier : Form
    {
        private string connectionString = "Data Source=LAPTOP-6K25P4MH\\SQLEXPRESS;Initial Catalog=Admin;" +
                "Integrated Security=True";
        public Supplier()
        {
            InitializeComponent();
        }

        private void btnStock_Click(object sender, EventArgs e)
        {
            AdminForm admin = new AdminForm();
            admin.Show();
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
            decimal price = decimal.Parse(txtPrice.Text);
            int qty = int.Parse(txtQty.Text);
            decimal total = price * qty;
            txtTotal.Text = total.ToString();
            string contact = txtContact.Text;
            string company = cmbCompany.Text;
            string address = cmbAddress.Text;
            string date = txtDate.Text;
            using (SqlConnection connect = new SqlConnection(connectionString))
            {
                string query = "INSERT INTO Supplier(ID,PName,Price,Qty,Total,Contact,Company,Address,PDate)" +
                    "VALUES(@ID,@PName,@Price,@Qty,@Total,@Contact,@Company,@Address,@PDate)";
                SqlCommand command = new SqlCommand(query, connect);
                command.Parameters.AddWithValue("@ID", Convert.ToInt32(txtID.Text));
                command.Parameters.AddWithValue("@PName", txtName.Text);
                //command.Parameters.AddWithValue("@Category", cmbCategory.Text);
                command.Parameters.AddWithValue("@Price", Convert.ToDecimal(txtPrice.Text));
                command.Parameters.AddWithValue("@Qty", Convert.ToInt32(txtQty.Text));
                command.Parameters.AddWithValue("@Total", Convert.ToDecimal(txtTotal.Text));
                command.Parameters.AddWithValue("@Contact", txtContact.Text);
                command.Parameters.AddWithValue("@Company", cmbCompany.Text);
                command.Parameters.AddWithValue("@Address", cmbAddress.Text);
                command.Parameters.AddWithValue("@PDate", Convert.ToDateTime(txtDate.Text));
                connect.Open();
                command.ExecuteNonQuery();
                connect.Close();
                LoadData();
                ClearFiled();
            }
        }
        private void LoadData()
        {
            using (SqlConnection connect = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM Supplier";
                SqlDataAdapter adapter = new SqlDataAdapter(query, connect);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);
                dataGridView1.DataSource = dataTable;
            }
            CalculateTotalAmount();
            CalculateKhmerAmount();
        }
        private void CalculateTotalAmount()
        {
            decimal totalAmount = 0;
            foreach (DataGridViewRow row in dataGridView1.Rows)
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
        private void ClearFiled()
        {
            txtID.Text = "";
            txtName.Text = "";
           
            txtPrice.Text = "";
            txtQty.Text = "";
            txtTotal.Text = "";
            txtContact.Text = "";
            cmbAddress.Text = "";
            txtDate.Text = "";
            txtDollar.Text = "";
            txtKhmer.Text = "";
        }
        private void Calculate()
        {
            if (decimal.TryParse(txtPrice.Text, out decimal price) &&
                int.TryParse(txtQty.Text, out int quantity))
            {
                decimal total = price * quantity;
                txtTotal.Text = total.ToString("c");
            }
        }

        private void txtPrice_TextChanged(object sender, EventArgs e)
        {
            Calculate();
        }

        private void txtQty_TextChanged(object sender, EventArgs e)
        {
            Calculate();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void Supplier_Load(object sender, EventArgs e)
        {
            //load data to data gridview
            LoadData();
            //load category option to cobobox search
            LoadCategoryOption();
        }
        private void LoadCategoryOption()
        {
            using (SqlConnection connect = new SqlConnection(connectionString))
            {
                string query = "SELECT DISTINCT Company FROM Supplier";
                SqlDataAdapter adapter = new SqlDataAdapter(query, connect);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);
                //Clear existing category
                cmbSearch.Items.Clear();
                //Add category option to search
                foreach (DataRow row in dataTable.Rows)
                {
                    cmbSearch.Items.Add(row["Company"].ToString());
                }
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = dataGridView1.SelectedRows[0];
                string selectedID = selectedRow.Cells["ID"].Value.ToString();

                int id = int.Parse(txtID.Text);
                string name = txtName.Text;
                decimal price = decimal.Parse(txtPrice.Text);
                int qty = int.Parse(txtQty.Text);
                decimal total = price * qty;
                txtTotal.Text = total.ToString();
                string contact = txtContact.Text;
                string address = cmbAddress.Text;
                string date = txtDate.Text;
                //database statement
                using (SqlConnection connect = new SqlConnection(connectionString))
                {
                    string query = "UPDATE Supplier SET ID=@ID, PName=@PName," +
                        "Price=@Price,Qty=@Qty,Total=@Total,Contact=@Contact,Company=@Company,Address=@Address," +
                        "PDate=@PDate WHERE ID=@SelectedID";
                    SqlCommand command = new SqlCommand(query, connect);
                    command.Parameters.AddWithValue("@ID", Convert.ToInt32(txtID.Text));
                    command.Parameters.AddWithValue("@PName", txtName.Text);
                    //command.Parameters.AddWithValue("@Category", cmbCategory.Text);
                    command.Parameters.AddWithValue("@Price", Convert.ToDecimal(txtPrice.Text));
                    command.Parameters.AddWithValue("@Qty", Convert.ToInt32(txtQty.Text));
                    command.Parameters.AddWithValue("@Total", Convert.ToDecimal(txtTotal.Text));
                    command.Parameters.AddWithValue("@Contact", txtContact.Text);
                    command.Parameters.AddWithValue("@Company", cmbCompany.Text);
                    command.Parameters.AddWithValue("@Address", cmbAddress.Text);
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
            if (e.RowIndex >= 0 && e.RowIndex < dataGridView1.Rows.Count)
            {
                DataGridViewRow selectedRow = dataGridView1.Rows[e.RowIndex];
                txtID.Text = selectedRow.Cells["ID"].Value.ToString();
                txtName.Text = selectedRow.Cells["PName"].Value.ToString();
                //cmbCategory.Text = selectedRow.Cells["Category"].Value.ToString();
                txtPrice.Text = selectedRow.Cells["Price"].Value.ToString();
                txtQty.Text = selectedRow.Cells["Qty"].Value.ToString();
                //Calculate the total based on price and qty
                if (decimal.TryParse(txtPrice.Text, out decimal price) && int.TryParse(txtQty.Text, out int quantity))
                {
                    decimal total = price * quantity;
                    txtTotal.Text = total.ToString("c");
                }
                else
                {
                    txtTotal.Text = "";
                }
                txtContact.Text = selectedRow.Cells["Contact"].Value.ToString();
                cmbCompany.Text = selectedRow.Cells["Company"].Value.ToString();
                cmbAddress.Text = selectedRow.Cells["Address"].Value.ToString();
                txtDate.Text = selectedRow.Cells["PDate"].Value.ToString();
            }
        }

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
                    using (SqlConnection connect = new SqlConnection(connectionString))
                    {
                        string query = "DELETE FROM Suppiler WHERE ID=@SelectedID";
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

        private void cmbSearch_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedID = cmbSearch.Text;
            using (SqlConnection connect = new SqlConnection(connectionString))
             {
               string query = "SELECT * FROM Supplier WHERE Company=@Company";
               SqlDataAdapter adapter = new SqlDataAdapter(query, connect);
               adapter.SelectCommand.Parameters.AddWithValue("@Company", selectedID);
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

        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            // Set up the printing content
            // Set up the printing content
            string headerText = "Management Supplier List";

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
            string[] columnHeaders = { "ID", " Name", "Price", "Qty", "Total","Contact","Address", "Date", "Company" };
            float[] columnWidths = { 50, 80, 80, 80, 80, 80, 80,80,80 };

            Color headerBackColor = Color.LightBlue;
            Brush headerBrush = new SolidBrush(headerBackColor);

            for (int i = 0; i < columnHeaders.Length; i++)
            {
                RectangleF headerRect = new RectangleF(startX, currentY, columnWidths[i], lineHeight);
                e.Graphics.FillRectangle(headerBrush, headerRect);

                // Add bold formatting to specific column headers
                if (columnHeaders[i] == "ID" || columnHeaders[i] == " Name" || columnHeaders[i] == "Price" || columnHeaders[i] == "Qty" || columnHeaders[i] == "Total" || columnHeaders[i] == "Contact" || 
                    columnHeaders[i] == "Address" || columnHeaders[i] == " Date" || columnHeaders[i] == " Company")
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

        private void btnPrint_Click(object sender, EventArgs e)
        {
            PrintDocument printDocument = new PrintDocument();
            printDocument.PrintPage += printDocument1_PrintPage;

            PrintPreviewDialog printPreviewDialog = new PrintPreviewDialog();
            printPreviewDialog.Document = printDocument;

            printPreviewDialog.ShowDialog();
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure you want to remove all data?",
            "Confirmation", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                using (SqlConnection connect = new SqlConnection(connectionString))
                {
                    string query = "DELETE FROM Supplier";
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
