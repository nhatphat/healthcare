using Home.models;
using Home.Utils;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Cache;
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
    /// Interaction logic for ManagerCatogoryScreen.xaml
    /// </summary>
    public partial class ManagerCatogoryScreen : UserControl
    {
        private MasterDataManager masterDataManager;

        public ManagerCatogoryScreen()
        {
            InitializeComponent();
            masterDataManager = MasterDataManager.getInstance();

        }

        private void updateCB()
        {
            var category = masterDataManager.getAllCategory();

            this.DataContext = new
            {
                category = category
            };
        }

        private void AddCatogory_Click(object sender, RoutedEventArgs e)
        {
            functionForm.Visibility = Visibility.Collapsed;
            btnBack.Visibility = Visibility.Visible;
            addForm.Visibility = Visibility.Visible;
        }

        private void EditCatogory_Click(object sender, RoutedEventArgs e)
        {
            functionForm.Visibility = Visibility.Collapsed;
            btnBack.Visibility = Visibility.Visible;
            editForm.Visibility = Visibility.Visible;

            updateCB();
        }

        private void DeleteCatogory_Click(object sender, RoutedEventArgs e)
        {
            functionForm.Visibility = Visibility.Collapsed;
            btnBack.Visibility = Visibility.Visible;
            deleteForm.Visibility = Visibility.Visible;
            updateCB();
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

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            btnEdit.Visibility = Visibility.Collapsed;
            fillForm.Visibility = Visibility.Visible;
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            //Lưu lại danh mục được chỉnh sửa
            var icon= ((Category)cbEdit.SelectedItem).Icon;
            var name = txtCatogoryNameEdit.Text;

            if (string.IsNullOrEmpty(name))
            {
                MessageBox.Show("Vui lòng nhập tên danh mục");
                return;
            }
            else {
                var oldCategoryName = ((Category)cbEdit.SelectedItem).Name;
                Category oldCategory = (Category)cbEdit.SelectedItem;
                Category newCategory = new Category()
                {
                    Name = name,
                    Icon = icon
                };
                if (MasterDataManager.getInstance().updateCategory(oldCategory,newCategory))
                {
                        MessageBox.Show($"Cập nhật {oldCategoryName} thành {newCategory.Name} thành công");
                        updateCB();
                }
                else
                {
                        MessageBox.Show($"Không thể sửa. {name} đã tồn tại");
                }
            }
        }

        private void btnAddCategory_Click(object sender, RoutedEventArgs e)
        {
            var name = txtCatogoryName.Text;
            Category newCategory = new Category(){
                Name = name,
                Icon = "default-catogory-icon.ico"
            };
            if (string.IsNullOrEmpty(name))
            {
                MessageBox.Show("Vui lòng nhập tên danh mục");
                return;
            }

            if (MasterDataManager.getInstance().addNewCategory(newCategory))
            {
                MessageBox.Show($"Thêm {newCategory.Name} thành công");
                updateCB();
            }
            else
            {
                MessageBox.Show($"Không thể thêm. {newCategory.Name} đã tồn tại");
            }
        }

        private void btnDelCategory_Click(object sender, RoutedEventArgs e)
        {
            var categoryId = ((Category)cbDel.SelectedItem).ID;
            var categoryName = ((Category)cbDel.SelectedItem).Name;

            if (MasterDataManager.getInstance().deleteCategory(categoryId))
            {
                MessageBox.Show($"Xóa {categoryName} thành công");
                updateCB();
            }
            else
            {
                MessageBox.Show($"Xóa {categoryName} thất bại. Còn sản phẩm thuộc loại này");
            }
        }

        private void ChooseIcon_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Filter = "jpg files(*.jpg)|*.jpg|PNG files(*.png)|*.png|ico files(*.ico)|*.ico";
            if(fileDialog.ShowDialog() == true)
            {
                var imagePath = fileDialog.FileName;

                BitmapImage imageBitmap = new BitmapImage();
                imageBitmap.BeginInit();
                imageBitmap.CacheOption = BitmapCacheOption.None;
                imageBitmap.UriCachePolicy = new RequestCachePolicy(RequestCacheLevel.BypassCache);
                imageBitmap.CacheOption = BitmapCacheOption.OnLoad;
                imageBitmap.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
                imageBitmap.UriSource = new Uri(imagePath, UriKind.RelativeOrAbsolute);
                imageBitmap.EndInit();             
                reviewIcon.Source = imageBitmap;
            }
            
        }
    }
}