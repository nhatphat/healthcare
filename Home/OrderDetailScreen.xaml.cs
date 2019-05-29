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
    /// Interaction logic for OrderDetailScreen.xaml
    /// </summary>
    public partial class OrderDetailScreen : UserControl, ManagerOrderScreen.OnOrderChangedListner
    {
        private MasterDataManager masterDataManager;
        Order order = new Order();

        public event ManagerOrderScreen.orderChanged onOrderChangedListener;

        private void orderChanged()
        {
            onOrderChangedListener?.Invoke();
        }

        public OrderDetailScreen(Order data)
        {
            InitializeComponent();
            masterDataManager = MasterDataManager.getInstance();
            order = data;
            
            //Cái này màu mè thêm thôi
            //có gì sau này sửa chỉ sửa 1 chỗ thôi là máy chỗ khác tự động theo, hạn chế gán cứng
            if(order.Status == Order.C_COMPLETED)
            {
                statusNow.Foreground = Brushes.DarkGreen;
            }
            else if (order.Status == Order.C_CANCELED)
            {
                statusNow.Foreground = Brushes.DarkRed;

            }


            orderDetailChosen.DataContext = order;
            listCosmeticOfOrder.ItemsSource = order.ListProducts;
            totalAmount.Content = string.Format("{0:0,0}", order.TotalPrice);
            handleDisplayButtonChangedOrderStatus();
        }

        private void handleDisplayButtonChangedOrderStatus()
        {
            //Nếu trạng thái đơn hàng đã hủy thì không hiện nút thay đổi trạng thái
            //Thay vào đó là dòng thông báo đơn hàng đã hủy
            if (order.Status == Order.C_CANCELED)
            {
                changeStatus.Visibility = Visibility.Collapsed;
                completedAndCacel.Visibility = Visibility.Collapsed;
                theOrderCanceled.Visibility = Visibility.Visible;

            }
            else if (order.Status == Order.C_COMPLETED)
            {
                completed.Visibility = Visibility.Collapsed;
            }
        }

        private void ChangeStatus_Click(object sender, RoutedEventArgs e)
        {
            changeStatus.Visibility = Visibility.Collapsed;
            completedAndCacel.Visibility = Visibility.Visible;
            handleDisplayButtonChangedOrderStatus();
        }

        private void Completed_Click(object sender, RoutedEventArgs e)
        {
            if(masterDataManager.updateOrder(order.ID, Order.C_COMPLETED))
            {
                statusNow.Foreground = Brushes.DarkGreen;
                MessageBox.Show("Đã cập nhật trạng thái đơn hàng.");
                order.Status = Order.C_COMPLETED;
                orderChanged();
                handleDisplayButtonChangedOrderStatus();
               
            }
            else
            {
                MessageBox.Show("Cập nhật trạng thái đơn hàng không thành công.");
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            if (masterDataManager.updateOrder(order.ID, Order.C_CANCELED))
            {
                statusNow.Foreground = Brushes.DarkRed;
                MessageBox.Show("Đã cập nhật trạng thái đơn hàng.");
                order.Status = Order.C_CANCELED;
                orderChanged();
                handleDisplayButtonChangedOrderStatus();
               
            }
            else
            {
                MessageBox.Show("Cập nhật trạng thái đơn hàng không thành công.");
            }
        }
    }
}
