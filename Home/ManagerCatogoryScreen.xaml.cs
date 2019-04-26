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
    /// Interaction logic for ManagerCatogoryScreen.xaml
    /// </summary>
    public partial class ManagerCatogoryScreen : UserControl
    {
        public ManagerCatogoryScreen()
        {
            InitializeComponent();


        }

        private void updateCB()
        {
            var Catogory = getAllCategoryName();

            this.DataContext = new
            {
                Catogory = Catogory
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

            txtCatogoryNameEdit.Text = ((Category)cbEdit.SelectedItem).Name;
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            //Lưu lại danh mục được chỉnh sửa

            var name = txtCatogoryNameEdit.Text;

            if (string.IsNullOrEmpty(name))
            {
                MessageBox.Show("Vui lòng nhập tên danh mục");
                return;
            }

            if (DBManager.getInstance().editNameCategory(((Category)cbEdit.SelectedItem).Name, name))
            {
                MessageBox.Show($"Sửa {name} thành công");
            }
            else
            {
                MessageBox.Show($"Không thể sửa. {name} đã tồn tại");
            }
        }

        private BindingList<Category> getAllCategoryName()
        {
            var categoryName = new BindingList<Category>();

            var sheets = DBManager.getInstance().getAllSheet();
            foreach (var sheet in sheets)
            {
                categoryName.Add(new Category { Name = sheet.Name });
            }

            return categoryName;
        }

        private void btnAddCategory_Click(object sender, RoutedEventArgs e)
        {
            var name = txtCatogoryName.Text;
            if (string.IsNullOrEmpty(name))
            {
                MessageBox.Show("Vui lòng nhập tên danh mục");
                return;
            }

            if (DBManager.getInstance().addNewCategory(name))
            {
                MessageBox.Show($"Thêm {name} thành công");
            }
            else
            {
                MessageBox.Show($"Không thể thêm. {name} đã tồn tại");
            }
        }

        private void btnDelCategory_Click(object sender, RoutedEventArgs e)
        {
            var category = ((Category)cbDel.SelectedItem).Name;

            if (DBManager.getInstance().deleteCategoryWithName(category))
            {
                MessageBox.Show($"Xóa {category} thành công");
            }
            else
            {
                MessageBox.Show($"Xóa {category} thất bại. Còn sản phẩm thuộc loại này");
            }
        }
    }
}