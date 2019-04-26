using Home.models;
using Home.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Home
{
    /// <summary>
    /// Interaction logic for ManagerProductScreen.xaml
    /// </summary>
    public partial class ManagerProductScreen : UserControl
    {
        private DBManager dbManager = DBManager.getInstance();

        public ManagerProductScreen()
        {
            InitializeComponent();
            
        }

        private void AddProduct_Click(object sender, RoutedEventArgs e)
        {
            functionForm.Visibility = Visibility.Collapsed;
            btnBack.Visibility = Visibility.Visible;
            addForm.Visibility = Visibility.Visible;
            BindingList<Category> catogory = new BindingList<Category>();
            catogory = dbManager.getAllCategoryName();
            cbCatogoryFrmAdd.ItemsSource = catogory;
 
        }

        private void EditProduct_Click(object sender, RoutedEventArgs e)
        {
            functionForm.Visibility = Visibility.Collapsed;
            btnBack.Visibility = Visibility.Visible;
            deleteForm.Visibility = Visibility.Visible;
        }

        private void DeleteProduct_Click(object sender, RoutedEventArgs e)
        {
            functionForm.Visibility = Visibility.Collapsed;
            btnBack.Visibility = Visibility.Visible;
            editForm.Visibility = Visibility.Visible;
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            functionForm.Visibility = Visibility.Visible;
            btnBack.Visibility = Visibility.Collapsed;
            if (editForm.Visibility == Visibility.Visible)
            {
                editForm.Visibility = Visibility.Collapsed;
            }
            else if (deleteForm.Visibility == Visibility.Visible)
            {
                deleteForm.Visibility = Visibility.Collapsed;
            }
            else if (addForm.Visibility == Visibility.Visible)
            {
                addForm.Visibility = Visibility.Collapsed;
            }
        }

        


        private void SelectProductDelete_Click(object sender, RoutedEventArgs e)
        {

        }

        private void DeleteProductSelected_Click(object sender, RoutedEventArgs e)
        {

        }

        private void AddProductFrmAdd_Click(object sender, RoutedEventArgs e)
        {
          
            uint price;
            string catogory = ((Category)cbCatogoryFrmAdd.SelectedItem).Name;

            if (string.IsNullOrEmpty(txtProductName.Text) || string.IsNullOrEmpty(txtProductPrice.Text) || string.IsNullOrEmpty(txtProductOrgin.Text) || string.IsNullOrEmpty(txtProductDetail.Text))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin!", "Lỗi");
            }
            else if (!uint.TryParse(txtProductPrice.Text, out price))
            {
                MessageBox.Show("Giá của sản phẩm không hợp lệ!", "Lỗi");
            }
            else
            {
                Cosmetic cosmetic = new Cosmetic();
             
                cosmetic.Name = txtProductName.Text;
                cosmetic.Price = price;
                cosmetic.Origin = txtProductOrgin.Text;
                cosmetic.Detail = txtProductDetail.Text;
                cosmetic.Image_url = "default.jpg";
                int result = dbManager.addNewCosmeticOfSheet(cosmetic, catogory);

                if (result != -1)
                {
                    cosmetic.row_in_db = result;
                    MessageBox.Show("Thêm thành công", "Lỗi");
                }
                else
                {
                    MessageBox.Show("Thêm không thành công", "Lỗi!");
                }
            }

        }
    }
}
