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
    /// Interaction logic for Statistical.xaml
    /// </summary>
    public partial class Statistical : UserControl
    {
        private MasterDataManager masterDataManager;
        private int type = -1;

        public Statistical()
        {
            InitializeComponent();
            masterDataManager = MasterDataManager.getInstance();

            //set ngày mặc định
            //nó k hiện, khi chọn ngày tháng nó mới hiện => fix đi nhật :v
            fromDate.DisplayDate = DateTime.Today.AddDays(-7);
            toDate.DisplayDate = DateTime.Today;

            fromDate.DisplayDateEnd = DateTime.Today;
            toDate.DisplayDateEnd = DateTime.Today;

            statiscalContent.Visibility = Visibility.Collapsed;
            statiscalOption.Visibility = Visibility.Visible;
        }

        private void statisticalByMonth_click(object sender, RoutedEventArgs e)
        {
            type = 0;
            
            //setting cho nó chỉ chọn tháng và năm thôi
            //chưa làm

            statiscalOption.Visibility = Visibility.Collapsed;
            pieChart.Visibility = Visibility.Collapsed;
            colChart.Visibility = Visibility.Collapsed;
            statiscalContent.Visibility = Visibility.Visible;
            lineChart.Visibility = Visibility.Visible;

            linechart.ItemsSource = masterDataManager.getStatisticalByMonth(
                fromDate.DisplayDate,
                toDate.DisplayDate
            );
        }

        private void statisticalByDate_click(object sender, RoutedEventArgs e)
        {
            type = 1;
            statiscalOption.Visibility = Visibility.Collapsed;
            pieChart.Visibility = Visibility.Collapsed;
            lineChart.Visibility = Visibility.Collapsed;
            statiscalContent.Visibility = Visibility.Visible;
            colChart.Visibility = Visibility.Visible;


            colchart.ItemsSource = masterDataManager.getStatisticalByDate(
                fromDate.DisplayDate,
                toDate.DisplayDate
            );
        }

        private void statisticalContributeByDate_click(object sender, RoutedEventArgs e)
        {
            type = 2;
            statiscalOption.Visibility = Visibility.Collapsed;
            colChart.Visibility = Visibility.Collapsed;
            lineChart.Visibility = Visibility.Collapsed;
            statiscalContent.Visibility = Visibility.Visible;
            pieChart.Visibility = Visibility.Visible;

            piechart.ItemsSource = masterDataManager.getStatisticalProductsContributeByDate(
                fromDate.DisplayDate,
                toDate.DisplayDate
            );
        }

        private void btnBack_click(object sender, RoutedEventArgs e)
        {
            statiscalContent.Visibility = Visibility.Collapsed;
            statiscalOption.Visibility = Visibility.Visible;
            colChart.Visibility = Visibility.Collapsed;
            pieChart.Visibility = Visibility.Collapsed;
            linechart.Visibility = Visibility.Collapsed;
        }

        private void btnStatistical_click(object sender, RoutedEventArgs e)
        {
            switch (type)
            {
                case 0:
                    statisticalByMonth_click(null, null);
                    break;
                case 1:
                    statisticalByDate_click(null, null);
                    break;
                case 2:
                    statisticalContributeByDate_click(null, null);
                    break;
            }
        }

        private void fromDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            //giới hạn vùng chọn ngày giữa ngày bắt đầu và kết thúc
            toDate.DisplayDateStart = fromDate.DisplayDate;
        }
    }
}
