
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Gaudioso_Work04
{
    public partial class Form1 : Form
    {
        // Store registered products
        private List<Product> productList = new List<Product>();

        // Product class
        private class Product
        {
            public string Barcode { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public string Category { get; set; }
            public string Origin { get; set; }
            public string Status { get; set; }
            public decimal Cost { get; set; }
            public decimal Price { get; set; }
            public decimal Markup { get; set; }
        }

        // FORM CONSTRUCTOR
        public Form1()
        {
            InitializeComponent();

            // Initialize categories
            LoadCategories();

            // Connect button events
            btnSave.Click += btnSave_Click;
            btnClear.Click += btnClear_Click;
            btnCancel.Click += btnCancel_Click;

            // Connect calculation events
            txtCost.TextChanged += CalculateMarkup;
            txtPrice.TextChanged += CalculateMarkup;

            // TextBox settings
            txtDescription.Multiline = true;
            txtDescription.ScrollBars = ScrollBars.Vertical;

            txtMarkup.ReadOnly = true;

            // ComboBox settings
            cmbCategory.DropDownStyle =
                ComboBoxStyle.DropDownList;
        }

        // LOAD CATEGORY OPTIONS
        private void LoadCategories()
        {
            cmbCategory.Items.Clear();

            cmbCategory.Items.Add("Electronics");
            cmbCategory.Items.Add("Food & Beverages");
            cmbCategory.Items.Add("Clothing");
            cmbCategory.Items.Add("School Supplies");
            cmbCategory.Items.Add("Household Items");
            cmbCategory.Items.Add("Personal Care");
            cmbCategory.Items.Add("Other");
        }

        // AUTOMATIC MARKUP CALCULATION
        private void CalculateMarkup(object sender, EventArgs e)
        {
            decimal cost;
            decimal price;

            if (decimal.TryParse(txtCost.Text, out cost) &&
                decimal.TryParse(txtPrice.Text, out price) &&
                cost > 0)
            {
                decimal markup =
                    ((price - cost) / cost) * 100;

                txtMarkup.Text = markup.ToString("0.00");
            }
            else
            {
                txtMarkup.Clear();
            }
        }

        // SAVE BUTTON
        private void btnSave_Click(object sender, EventArgs e)
        {
            // Validate Barcode
            if (string.IsNullOrWhiteSpace(txtBarcode.Text))
            {
                MessageBox.Show(
                    "Please enter the barcode.",
                    "Validation Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                txtBarcode.Focus();
                return;
            }

            // Validate Name
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show(
                    "Please enter the product name.",
                    "Validation Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                txtName.Focus();
                return;
            }

            // Validate Description
            if (string.IsNullOrWhiteSpace(txtDescription.Text))
            {
                MessageBox.Show(
                    "Please enter the product description.",
                    "Validation Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                txtDescription.Focus();
                return;
            }

            // Validate Category
            if (cmbCategory.SelectedIndex == -1)
            {
                MessageBox.Show(
                    "Please select a category.",
                    "Validation Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                cmbCategory.Focus();
                return;
            }

            // Validate Origin
            if (!rdoLocal.Checked && !rdoAbroad.Checked)
            {
                MessageBox.Show(
                    "Please select the product origin.",
                    "Validation Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            // Check Duplicate Barcode
            foreach (Product p in productList)
            {
                if (p.Barcode.Equals(
                    txtBarcode.Text.Trim(),
                    StringComparison.OrdinalIgnoreCase))
                {
                    MessageBox.Show(
                        "Barcode already exists!",
                        "Duplicate Barcode",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    txtBarcode.Focus();
                    return;
                }
            }

            // Validate Cost
            decimal cost;

            if (!decimal.TryParse(txtCost.Text, out cost) ||
                cost < 0)
            {
                MessageBox.Show(
                    "Please enter a valid cost.",
                    "Invalid Cost",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                txtCost.Focus();
                return;
            }

            // Validate Price
            decimal price;

            if (!decimal.TryParse(txtPrice.Text, out price) ||
                price < 0)
            {
                MessageBox.Show(
                    "Please enter a valid selling price.",
                    "Invalid Price",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                txtPrice.Focus();
                return;
            }

            // Calculate Markup
            decimal markup = 0;

            if (cost > 0)
            {
                markup = ((price - cost) / cost) * 100;
            }

            // Get Origin
            string origin;

            if (rdoLocal.Checked)
            {
                origin = "Local";
            }
            else
            {
                origin = "Abroad";
            }

            // Get Status
            string status = chkActive.Checked
                ? "Active"
                : "Inactive";

            // Create New Product
            Product newProduct = new Product
            {
                Barcode = txtBarcode.Text.Trim(),
                Name = txtName.Text.Trim(),
                Description = txtDescription.Text.Trim(),
                Category = cmbCategory.Text,
                Origin = origin,
                Status = status,
                Cost = cost,
                Price = price,
                Markup = markup
            };

            // Add Product to List
            productList.Add(newProduct);

            // Display Saved Information
            MessageBox.Show(
                "PRODUCT SUCCESSFULLY SAVED!\n\n" +
                "Barcode: " + newProduct.Barcode + "\n" +
                "Name: " + newProduct.Name + "\n" +
                "Description: " + newProduct.Description + "\n" +
                "Category: " + newProduct.Category + "\n" +
                "Origin: " + newProduct.Origin + "\n" +
                "Status: " + newProduct.Status + "\n" +
                "Cost: " + newProduct.Cost.ToString("N2") + "\n" +
                "Price: " + newProduct.Price.ToString("N2") + "\n" +
                "Markup: " + newProduct.Markup.ToString("0.00") + "%",
                "Save Successful",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);

            ClearFields();
        }

        // CLEAR BUTTON
        private void btnClear_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(
                "Are you sure you want to clear all fields?",
                "Confirm Clear",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                ClearFields();
            }
        }

        // CANCEL BUTTON
        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(
                "Are you sure you want to close this form?",
                "Confirm Exit",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                this.Close();
            }
        }

        // CLEAR ALL INPUT FIELDS
        private void ClearFields()
        {
            txtBarcode.Clear();
            txtName.Clear();
            txtDescription.Clear();

            cmbCategory.SelectedIndex = -1;

            rdoLocal.Checked = false;
            rdoAbroad.Checked = false;

            chkActive.Checked = false;

            txtCost.Clear();
            txtPrice.Clear();
            txtMarkup.Clear();

            txtBarcode.Focus();
        }
    }
}
