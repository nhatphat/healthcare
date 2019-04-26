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

        private BindingList<Category> category;
        private BindingList<Cosmetic> cosmetic;

        public ManagerProductScreen()
        {
            InitializeComponent();


        }

        private void updateDataContext(string categoryName)
        {
            if (categoryName == null)
            {
                category = dbManager.getAllCategoryName();
            }
            else
            {
                cosmetic = dbManager.getAllCosmeticBySheetName(categoryName);
            }
            this.DataContext = new
            {
                category = category,
                cosmetic = cosmetic
            };
        }

        private void AddProduct_Click(object sender, RoutedEventArgs e)
        {
            functionForm.Visibility = Visibility.Collapsed;
            btnBack.Visibility = Visibility.Visible;
            addForm.Visibility = Visibility.Visible;
            BindingList<Category> catogorys = new BindingList<Category>();
            catogorys = dbManager.getAllCategoryName();
            cbCatogoryFrmAdd.ItemsSource = catogorys;

        }

        private void EditProduct_Click(object sender, RoutedEventArgs e)
        {
            functionForm.Visibility = Visibility.Collapsed;
            btnBack.Visibility = Visibility.Visible;
            editForm.Visibility = Visibility.Visible;

            updateDataContext(null);
        }

        private void DeleteProduct_Click(object sender, RoutedEventArgs e)
        {
            functionForm.Visibility = Visibility.Collapsed;
            btnBack.Visibility = Visibility.Visible;
            deleteForm.Visibility = Visibility.Visible;

            updateDataContext(null);
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
            productFullDetail.Visibility = Visibility.Visible;
        }

        private void DeleteProductSelected_Click(object sender, RoutedEventArgs e)
        {

        }

        private void AddProductFrmAdd_Click(object sender, RoutedEventArgs e)
        {

            int price;
            string catogory = ((Category)cbCatogoryFrmAdd.SelectedItem).Name;

            if (string.IsNullOrEmpty(txtProductName.Text) || string.IsNullOrEmpty(txtProductPrice.Text) || string.IsNullOrEmpty(txtProductOrgin.Text) || string.IsNullOrEmpty(txtProductDetail.Text))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin!", "Lỗi");
            }
            else if (!int.TryParse(txtProductPrice.Text, out price))
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
                cosmetic.Image = "default.jpg";
                int result = dbManager.addNewCosmeticOfSheet(cosmetic, catogory);

                if (result != -1)
                {
                    //cosmetic.row_in_db = result;
                    MessageBox.Show("Thêm thành công", "Lỗi");
                }
                else
                {
                    MessageBox.Show("Thêm không thành công", "Lỗi!");
                }
            }

        }

        private void item_edit_changed(object sender, SelectionChangedEventArgs e)
        {
            if (cbEdit.SelectedItem != null)
            {
                var category = ((Category)cbEdit.SelectedItem).Name;

                updateDataContext(category);
            }
        }

        private void item_del_changed(object sender, SelectionChangedEventArgs e)
        {
            if (cbDel.SelectedItem != null)
            {
                var category = ((Category)cbDel.SelectedItem).Name;

                updateDataContext(category);
            }
        }
    }
}
