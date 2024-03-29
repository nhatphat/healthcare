﻿using Home.models;
using Home.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
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
    /// Interaction logic for CreateNewOrderScreen.xaml
    /// </summary>
    public partial class CreateNewOrderScreen : UserControl
    {
        private MasterDataManager masterDataManager;
        private List<Cosmetic> allCosmetics;
        private BindingList<ProductOfOrder> cosmeticsSelected;

        public CreateNewOrderScreen()
        {
            InitializeComponent();

            masterDataManager = MasterDataManager.getInstance();
            allCosmetics = masterDataManager.getAllCosmetic();
            cosmeticsSelected = new BindingList<ProductOfOrder>();
        }

        private int getIndexProductInCosmeticsSelectedById(int id)
        {
            for (int i = 0; i < cosmeticsSelected.Count; i++)
            {
                if (cosmeticsSelected[i].ID == id)
                {
                    return i;
                }
            }

            return -1;
        }

        private void Product_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //Thêm sản phẩm vào đơn hàng, nếu đã có trong đơn hàng số lượng++ 
            var context = sender as StackPanel;
            var data = context.DataContext as Cosmetic;
            int index = getIndexProductInCosmeticsSelectedById(data.ID);

            //new product
            if (index == -1)
            {
                cosmeticsSelected.Add(new ProductOfOrder
                {
                    ID = data.ID,
                    Name = data.Name,
                    Image = data.Image,
                    Price = data.Price,
                    Quantity = 1
                });

                // update new index
                index = cosmeticsSelected.Count - 1;
            }
            // exists product -> count ++
            else
            {
                cosmeticsSelected[index].Quantity += 1;
            }

            listCosmeticSelected.ItemsSource = cosmeticsSelected;

            //scroll to position affected
            listCosmeticSelected.ScrollIntoView(cosmeticsSelected[index]);
            updateTotalPriceOfOrder();
        }

        private void updateTotalPriceOfOrder()
        {
            int total = 0;
            foreach (var product in cosmeticsSelected)
            {
                total += product.Total;
            }

            totalAmount.Content = total == 0 ? "0" : string.Format("{0:0,0}", total);
        }

        private void Searchingtxt_GotFocus(object sender, RoutedEventArgs e)
        {
            placeholdertext.Visibility = Visibility.Collapsed;
        }

        private void Searchingtxt_LostFocus(object sender, RoutedEventArgs e)
        {
            if (searchingtxt.Text == "")
            {
                placeholdertext.Visibility = Visibility.Visible;
            }
        }

        private void CreateOrderbtn_Click(object sender, RoutedEventArgs e)
        {
            var customerName = txtCustomerName.Text;
            var phoneNumber = txtCustomerPhone.Text;
            var customerAddress = txtCustomerAddress.Text;

            if (Regex.IsMatch(customerName, "^\\s+") || customerName.Equals(""))
            {
                MessageBox.Show("Vui lòng nhập họ tên khách hàng.");
            }
            else if (Regex.IsMatch(phoneNumber, "^\\s+") || phoneNumber.Equals("") || Regex.IsMatch(phoneNumber, "\\D+"))
            {
                MessageBox.Show("Vui lòng nhập số điện thoại của khách hàng và đúng định dạng.");
            }
            else if (Regex.IsMatch(customerAddress, "^\\s+") || customerAddress.Equals(""))
            {
                MessageBox.Show("Vui lòng nhập địa chỉ của khách hàng.");
            }
            else
            {
                listCosmetic.ItemsSource = allCosmetics;

                //Hiển thị lại thông tin khách hàng đã nhập ở form trước
                lbCustomerName.Content = customerName;
                lbCustomerPhone.Content = phoneNumber;
                lbCustomerAdd.Text = customerAddress;
                totalAmount.Content = 0;

                customerInfoForm.Visibility = Visibility.Collapsed;

                addProductIntoOrder.Visibility = Visibility.Visible;
                //Khi nào click vào sản phẩm thì listview sản phẩm đã chọn bên phải thêm 
                //sản phẩm đó vào, nếu đã có trong danh sách thì tăng số lượng lên,
                //sau đó tăng tổng hóa đơn lên, 
                //
                //listCosmeticSelected.ItemsSource = allCosmetics;
            }
        }

        private void searchingtxt_TextChanged(object sender, TextChangedEventArgs e)
        {
            var keySearch = (string)searchingtxt.Text;
            // check key search not empty
            if (Regex.IsMatch(keySearch, "^\\s+") || keySearch.Equals(""))
            {
                listCosmetic.ItemsSource = allCosmetics;
                return;
            }
            var result = masterDataManager.searchCosmeticsByName(keySearch);
            listCosmetic.ItemsSource = result;
        }

      

        private void saveOrder_click(object sender, RoutedEventArgs e)
        {
            int total = int.Parse(totalAmount.Content.ToString().Replace(",", ""));
            if (total == 0)
            {
                MessageBox.Show("Đơn hàng rỗng, vui lòng chọn sản phẩm");
                return;
            }

            var jsonManager = new JavaScriptSerializer();
            var order = new Order
            {
                Status = Order.C_NEW,
                Products = jsonManager.Serialize(cosmeticsSelected),
                CustomerName = lbCustomerName.Content.ToString(),
                CustomerTel = lbCustomerPhone.Content.ToString(),
                CustomerAddr = lbCustomerAdd.Text,
                TotalPrice = total
            };
            if (masterDataManager.addNewOrder(order))
            {
                MessageBox.Show("Thêm đơn hàng thành công");
                this.Content = new ManagerOrderScreen();
            }
            else
            {
                MessageBox.Show("Thêm đơn hàng thất bại. Vui lòng thử lại hoặc liên hệ kỹ thuật viên");
            }
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            if (customerInfoForm.Visibility == Visibility.Visible)
            {
                this.Content = new ManagerOrderScreen();
            }
            else if (addProductIntoOrder.Visibility == Visibility.Visible)
            {
                // reset info
                txtCustomerName.Text = "";
                txtCustomerPhone.Text = "";
                txtCustomerAddress.Text = "";
                cosmeticsSelected.Clear();

                addProductIntoOrder.Visibility = Visibility.Collapsed;
                customerInfoForm.Visibility = Visibility.Visible;
            }
        }

       

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void Decrease_Click(object sender, RoutedEventArgs e)
        {
            var context = (((sender as Button).Parent as StackPanel).Parent as StackPanel).Parent as Grid;
            var data = context.DataContext as ProductOfOrder;
            int index = getIndexProductInCosmeticsSelectedById(data.ID);
            if (index != -1)
            {
                if (cosmeticsSelected[index].Quantity > 1)
                {
                    cosmeticsSelected[index].Quantity -= 1;
                    updateTotalPriceOfOrder();
                }
            }
        }

        private void Increase_Click(object sender, RoutedEventArgs e)
        {
            var context = (((sender as Button).Parent as StackPanel).Parent as StackPanel).Parent as Grid;
            var data = context.DataContext as ProductOfOrder;

            int index = getIndexProductInCosmeticsSelectedById(data.ID);
            if (index != -1)
            {
                cosmeticsSelected[index].Quantity += 1;
                updateTotalPriceOfOrder();
            }

        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            var context = ((sender as Button).Parent as StackPanel).Parent as Grid;
            var data = context.DataContext as ProductOfOrder;

            int index = getIndexProductInCosmeticsSelectedById(data.ID);
            if (index != -1)
            {
                cosmeticsSelected.RemoveAt(index);
                updateTotalPriceOfOrder();
            }
        }
    }
}


