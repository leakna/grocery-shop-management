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
using System.Drawing.Printing;

namespace Grocery_Shop_Management_System
{
    public partial class CasheirForm : Form
    {
        private PrintDocument print;
        private DataTable dataTable;
        //private int rowIndex = 0;
        //private DataTable dataTable;
        private SqlConnection connection;
        public CasheirForm()
        {
            InitializeComponent();
            dataTable = new DataTable();
            dataTable.Columns.Add("Name", typeof(string));
            dataTable.Columns.Add("Price", typeof(decimal));
            dataTable.Columns.Add("Quantity", typeof(int));
            dataTable.Columns.Add("Total", typeof(decimal));
            txtAmount.TextChanged += txtPaid_TextChanged;
            txtDiscount.TextChanged += txtPaid_TextChanged;
            txtPaid.TextChanged += txtPaid_TextChanged;
            printDocument1.PrintPage += printDocument1_PrintPage;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                string name = txtName.Text;
                decimal price = decimal.Parse(txtPrice.Text);
                int qty = int.Parse(txtQty.Text);
                decimal total = price * qty;
                txtTotal.Text = total.ToString();
                dataTable.Rows.Add(name, price, qty, total);
                dataGridView1.DataSource = dataTable;
                UpdateTotalAmount();

                Clear();

                // Insert data into the Billing table

            }
            catch (FormatException)
            {
                MessageBox.Show("Invalid input. Please enter a valid decimal value for price and a valid integer value for quantity.");
            }

        }
        private void Clear()
        {
            txtName.Text = "";
            txtPrice.Text = "";
            txtQty.Text = "";
            txtTotal.Text = "";
        }
        private void UpdateTotalAmount()
        {
            decimal sum = 0;

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.Cells["Total"].Value != null)
                {
                    sum += Convert.ToDecimal(row.Cells["Total"].Value);
                }
            }

            txtAmount.Text = sum.ToString("c");
            Pay();
        }
        private void Calculate()
        {

            if (decimal.TryParse(txtPrice.Text, out decimal price) && int.TryParse(txtQty.Text, out int quantity))
            {
                decimal total = price * quantity;
                txtTotal.Text = total.ToString("c");

            }
        }


        private void CasheirForm_Load(object sender, EventArgs e)
        {
            LoadBillingData();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            PrintDocument printDocument = new PrintDocument();
            printDocument.PrintPage += printDocument1_PrintPage; // Assign the event handler

            PrintPreviewDialog printPreviewDialog = new PrintPreviewDialog();
            printPreviewDialog.Document = printDocument;

            printPreviewDialog.ShowDialog();
        }

        private void txtPrice_TextChanged(object sender, EventArgs e)
        {
            Calculate();
        }

        private void txtQty_TextChanged(object sender, EventArgs e)
        {
            Calculate();
        }

        private void txtDiscount_TextChanged(object sender, EventArgs e)
        {
            Pay();
        }
        private void Pay()
        {
            if (decimal.TryParse(txtAmount1.Text, out decimal amount) &&
        decimal.TryParse(txtDiscount.Text, out decimal discount))
            {
                decimal paid = amount - (amount * (discount / 100));
                txtPaid.Text = paid.ToString("c");

                // Calculate the value for txtRiel
                decimal riel = paid * 4000;
                txtReil.Text = riel.ToString() + "R";
            }
            else
            {
                txtPaid.Text = ""; // Clear the TextBox if the input is invalid
                txtReil.Text = ""; // Clear the TextBox for txtRiel
            }

        }

        private void txtAmount_TextChanged(object sender, EventArgs e)
        {
            Pay();
        }

        private void txtPaid_TextChanged(object sender, EventArgs e)
        {
            Pay();
        }

        private void printDocument1_PrintPage(object sender, PrintPageEventArgs e)
        {
            Graphics graphics = e.Graphics;
            Font font = new Font("Arial", 18, FontStyle.Bold); // Set the font style to bold

            float lineHeight = font.GetHeight();

            float x = 10;
            float y = 10;
            string missYouText = "Miss You ម៉ាត";
            float missYouTextX = x + (graphics.VisibleClipBounds.Width - graphics.MeasureString(missYouText, font).Width) / 2;
            graphics.DrawString(missYouText, font, Brushes.Black, missYouTextX, y);
            y += lineHeight * 2;

            // Print the "#220 Chhroy Chongvar Phnom Penh" line
            string addressText = "#220 Chhroy Chongvar Phnom Penh";
            float addressTextX = x;
            float addressTextY = y;
            graphics.DrawString(addressText, font, Brushes.Black, addressTextX, addressTextY);

            y += lineHeight * 2;
            //Print Cashier
            string cashierText = "Cashier: " + txtCashier.Text;
            float cashierTextX = x;
            float cashierTextY = y;
            graphics.DrawString(cashierText, font, Brushes.Black, cashierTextX, cashierTextY);
            y += lineHeight * 2;
            //Print Date
            string dateText = "Date: " + txtDate.Text;
            float dateTextX = x;
            float dateTextY = y;
            graphics.DrawString(dateText, font, Brushes.Black, dateTextX, dateTextY);
            y += lineHeight * 2;

            // Print the header
            StringFormat stringFormat = new StringFormat();
            stringFormat.Alignment = StringAlignment.Center; // Set the alignment to center
            graphics.DrawString("វិក្កយបត្រ/Receipt", font, Brushes.Black, new RectangleF(x, y, graphics.VisibleClipBounds.Width, lineHeight), stringFormat);
            y += lineHeight * 2;

            string line = "=================================================";
            float lineX = x + (graphics.VisibleClipBounds.Width - graphics.MeasureString(line, font).Width) / 2;
            graphics.DrawString(line, font, Brushes.Black, lineX, y);
            y += lineHeight * 2;

            // Add table headers
            string[] headers = { "Name", "Price", "Quantity", "   Total   " }; // Add spaces to "Total" header
            float tableCenterX = 200; // X position of the table headers
            float headerY = y;
            graphics.DrawString(headers[0], font, Brushes.Black, tableCenterX, headerY);
            graphics.DrawString(headers[1], font, Brushes.Black, tableCenterX + 100, headerY);
            graphics.DrawString(headers[2], font, Brushes.Black, tableCenterX + 220, headerY); // Adjusted X position for "Quantity"
            graphics.DrawString(headers[3], font, Brushes.Black, tableCenterX + 320, headerY); // Adjusted X position for "Total"

            y += lineHeight;

            foreach (DataRow row in dataTable.Rows)
            {
                string name = row["Name"].ToString();
                decimal price = (decimal)row["Price"];
                int quantity = (int)row["Quantity"];
                decimal total = (decimal)row["Total"];

                float nameX = tableCenterX;
                float priceX = tableCenterX + 100;
                float quantityX = tableCenterX + 220; // Adjusted X position for "Quantity"
                float totalX = tableCenterX + 320; // Adjusted X position for "Total"

                y += lineHeight;

                graphics.DrawString(name, font, Brushes.Black, nameX, y);
                graphics.DrawString(price.ToString(), font, Brushes.Black, priceX, y);
                graphics.DrawString(quantity.ToString(), font, Brushes.Black, quantityX, y);
                graphics.DrawString(total.ToString(), font, Brushes.Black, totalX, y);
                y += lineHeight;
            }
            string line1 = "=================================================";
            float lineX1 = x + (graphics.VisibleClipBounds.Width - graphics.MeasureString(line1, font).Width) / 2;
            graphics.DrawString(line1, font, Brushes.Black, lineX1, y);
            y += lineHeight * 2;

            // Print the total amount label and value
            string totalAmountLabel = "Total Amount: " + txtAmount.Text;
            float totalAmountLabelX = x + (graphics.VisibleClipBounds.Width - graphics.MeasureString(totalAmountLabel, font).Width) / 2;
            graphics.DrawString(totalAmountLabel, font, Brushes.Black, totalAmountLabelX, y);
            // Print the discount label and value
            string discountLabel = "Discount: " + txtDiscount.Text + "%";
            float discountLabelX = x + (graphics.VisibleClipBounds.Width - graphics.MeasureString(discountLabel, font).Width) / 2;
            graphics.DrawString(discountLabel, font, Brushes.Black, discountLabelX, y + lineHeight * 2);


            // Print the paid amount label and value
            string paidAmountLabel = "Paid Amount: " + txtPaid.Text;
            float paidAmountLabelX = x + (graphics.VisibleClipBounds.Width - graphics.MeasureString(paidAmountLabel, font).Width) / 2;
            graphics.DrawString(paidAmountLabel, font, Brushes.Black, paidAmountLabelX, y + lineHeight * 4);
            // Print the Reil label and value
            string reilLabel = "Riel: " + txtReil.Text;
            float reilLabelX = x + (graphics.VisibleClipBounds.Width - graphics.MeasureString(reilLabel, font).Width) / 2;
            graphics.DrawString(reilLabel, font, Brushes.Black, reilLabelX, y + lineHeight * 6);
            //Print Thank you
            // Print Thank you
            StringFormat stringFormat1 = new StringFormat();
            stringFormat1.Alignment = StringAlignment.Center; // Set the alignment to center
            graphics.DrawString("Thank you so much!!", font, Brushes.Black, new RectangleF(x, y + lineHeight * 8, graphics.VisibleClipBounds.Width,
                lineHeight), stringFormat1);
            // Print the line of dashes
            string dashLine = new string('-', (int)graphics.VisibleClipBounds.Width);
            graphics.DrawString(dashLine, font, Brushes.Black, new RectangleF(x, y + lineHeight * 10,
                graphics.VisibleClipBounds.Width, lineHeight), stringFormat1);
        }

        private void btnAddCart_Click(object sender, EventArgs e)
        {
            try
            {
                if (decimal.TryParse(txtPaid1.Text, out decimal paidAmount))
                {
                    using (SqlConnection connection = new SqlConnection("Data Source=LAPTOP-6K25P4MH\\SQLEXPRESS;Initial Catalog=Admin;" +
                        "Integrated Security=True"))
                    {
                        using (SqlCommand command = new SqlCommand("INSERT INTO Billing (Cash, Paid, PDate) VALUES (@Cash, @Paid, @PDate)", connection))
                        {
                            command.Parameters.AddWithValue("@Cash", int.Parse(txtCashier.Text));
                            command.Parameters.AddWithValue("@Paid", paidAmount);
                            command.Parameters.AddWithValue("@PDate", DateTime.Parse(txtDate.Text));

                            connection.Open();
                            command.ExecuteNonQuery();
                            connection.Close();
                        }
                    }

                    dataGridView2.Rows.Clear(); // Clear existing rows
                    LoadBillingData(); // Load the updated data
                    CalculateTotalPaid(); // Calculate and display the total paid amount
                }
                else
                {
                    MessageBox.Show("Invalid input. Please enter a valid decimal value for the paid amount.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error occurred: " + ex.Message);
            }
        }
        private void CalculateTotalPaid()
        {
            decimal totalPaid = 0;

            // Calculate the total paid amount from the dataGridView2 control
            foreach (DataGridViewRow row in dataGridView2.Rows)
            {
                if (decimal.TryParse(row.Cells["Paid"].Value?.ToString(), out decimal paidAmount))
                {
                    totalPaid += paidAmount;
                }
            }

            // Display the total paid amount in the txtRs TextBox
            txtRs.Text = totalPaid.ToString("c");
        }
        private void LoadBillingData()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection("Data Source=LAPTOP-6K25P4MH\\SQLEXPRESS;Initial Catalog=Admin;" +
                    "Integrated Security=True"))
                {
                    using (SqlCommand command = new SqlCommand("SELECT Cash, Paid, PDate FROM Billing", connection))
                    {
                        connection.Open();
                        SqlDataReader reader = command.ExecuteReader();

                        if (dataGridView2.Columns.Count == 0)
                        {
                            dataGridView2.Columns.Add("Cashier", "Cashier");
                            dataGridView2.Columns.Add("Paid", "Paid");
                            dataGridView2.Columns.Add("Date", "Date");
                        }

                        while (reader.Read())
                        {
                            dataGridView2.Rows.Add(reader["Cash"].ToString(), reader["Paid"].ToString(), reader["PDate"].ToString());
                        }

                        reader.Close();
                        connection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error occurred: " + ex.Message);
            }
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult result = MessageBox.Show("Are you sure you want to delete all data?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    using (SqlConnection connection = new SqlConnection("Data Source=LAPTOP-6K25P4MH\\SQLEXPRESS;Initial Catalog=Admin;" +
                        "Integrated Security=True"))
                    {
                        using (SqlCommand command = new SqlCommand("DELETE FROM Billing", connection))
                        {
                            connection.Open();
                            command.ExecuteNonQuery();
                            connection.Close();
                        }
                    }

                    dataGridView2.Rows.Clear(); // Clear existing rows
                    CalculateTotalPaid(); // Recalculate and display the total paid amount
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error occurred: " + ex.Message);
            }
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            try
            {
                int searchCash = int.Parse(txtSearch.Text);

                using (SqlConnection connection = new SqlConnection("Data Source=LAPTOP-6K25P4MH\\SQLEXPRESS;Initial Catalog=Admin;" +
                    "Integrated Security=True"))
                {
                    using (SqlCommand command = new SqlCommand("SELECT Cash, Paid, PDate FROM Billing WHERE Cash = @Cash", connection))
                    {
                        command.Parameters.AddWithValue("@Cash", searchCash);

                        connection.Open();
                        SqlDataReader reader = command.ExecuteReader();

                        dataGridView2.Rows.Clear(); // Clear existing rows

                        while (reader.Read())
                        {
                            dataGridView2.Rows.Add(reader["Cash"].ToString(), reader["Paid"].ToString(), reader["PDate"].ToString());
                        }

                        reader.Close();
                        connection.Close();
                    }
                }

                CalculateTotalPaid(); // Recalculate and display the total paid amount
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error occurred: " + ex.Message);
            }
        }
    }
}
