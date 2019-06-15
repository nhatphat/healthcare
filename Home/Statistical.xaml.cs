using Home.Utils;
using System;
using System.Collections.Generic;
using System.Globalization;
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
        BrushConverter bc = new BrushConverter();


        public Statistical()
        {
            InitializeComponent();
            masterDataManager = MasterDataManager.getInstance();

            //set ngày mặc định
            //nó k hiện, khi chọn ngày tháng nó mới hiện => fix đi nhật :v
            //Đã fix
            fromDate.DisplayDate = DateTime.Today.AddDays(-7);
            toDate.DisplayDate = DateTime.Today;
            fromDate.Text = DateTime.Today.ToString();
            toDate.Text = DateTime.Today.ToString();
            monthPicker.Text = DateTime.Today.ToString();
            statisticalByMonth_click(new object(), new RoutedEventArgs());


         
        }

        private void statisticalByMonth_click(object sender, RoutedEventArgs e)
        {         
            EventWhenAButton_Click(btnStatisticalByMonth);
            type = 0;
            
            //setting cho nó chỉ chọn tháng và năm thôi
            //chưa làm

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
            EventWhenAButton_Click(btnStatisticalByDate);
            type = 1;

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
            EventWhenAButton_Click(btnStatisticalContributeByDate);
            type = 2;

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
            this.Content = new HomeScreen();
        }

        private void btnStatistical_click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(monthPicker.Text.ToString());
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





        // Mấy cái này hiệu ứng hover leave này kia thôi.
        #region Event when click, mouse enter(hover) and leave

        bool btnStatisticalByMonth_State = false;
        bool btnStatisticalByDate_State = false;
        bool btnStatisticalContributeByDate_State = false;


        private void EventWhenAButton_Click(Button btn_clicked)
        {
            if (btn_clicked == btnStatisticalByMonth)
            {
                //unselect 2 nút kia
                unSelected(btnStatisticalByDate);
                unSelected(btnStatisticalContributeByDate);
                btnStatisticalByDate_State = false;
                btnStatisticalContributeByDate_State = false;
                ChooseDateFromTo.Visibility = Visibility.Collapsed;

                //select this
                Selected(btnStatisticalByMonth);
                btnStatisticalByMonth_State = true;
                ChooseMonthOnly.Visibility = Visibility.Visible;
                //
            }
            else if (btn_clicked == btnStatisticalByDate)
            {
                //unselect 2 nút kia
                unSelected(btnStatisticalByMonth);
                unSelected(btnStatisticalContributeByDate);
                btnStatisticalByMonth_State = false;
                btnStatisticalContributeByDate_State = false;
                ChooseMonthOnly.Visibility = Visibility.Collapsed;

                //select this
                Selected(btnStatisticalByDate);
                btnStatisticalByDate_State = true;
                ChooseDateFromTo.Visibility = Visibility.Visible;

                //
            }
            else
            {
                //unselect 2 nút kia
                unSelected(btnStatisticalByMonth);
                unSelected(btnStatisticalByDate);
                btnStatisticalByMonth_State = false;
                btnStatisticalByDate_State = false;
                ChooseMonthOnly.Visibility = Visibility.Collapsed;

                //select this
                Selected(btnStatisticalContributeByDate);
                btnStatisticalContributeByDate_State = true;
                ChooseDateFromTo.Visibility = Visibility.Visible;

                //
            }
        }


        private void BtnStatisticalByMonth_MouseEnter(object sender, MouseEventArgs e)
        {
            Selected(btnStatisticalByMonth);
        }

        private void BtnStatisticalByMonth_MouseLeave(object sender, MouseEventArgs e)
        {
            if (!btnStatisticalByMonth_State)
            {
                unSelected(btnStatisticalByMonth);
            }
        }
        private void Selected(Button btn)
        {
            btn.Background = (Brush)bc.ConvertFrom("#FFF78787");
            btn.Foreground = Brushes.White;
        }
        private void unSelected(Button btn)
        {
            btn.Background = Brushes.Transparent;
            btn.Foreground = Brushes.Black;
        }

        private void BtnStatisticalByDate_MouseEnter(object sender, MouseEventArgs e)
        {
            Selected(btnStatisticalByDate);
        }

        private void BtnStatisticalByDate_MouseLeave(object sender, MouseEventArgs e)
        {
            if (!btnStatisticalByDate_State)
            {
                unSelected(btnStatisticalByDate);
            }
        }

        private void BtnStatisticalContributeByDate_MouseEnter(object sender, MouseEventArgs e)
        {
            Selected(btnStatisticalContributeByDate);
        }

        private void BtnStatisticalContributeByDate_MouseLeave(object sender, MouseEventArgs e)
        {
            if (!btnStatisticalContributeByDate_State)
            {
                unSelected(btnStatisticalContributeByDate);
            }
        }
        #endregion

        private void BtnCloseDatePicker_Click(object sender, RoutedEventArgs e)
        {
            btnOpenDatePicker.IsEnabled = true;
        }

        private void BtnOpenDatePicker_Click(object sender, RoutedEventArgs e)
        {
            btnOpenDatePicker.IsEnabled = false;
        }
    }
}
