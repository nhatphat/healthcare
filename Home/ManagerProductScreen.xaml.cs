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
            catogory = dbManager.loadAllCategoryOfCosmetic();
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
        
            if (!uint.TryParse(txtProductPrice.Text, out price))
            {
                MessageBox.Show("Lỗi", "Giá của sản phẩm không hợp lệ!");
            }
            else
            {
                Cosmetic cosmetic = new Cosmetic();
                cosmetic.Name = txtProductName.Text;
                cosmetic.Price = price;
                cosmetic.Origin = txtProductOrgin.Text;
                cosmetic.Image_url = "default.jpg";
                int result = dbManager.addNewCosmeticOfSheet(cosmetic, catogory);

                if (result != -1)
                {
                    cosmetic.row_in_db = result;
                    MessageBox.Show("Lỗi", "Thêm thành công!");
                }
                else
                {
                    MessageBox.Show("Lỗi", "Thêm không thành công!");
                }
            }

        }
    }
}
