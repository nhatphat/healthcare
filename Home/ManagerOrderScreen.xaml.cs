using Home.models;
using Home.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Interaction logic for ManagerOrderScreen.xaml
    /// </summary>
    public partial class ManagerOrderScreen : UserControl
    {
        public delegate void orderChanged();
        public interface OnOrderChangedListner
        {
            event orderChanged onOrderChangedListener;
        }

        private MasterDataManager masterDataManager;

        public ManagerOrderScreen()
        {
            InitializeComponent();
            masterDataManager = MasterDataManager.getInstance();

            updateOrderList();
        }

        

        #region Paging - Bỏ không dùng nữa 
        //int currentPage = 1;
        //int itemsPerPage = 10;
        //int totalPages = 0;
        //List<Cosmetic> cosmetics;
        //private void PreviousButton_Click(object sender, RoutedEventArgs e)
        //{
        //    if (currentPage > 1)
        //    {
        //        currentPage--;
        //        listOrder.ItemsSource = cosmetics
        //            .Skip((currentPage - 1) * itemsPerPage)
        //            .Take(itemsPerPage);
        //        pagingInfoLabel.Content = $"Page {currentPage} of {totalPages}";

        //    }
        //}
        //private void NextButton_Click(object sender, RoutedEventArgs e)
        //{
        //    if (currentPage < totalPages)
        //    {
        //        currentPage++;
        //        listOrder.ItemsSource = cosmetics
        //            .Skip((currentPage - 1) * itemsPerPage)
        //            .Take(itemsPerPage);
        //        pagingInfoLabel.Content = $"Page {currentPage} of {totalPages}";

        //    }
        //} 
        #endregion

        private void AddNewOrder_Click(object sender, RoutedEventArgs e)
        {
            this.Content = new CreateNewOrderScreen();
        }

        private void updateOrderList()
        {
            listOrder.ItemsSource = masterDataManager.LoadAllOrder();
        }

        private void Searchingtxt_GotFocus(object sender, RoutedEventArgs e)
        {
            placeholdertext.Visibility = Visibility.Collapsed;
        }

        private void Searchingtxt_LostFocus(object sender, RoutedEventArgs e)
        {
            if(searchingtxt.Text == "")
            {
                placeholdertext.Visibility = Visibility.Visible;
            }
        }

        private void ListViewItem_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //Click vào đơn hàng
            var item = sender as StackPanel;
            var data = item.DataContext as Order;
            OrderDetailScreen screen = new OrderDetailScreen(data);
            screen.onOrderChangedListener += () =>
            {
                updateOrderList();
            };
            oderDetail.Content = screen;
        }

        private void AddNewOrder_MouseEnter(object sender, MouseEventArgs e)
        {
            addtooltip.Visibility = Visibility.Visible;
        }

        private void AddNewOrder_MouseLeave(object sender, MouseEventArgs e)
        {
            addtooltip.Visibility = Visibility.Hidden;
        }

        private void searchingtxt_TextChanged(object sender, TextChangedEventArgs e)
        {
            var keySearch = searchingtxt.Text;
            // check key search not empty
            if (Regex.IsMatch(keySearch, "^\\s+") || keySearch.Equals(""))
            {
                updateOrderList();
                return;
            }
            var result = masterDataManager.searchOrdersByPhoneOrName(keySearch);
            listOrder.ItemsSource = result;
        }

        private void Order_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

        }
    }
}
