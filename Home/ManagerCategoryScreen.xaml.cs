using Home.models;
using Home.Utils;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
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
    public partial class ManagerCatogoryScreen : UserControl, MainWindow.ICategoryChangeListenter
    {
        private MasterDataManager masterDataManager = MasterDataManager.getInstance();

        //using update main screen when modify category
        public event MainWindow.categoryChanged OnCategoryChangeListener;

        private void categoryChanged()
        {
            OnCategoryChangeListener?.Invoke();
        }

        public ManagerCatogoryScreen()
        {
            InitializeComponent();
        }

        private void updateCB()
        {
            //yêu cầu main screen cập nhật lại category
            categoryChanged();

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
                //quay lại form chọn category để chỉnh sửa
                if (fillForm.Visibility == Visibility.Visible)
                {
                    functionForm.Visibility = Visibility.Collapsed;
                    btnEdit.Visibility = Visibility.Visible;
                    fillForm.Visibility = Visibility.Collapsed;
                    updateCB();
                    return;
                }
                
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
            //this.Content = new ManagerCatogoryScreen();
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (((Category)cbEdit.SelectedItem) == null)
            {
                MessageBox.Show("Vui lòng chọn danh mục cần chỉnh sửa");
            }
            else
            {
                btnEdit.Visibility = Visibility.Collapsed;
                fillForm.Visibility = Visibility.Visible;
            }
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            Category oldCategory = (Category)cbEdit.SelectedItem;
            string oldIcon = oldCategory.Icon;

            //Lưu lại danh mục được chỉnh sửa
            if (string.IsNullOrEmpty(txtCatogoryNameEdit.Text))
            {
                MessageBox.Show("Vui lòng nhập tên danh mục");
                return;
            }
            string newName = txtCatogoryNameEdit.Text;
            string newIcon = iconCategorySelected.Source.ToString();

            bool isUsingOldIcon = Global.isUsingtheOldFile(newIcon, oldIcon);

            if (newName.Equals(oldCategory.Name) && isUsingOldIcon)
            {
                return;
            }

            string iconFullName;
            string sourcePath = "";

            //Nếu xử dụng old icon
            if (isUsingOldIcon)
            {
                iconFullName = oldIcon;
            }
            else
            {
                //Tạo tên mới cho icon 
                string iconExtension = Global.getExtensionOfFile(newIcon);
                string iconName = Global.makeFileNameBy(newName);
                iconFullName = iconName + iconExtension;

                //Xử lí path của icon về chuẩn hàm File.Copy
                sourcePath = newIcon.Remove(0, 8).Replace("/", @"\");
            }
            //Lấy thư mục chứa file icon của app
            string iconFolder = Global.getBaseFolder() + @"\Images\category\";


            var oldCategoryName = oldCategory.Name;

            Category newCategory = new Category()
            {
                Name = newName,
                Icon = iconFullName
            };


            if (masterDataManager.updateCategory(oldCategory, newCategory))
            {
                if (oldCategoryName != newCategory.Name)
                {
                    MessageBox.Show($"Cập nhật {oldCategoryName} thành {newCategory.Name} thành công");

                }
                else
                {
                    MessageBox.Show($"Cập nhật {newCategory.Name} thành công");
                }
                if (!isUsingOldIcon)
                {
                    Global.copyFileTo(sourcePath, iconFolder + iconFullName);
                }
                updateCB();
                //set txtCatogoryNameEdit when update success
                txtCatogoryNameEdit.Text = newName;
            }
            else
            {
                MessageBox.Show($"Không thể sửa. {newCategory.Name} đã tồn tại");
            }

        }

        private void btnAddCategory_Click(object sender, RoutedEventArgs e)
        {
            string categoryName = txtCatogoryName.Text;
            string iconFullName;
            string sourcePath = "";

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

            Category newCategory = new Category()
            {
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
                if (!Global.isUsingtheOldFile(reviewIcon.Source.ToString(), "default-category-icon.ico"))
                {
                    Global.copyFileTo(sourcePath, iconFolder + iconFullName);
                }
                updateCB();
                //refresh name and icon
                txtCatogoryName.Text = "";
                reviewIcon.Source = Global.loadBitmapImageFrom($"{Global.getBaseFolder()}\\Images\\category\\default-category-icon.ico");
            }
            else
            {
                MessageBox.Show($"Không thể thêm. {newCategory.Name} đã tồn tại");
            }
        }

        private void btnDelCategory_Click(object sender, RoutedEventArgs e)
        {
            var category = ((Category)cbDel.SelectedItem);

            if (category == null)
            {
                MessageBox.Show("Vui lòng chọn danh mục cần xóa", "Lỗi");
            }
            else
            {

                if (masterDataManager.deleteCategory(category.ID))
                {
                    //Đã xử lý(xóa) icon
                    //khác icon default thì xóa
                    if (!category.Icon.Equals("default-category-icon.ico"))
                    {
                        Global.deleteFile($"{Global.getBaseFolder()}\\Images\\category\\{category.Icon}");
                    }
                    MessageBox.Show($"Xóa {category.Name} thành công");
                    updateCB();

                }
                else
                {
                    MessageBox.Show($"Xóa {category.Name} thất bại. Còn sản phẩm thuộc loại này");
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
            if (icon != null)
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

        private void cbEdit_selectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Category category = ((Category)cbEdit.SelectedItem);
            if (category != null)
            {
                txtCatogoryNameEdit.Text = category.Name;
                iconCategorySelected.Source = Global.loadBitmapImageFrom($"{Global.getBaseFolder()}\\Images\\category\\{category.Icon}");
            }
        }
    }
}