﻿using Home.models;
using Home.Utils;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
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
                category = new BindingList<Category>(masterDataManager.getAllCategory());
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
            if (productFullDetail.Visibility == Visibility.Visible)
            {
                productFullDetail.Visibility = Visibility.Collapsed;
            }
            if (EditProductSelectedForm.Visibility == Visibility.Visible)
            {
                EditProductSelectedForm.Visibility = Visibility.Collapsed;
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
            bool result = masterDataManager.deleteCosmetic(cosmetic.ID);
            if (result)
            {
                MessageBox.Show($"Đã xóa {cosmetic.Name}", "Thành công");
                if (!cosmetic.Image.Equals("default_cosmetic_icon.ico"))
                {
                    Global.deleteFile($"{Global.getBaseFolder()}\\Images\\cosmetic\\{cosmetic.Image}");
                }
                BtnBack_Click(null, null);
                DeleteProduct_Click(null, null);
            }
            else
            {
                MessageBox.Show($"Xóa {cosmetic.Name} không thành công", "Lỗi");
            }

        }

        private void AddProductFrmAdd_Click(object sender, RoutedEventArgs e)
        {
            int price;

            if (string.IsNullOrEmpty(txtProductName.Text) || string.IsNullOrEmpty(txtProductPrice.Text) || string.IsNullOrEmpty(txtProductOrgin.Text) || string.IsNullOrEmpty(txtProductDetail.Text))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin!", "Lỗi");
                return;
            }

            if (!int.TryParse(txtProductPrice.Text, out price))
            {
                MessageBox.Show("Giá của sản phẩm không hợp lệ!", "Lỗi");
                return;
            }

            int categoryId = ((Category)cbCatogoryFrmAdd.SelectedItem).ID;
            Cosmetic cosmetic = new Cosmetic
            {
                Category = categoryId,
                Name = txtProductName.Text,
                Price = price,
                Origin = txtProductOrgin.Text,
                Detail = txtProductDetail.Text
            };
            
            string iconFullName = "";
            string sourcePath = "";

            if (Global.isUsingtheOldFile(reviewIcon.Source.ToString(), cosmetic.Image))
            {
                iconFullName = cosmetic.Image;//default image
            }
            else
            {
                initNewImageProduct(reviewIcon.Source.ToString(), cosmetic.Name, out sourcePath, out iconFullName);
                cosmetic.Image = iconFullName;//update new image
            }
            //Lấy thư mục chứa file icon của app
            string iconFolder = Global.getBaseFolder() + @"\Images\cosmetic\";

            bool result = masterDataManager.addNewCosmetic(cosmetic);
            if (result)
            {
                //cosmetic.row_in_db = result;
                MessageBox.Show($"Thêm {cosmetic.Name} thành công", "Thành công");
                if (!cosmetic.Image.Equals("default_cosmetic_icon.ico"))
                {
                    Global.copyFileTo(sourcePath, iconFolder + iconFullName);
                }
                refreshFormAddProduct();

            }
            else
            {
                MessageBox.Show($"Không thể thêm {cosmetic.Name}. Vui lòng thử lại", "Lỗi");
            }
        }

        private void initNewImageProduct(string imagePath, string name, out string newImagePath, out string newName)
        {
            //Tạo tên mới cho icon 
            string iconExtension = Global.getExtensionOfFile(imagePath);
            string iconName = Global.makeFileNameBy(name);
            newName = iconName + iconExtension;
            
            //Xử lí path của icon về chuẩn hàm File.Copy
            newImagePath = reviewIcon.Source.ToString().Remove(0, 8).Replace("/", @"\");
        }

        private void refreshFormAddProduct()
        {
            txtProductName.Text = "";
            txtProductPrice.Text = "";
            txtProductOrgin.Text = "";
            txtProductDetail.Text = "";
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
            EditProductSelectedForm.Visibility = Visibility.Visible;
            var cosmetic = ((Cosmetic)cbCosmeticEdit.SelectedItem);

            //
            imgEditImageProduct.Source = new BitmapImage(
                new Uri(
                    $"{Global.getBaseFolder()}\\Images\\cosmetic\\{cosmetic.Image}"
                )
            );
            tbEditNameProduct.Text = cosmetic.Name;
            tbEditPriceProduct.Text = cosmetic.Price.ToString();
            tbEditOriginProduct.Text = cosmetic.Origin;
            tbEditDetailProduct.Text = cosmetic.Detail;

            //cbEditCategoryOfProduct.SelectedItem = cosmetic;
            cbEditCategoryOfProduct.DisplayMemberPath = cosmetic.Name;
        }

        private void ChooseIconProduct_Click(object sender, RoutedEventArgs e)
        {
            //// có thể thay thế getImage()
            //OpenFileDialog icon = Global.getFile();
            //if (icon != null)
            //{
            //    reviewIcon.Source = new BitmapImage(
            //        new Uri(
            //            icon.FileName,
            //            UriKind.Absolute
            //        )
            //    );
            //}

            BitmapImage icon = Global.getImage();
            if (icon != null)
            {
                reviewIcon.Source = icon;
            }
        }

        private void EditProductSelected_Click(object sender, RoutedEventArgs e)
        {
            int price;

            if (string.IsNullOrEmpty(tbEditNameProduct.Text) || string.IsNullOrEmpty(tbEditPriceProduct.Text) || string.IsNullOrEmpty(tbEditOriginProduct.Text) || string.IsNullOrEmpty(tbEditDetailProduct.Text))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin!", "Lỗi");
                return;
            }

            if (!int.TryParse(tbEditPriceProduct.Text, out price))
            {
                MessageBox.Show("Giá của sản phẩm không hợp lệ!", "Lỗi");
                return;
            }

            int categoryId = ((Category)cbCatogoryFrmAdd.SelectedItem).ID;
            Cosmetic cosmetic = new Cosmetic
            {
                Category = categoryId,
                Name = txtProductName.Text,
                Price = price,
                Origin = txtProductOrgin.Text,
                Detail = txtProductDetail.Text
            };

            string iconFullName = "";
            string sourcePath = "";

            if (Global.isUsingtheOldFile(reviewIcon.Source.ToString(), cosmetic.Image))
            {
                iconFullName = cosmetic.Image;//default image
            }
            else
            {
                initNewImageProduct(reviewIcon.Source.ToString(), cosmetic.Name, out sourcePath, out iconFullName);
                cosmetic.Image = iconFullName;//update new image
            }
            //Lấy thư mục chứa file icon của app
            string iconFolder = Global.getBaseFolder() + @"\Images\cosmetic\";

            bool result = masterDataManager.addNewCosmetic(cosmetic);
            if (result)
            {
                //cosmetic.row_in_db = result;
                MessageBox.Show($"Thêm {cosmetic.Name} thành công", "Thành công");
                if (!cosmetic.Image.Equals("default_cosmetic_icon.ico"))
                {
                    Global.copyFileTo(sourcePath, iconFolder + iconFullName);
                }
                refreshFormAddProduct();

            }
            else
            {
                MessageBox.Show($"Không thể thêm {cosmetic.Name}. Vui lòng thử lại", "Lỗi");
            }
        }

        private void ChooseEditIconProduct_Click(object sender, RoutedEventArgs e)
        {
            BitmapImage icon = Global.getImage();
            if (icon != null)
            {
                imgEditImageProduct.Source = icon;
            }
        }
    }
}
