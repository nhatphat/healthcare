using Home.models;
using Home.Utils;
using System;
using System.Collections.Generic;
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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DBManager dbManager;

        public MainWindow()
        {
            InitializeComponent();

            dbManager = DBManager.getInstance();

            this.Closed += new EventHandler(closed);
        }

        private void closed(object sender, EventArgs e)
        {
            // save all changed again
            dbManager.saveChanged();
        }
    }
}
