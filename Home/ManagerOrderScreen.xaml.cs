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
    /// Interaction logic for ManagerOrderScreen.xaml
    /// </summary>
    public partial class ManagerOrderScreen : UserControl
    {
        public ManagerOrderScreen()
        {
            InitializeComponent();
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
            //var item = sender as ListViewItem;
            //var data = item.DataContext as Order;
            //OderDetailScreen screen = new OderDetailScreen(data);
            //oderDetail.Content = screen;
        }

        private void AddNewOrder_MouseEnter(object sender, MouseEventArgs e)
        {
            addtooltip.Visibility = Visibility.Visible;
        }

        private void AddNewOrder_MouseLeave(object sender, MouseEventArgs e)
        {
            addtooltip.Visibility = Visibility.Hidden;
        }

    }
}
