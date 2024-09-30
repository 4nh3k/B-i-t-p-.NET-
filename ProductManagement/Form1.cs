using ProductManagement.Models;
using ProductManagement.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace ProductManagement
{
    public partial class frmProdManagement : Form
    {
        private readonly ProductService _productService;
        private ProductDTO _selectedProduct;

        public frmProdManagement()
        {
            InitializeComponent();
            _productService = new ProductService();
            _selectedProduct = new ProductDTO();
        }

        private void btn_AddProd_Click(object sender, EventArgs e)
        {
            ClearFormFields();
        }

        private void btn_SaveProd_Click(object sender, EventArgs e)
        {
            if (!IsProductFormValid())
            {
                MessageBox.Show("Có trường thông tin còn trống, vui lòng nhập đủ");
                return;
            }

            PopulateProductFromForm();

            _productService.SaveProduct(_selectedProduct);
            MessageBox.Show("Lưu thông tin SP thành công");
            ReloadProductData();
        }

        private void btn_DeleteProd_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(_selectedProduct.ProdID))
            {
                MessageBox.Show("Trường mã SP chưa có, không thể xóa");
                return;
            }

            if (MessageBox.Show("Có muốn xóa SP này không ?", "Xác nhận", MessageBoxButtons.YesNo) == DialogResult.No)
                return;

            _productService.DeleteProduct(p => p.ProdID == _selectedProduct.ProdID);
            ReloadProductData();
            ClearFormFields();
            MessageBox.Show("Xóa SP thành công!");
        }

        private void btn_MaxPriceProd_Click(object sender, EventArgs e)
        {
            var mostExpensiveProduct = _productService.GetMaxPriceProduct();
            DisplayProducts(new List<ProductDTO> { mostExpensiveProduct });
        }

        private void btn_JPProd_Click(object sender, EventArgs e)
        {
            var jpProducts = _productService.GetAllProducts(p => p.Origin == "Nhật Bản");
            DisplayProducts(jpProducts);
        }

        private void btn_ExpProd_Click(object sender, EventArgs e)
        {
            var expiredProducts = _productService.GetAllProducts(p => p.ExpireDate <= DateTime.Now);
            DisplayProducts(expiredProducts);
        }

        private void btn_RangePriceProd_Click(object sender, EventArgs e)
        {
            if (!IsPriceRangeValid())
            {
                MessageBox.Show("Khoảng giá thiếu giá trị đầu hoặc cuối chưa nhập");
                return;
            }

            decimal startPrice = decimal.Parse(txt_StartPrice.Text);
            decimal endPrice = decimal.Parse(txt_EndPrice.Text);
            var productsInRange = _productService.GetAllProducts(p => p.Price >= startPrice && p.Price <= endPrice);
            DisplayProducts(productsInRange);
        }

        private void btn_DeleteOrg_Click(object sender, EventArgs e)
        {
            string originToDelete = txt_DeleteOrg.Text;
            if (string.IsNullOrEmpty(originToDelete))
            {
                MessageBox.Show("Chưa nhập thông tin nguồn gốc xóa");
                return;
            }

            _productService.DeleteProduct(p => p.Origin == originToDelete);
            ReloadProductData();
        }

        private void btnCheckExp_Click(object sender, EventArgs e)
        {
            bool hasExpiredProducts = _productService.GetAllProducts(p => p.ExpireDate <= DateTime.Now).Any();
            string message = hasExpiredProducts ? "Có sản phẩm hết hạn" : "Không có sản phẩm hết hạn";
            MessageBox.Show(message);
        }

        private void btn_DeleteAllProd_Click(object sender, EventArgs e)
        {
            _productService.DeleteAllProducts();
            MessageBox.Show("Đã xóa hết sản phẩm");
            ReloadProductData();
        }

        private void btn_DeleteAllExp_Click(object sender, EventArgs e)
        {
            _productService.DeleteProduct(p => p.ExpireDate <= DateTime.Now);
            MessageBox.Show("Đã xóa hết sản phẩm hết hạn");
            ReloadProductData();
        }

        private void dt_GridProd_SelectionChanged(object sender, EventArgs e)
        {
            if (dt_GridProd.SelectedRows.Count == 0) return;

            var selectedRow = dt_GridProd.SelectedRows[0];
            string selectedProdID = (string)selectedRow.Cells[0].Value;

            _selectedProduct = _productService.GetAllProducts().FirstOrDefault(p => p.ProdID == selectedProdID);
            if (_selectedProduct == null) return;

            FillFormWithProduct(_selectedProduct);
            btn_DeleteProd.Enabled = true;
        }

        private void ClearFormFields()
        {
            txt_ProdID.Clear();
            txt_ProdName.Clear();
            txt_Quantity.Clear();
            txt_Price.Clear();
            txt_Origin.Clear();
            dtp_ExpireDate.Value = DateTime.Now;
            dt_GridProd.ClearSelection();
            btn_DeleteProd.Enabled = false;
        }

        private void ReloadProductData()
        {
            var allProducts = _productService.GetAllProducts();
            DisplayProducts(allProducts);
        }

        private void DisplayProducts(List<ProductDTO> products)
        {
            dt_GridProd.DataSource = null;
            dt_GridProd.DataSource = products;
            dt_GridProd.ClearSelection();
        }

        private void FillFormWithProduct(ProductDTO product)
        {
            txt_ProdID.Text = product.ProdID;
            txt_ProdName.Text = product.ProdName;
            txt_Price.Text = product.Price.ToString();
            txt_Quantity.Text = product.Quantity.ToString();
            txt_Origin.Text = product.Origin;
            dtp_ExpireDate.Value = product.ExpireDate;
        }

        private void PopulateProductFromForm()
        {
            _selectedProduct.ProdID = txt_ProdID.Text.Trim();
            _selectedProduct.ProdName = txt_ProdName.Text.Trim();
            _selectedProduct.Quantity = int.Parse(txt_Quantity.Text.Trim());
            _selectedProduct.Price = decimal.Parse(txt_Price.Text.Trim());
            _selectedProduct.Origin = txt_Origin.Text.Trim();
            _selectedProduct.ExpireDate = dtp_ExpireDate.Value;
        }

        private bool IsProductFormValid()
        {
            return !string.IsNullOrEmpty(txt_ProdID.Text) &&
                   !string.IsNullOrEmpty(txt_ProdName.Text) &&
                   !string.IsNullOrEmpty(txt_Quantity.Text) &&
                   !string.IsNullOrEmpty(txt_Price.Text) &&
                   !string.IsNullOrEmpty(txt_Origin.Text);
        }

        private bool IsPriceRangeValid()
        {
            return !string.IsNullOrEmpty(txt_StartPrice.Text) &&
                   !string.IsNullOrEmpty(txt_EndPrice.Text);
        }

        private void txt_Quantity_KeyPress(object sender, KeyPressEventArgs e)
        {
            HandleKeyPressForNumericInput(e, (TextBox)sender, true);
        }

        private void txt_Price_KeyPress(object sender, KeyPressEventArgs e)
        {
            HandleKeyPressForNumericInput(e, (TextBox)sender, true);
        }

        private void txt_StartPrice_KeyPress(object sender, KeyPressEventArgs e)
        {
            HandleKeyPressForNumericInput(e, (TextBox)sender, true);
        }

        private void txt_EndPrice_KeyPress(object sender, KeyPressEventArgs e)
        {
            HandleKeyPressForNumericInput(e, (TextBox)sender, true);
        }

        private void HandleKeyPressForNumericInput(KeyPressEventArgs e, TextBox textBox, bool allowDecimal)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.' || !allowDecimal))
            {
                e.Handled = true;
            }

            // Allow only one decimal point
            if (e.KeyChar == '.' && textBox.Text.Contains('.'))
            {
                e.Handled = true;
            }
        }

        private void frmProdManagement_Load(object sender, EventArgs e)
        {
            ReloadProductData();
        }
    }
}
