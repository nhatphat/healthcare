using Home.models;
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
    /// Interaction logic for OrderDetailScreen.xaml
    /// </summary>
    public partial class OrderDetailScreen : UserControl
    {
        Order order = new Order();
        public OrderDetailScreen(Order data)
        {
            InitializeComponent();
            order = data;
            
            //Cái này màu mè thêm thôi
            if(order.Status ==1)
            {
                statusNow.Foreground = Brushes.DarkGreen;
            }
            else if (order.Status == 2)
            {
                statusNow.Foreground = Brushes.DarkRed;

            }


            orderDetailChosen.DataContext = order;
            listCosmeticOfOrder.ItemsSource = order.ListProducts;
            //Nếu trạng thái đơn hàng đã hủy thì không hiện nút thay đổi trạng thái
            //Thay vào đó là dòng thông báo đơn hàng đã hủy
            if (order.Status == 2)
            {
                changeStatus.Visibility = Visibility.Collapsed;
                theOrderCanceled.Visibility = Visibility.Visible;
            }
        }

        private void ChangeStatus_Click(object sender, RoutedEventArgs e)
        {
            changeStatus.Visibility = Visibility.Collapsed;
            completedAndCacel.Visibility = Visibility.Visible;
            if(order.Status == 1)
            {
                completed.Visibility = Visibility.Collapsed;
            }
        }

        private void Completed_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
