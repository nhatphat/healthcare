using Home.models;
using Home.Utils;
using System;
using System.Collections.Generic;
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
    /// Interaction logic for CreateNewOrderScreen.xaml
    /// </summary>
    public partial class CreateNewOrderScreen : UserControl
    {
        private MasterDataManager masterDataManager;
        List<Cosmetic> allCosmetics;
        public CreateNewOrderScreen()
        {
            masterDataManager = MasterDataManager.getInstance();
            InitializeComponent();
            allCosmetics = masterDataManager.getAllCosmetic();
        }

        private void Product_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //Thêm sản phẩm vào đơn hàng, nếu đã có trong đơn hàng số lượng++ 
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

            if(String.IsNullOrEmpty(customerName))
            {
                MessageBox.Show("Vui lòng nhập họ tên khách hàng");
            }
            else if(String.IsNullOrEmpty(phoneNumber))
            {
                MessageBox.Show("Vui lòng nhập số điện thoại của khách hàng");
            }
            else if (String.IsNullOrEmpty(customerAddress))
            {
                MessageBox.Show("Vui lòng nhập địa chỉ của khách hàng");
            }
            else
            {
                listCosmetic.ItemsSource = allCosmetics;

                //Hiển thị lại thông tin khách hàng đã nhập ở form trước
                lbCustomerName.Content = customerName;
                lbCustomerPhone.Content = phoneNumber;
                lbCustomerAdd.Text = customerAddress;

                customerInfoForm.Visibility = Visibility.Collapsed;

                addProductIntoOrder.Visibility = Visibility.Visible;
                //Khi nào click vào sản phẩm thì listview sản phẩm đã chọn bên phải thêm 
                //sản phẩm đó vào, nếu đã có trong danh sách thì tăng số lượng lên,
                //sau đó tăng tổng hóa đơn lên, 
                //
                listCosmeticSelected.ItemsSource = allCosmetics;
            }

           

        }
    }
}


