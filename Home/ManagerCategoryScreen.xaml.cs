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
        private MasterDataManager masterDataManager = MasterDataManager.getInstance();

        public ManagerCatogoryScreen()
        {
            InitializeComponent();
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
            if(((Category)cbEdit.SelectedItem) == null)
            {
                MessageBox.Show("Vui lòng chọn danh mục cần chỉnh sửa");
            }
            else
	        {
                btnEdit.Visibility = Visibility.Collapsed;

                Category category = ((Category)cbEdit.SelectedItem);
               //Chưa xử lý icon.
                fillForm.Visibility = Visibility.Visible;
                fillForm.DataContext = category;
            }
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            //Lưu lại danh mục được chỉnh sửa

            string oldIcon= ((Category)cbEdit.SelectedItem).Icon;
            var newName = txtCatogoryNameEdit.Text;

            //string categoryName = txtCatogoryName.Text;

            string iconFullName;
            string sourcePath = "";

            //Nếu xử dụng default icon
            if (Global.isUsingtheOldFile(iconCategorySelected.Source.ToString(), oldIcon))
            {
                iconFullName = oldIcon;
            }
            else
            {
                //Tạo tên mới cho icon 
                string iconExtension = Global.getExtensionOfFile(iconCategorySelected.Source.ToString());
                string iconName = Global.makeFileNameBy(newName);
                iconFullName = iconName + iconExtension;

                //Xử lí path của icon về chuẩn hàm File.Copy
                sourcePath = iconCategorySelected.Source.ToString().Remove(0, 8).Replace("/", @"\");
            }
            //Lấy thư mục chứa file icon của app
            string baseFolder = Global.getBaseFolder();
            string iconFolder = baseFolder + @"\Images\category\";

            if (string.IsNullOrEmpty(newName))
            {
                MessageBox.Show("Vui lòng nhập tên danh mục");
                return;
            }
            else {
                var oldCategoryName = ((Category)cbEdit.SelectedItem).Name;
                Category oldCategory = (Category)cbEdit.SelectedItem;
                Category newCategory = new Category()
                {
                    Name = newName,
                    Icon = iconFullName
                };
                if (masterDataManager.updateCategory(oldCategory,newCategory))
                {
                    MessageBox.Show($"Cập nhật {oldCategoryName} thành {newCategory.Name} thành công");
                    if (!Global.isUsingtheOldFile(iconCategorySelected.Source.ToString(), oldIcon))
                    {
                        Global.copyFileTo(sourcePath, iconFolder + iconFullName);
                    }
                    updateCB();
                }
                else
                {
                        MessageBox.Show($"Không thể sửa. {newCategory.Name} đã tồn tại");
                }
            }
        }

        private void btnAddCategory_Click(object sender, RoutedEventArgs e)
        {
            string categoryName = txtCatogoryName.Text;
            string iconFullName;
            string sourcePath= "";
           
            //Nếu xử dụng default icon
            if (Global.isUsingtheOldFile(reviewIcon.Source.ToString(), "default-category-icon.ico"))
            {
                iconFullName = "default-category-icon.ico";
            }
            else
            { 
                //Tạo tên mới cho icon 
                string iconExtension = Global.getExtensionOfFile(reviewIcon.Source.ToString());
                string iconName = Global.makeFileNameBy(categoryName);
                iconFullName = iconName + iconExtension;

                //Xử lí path của icon về chuẩn hàm File.Copy
                sourcePath = reviewIcon.Source.ToString().Remove(0, 8).Replace("/", @"\");
            }
            //Lấy thư mục chứa file icon của app
            string baseFolder = Global.getBaseFolder();
            string iconFolder = baseFolder + @"\Images\category\";

            Category newCategory = new Category(){
                Name = categoryName,
                Icon = iconFullName
            };

            if (string.IsNullOrEmpty(categoryName))
            {
                MessageBox.Show("Vui lòng nhập tên danh mục");
                return;
            }

            if (masterDataManager.addNewCategory(newCategory))
            {
                MessageBox.Show($"Thêm {newCategory.Name} thành công");
                if(!Global.isUsingtheOldFile(reviewIcon.Source.ToString(), "default-category-icon.ico"))
                { 
                    Global.copyFileTo(sourcePath, iconFolder + iconFullName);
                }
                updateCB();
            }
            else
            {
                MessageBox.Show($"Không thể thêm. {newCategory.Name} đã tồn tại");
            }
        }

        private void btnDelCategory_Click(object sender, RoutedEventArgs e)
        {
            if (((Category)cbDel.SelectedItem) == null)
            {
                MessageBox.Show("Vui lòng chọn danh mục cần xóa","Lỗi");
            }
            else
            {
                var categoryId = ((Category)cbDel.SelectedItem).ID;
                var categoryName = ((Category)cbDel.SelectedItem).Name;

                if (masterDataManager.deleteCategory(categoryId))
                {
                    //Chưa xử lý(xóa) icon
                    MessageBox.Show($"Xóa {categoryName} thành công");
                    updateCB();
                }
                else
                {
                    MessageBox.Show($"Xóa {categoryName} thất bại. Còn sản phẩm thuộc loại này");
                } 
            }
        }

        /// <summary>
        /// Chọn icon khi thêm category
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ChooseIcon_Click(object sender, RoutedEventArgs e)
        {
            BitmapImage icon = Global.getImage();
            if(icon != null)
            {
                reviewIcon.Source = icon;
            } 
        }


        /// <summary>
        /// Chọn icon khi sửa category
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ChangeIcon_Click(object sender, RoutedEventArgs e)
        {
            BitmapImage icon = Global.getImage();
            if (icon != null)
            {
                iconCategorySelected.Source = icon;
            }
        }
    }
}