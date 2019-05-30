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
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Home
{
    /// <summary>
    /// Interaction logic for SplashScreen.xaml
    /// </summary>
    /// 
    
    public partial class SplashScreen : Window
    {

        DispatcherTimer dp = new DispatcherTimer();

        public SplashScreen()
        {
            InitializeComponent();
            this.AllowsTransparency = true;
            dp.Tick += new EventHandler(dpTick);
            dp.Interval = new TimeSpan(0, 0, 3);
            dp.Start();
        }


        private void dpTick(object sender, EventArgs e)
        {
            MainWindow mainScreen = new MainWindow();
            mainScreen.Show();

            dp.Stop();
            this.Close();
        }
    }
}
