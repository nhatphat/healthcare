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

        MasterDataManager masterDataManager = MasterDataManager.getInstance();

        private BindingList<Category> category;
        private BindingList<Cosmetic> cosmetic;

        public ManagerProductScreen()
        {
            InitializeComponent();


        }

        private void updateDataContext(int categoryId)
        {
            if (categoryId == -1)
            {
                category  = new BindingList<Category>(masterDataManager.getAllCategory());
            }
            else
            {
                cosmetic = new BindingList<Cosmetic>(masterDataManager.getAllCosmeticOfCategory(categoryId));
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

            BindingList<Category> catogorys = new BindingList<Category>(masterDataManager.getAllCategory());
            cbCatogoryFrmAdd.ItemsSource = catogorys;

        }

        private void EditProduct_Click(object sender, RoutedEventArgs e)
        {
            functionForm.Visibility = Visibility.Collapsed;
            btnBack.Visibility = Visibility.Visible;
            editForm.Visibility = Visibility.Visible;

            updateDataContext(-1);
        }

        private void DeleteProduct_Click(object sender, RoutedEventArgs e)
        {
            functionForm.Visibility = Visibility.Collapsed;
            btnBack.Visibility = Visibility.Visible;
            deleteForm.Visibility = Visibility.Visible;

            updateDataContext(-1);
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            functionForm.Visibility = Visibility.Visible;
            btnBack.Visibility = Visibility.Collapsed;
            if (editForm.Visibility == Visibility.Visible)
            {
                editForm.Visibility = Visibility.Collapsed;
            }
            if (deleteForm.Visibility == Visibility.Visible)
            {
                deleteForm.Visibility = Visibility.Collapsed;
            }
            if (addForm.Visibility == Visibility.Visible)
            {
                addForm.Visibility = Visibility.Collapsed;
            }
            if(productFullDetail.Visibility == Visibility.Visible)
            {
                productFullDetail.Visibility = Visibility.Collapsed;
            }
        }




        private void SelectProductDelete_Click(object sender, RoutedEventArgs e)
        {
            productFullDetail.Visibility = Visibility.Visible;
            deleteForm.Visibility = Visibility.Collapsed;
            Cosmetic cosmetic = (Cosmetic)cbCosmeticDel.SelectedItem;
            productFullDetail.DataContext = cosmetic;
        }

        private void DeleteProductSelected_Click(object sender, RoutedEventArgs e)
        {
            
        
            Cosmetic cosmetic = productFullDetail.DataContext as Cosmetic;
            bool result = MasterDataManager.getInstance().deleteCosmetic(cosmetic.ID);
            if(result)
            {
                MessageBox.Show($"Đã xóa {cosmetic.Name}", "Thành công");
            }
            else
            {
                MessageBox.Show($"Xóa {cosmetic.Name} không thành công","Lỗi");
            }
            
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
                int categoryId = ((Category)cbCatogoryFrmAdd.SelectedItem).ID;
                Cosmetic cosmetic = new Cosmetic();
                cosmetic.Category = categoryId;
                cosmetic.Name = txtProductName.Text;
                cosmetic.Price = price;
                cosmetic.Origin = txtProductOrgin.Text;
                cosmetic.Detail = txtProductDetail.Text;
                cosmetic.Image = "default.jpg";
                bool result = masterDataManager.addNewCosmetic(cosmetic);

                if (result)
                {
                    //cosmetic.row_in_db = result;
                    MessageBox.Show("Thêm thành công", "Thành công");
                }
                else
                {
                    MessageBox.Show("Thêm không thành công", "Lỗi");
                }
            }

        }

        private void item_edit_changed(object sender, SelectionChangedEventArgs e)
        {
            if (cbEdit.SelectedItem != null)
            {
                var categoryId = ((Category)cbEdit.SelectedItem).ID;

                updateDataContext(categoryId);
            }
        }

        private void item_del_changed(object sender, SelectionChangedEventArgs e)
        {
            if (cbDel.SelectedItem != null)
            {
                var categoryId = ((Category)cbDel.SelectedItem).ID;

                updateDataContext(categoryId);
            }
        }

        private void BtnEditProductSelected_Click(object sender, RoutedEventArgs e)
        {
            editForm.Visibility = Visibility.Collapsed;
            //EditProductSelectedForm.Visibility = Visibility.Visible;
        }
    }
}
