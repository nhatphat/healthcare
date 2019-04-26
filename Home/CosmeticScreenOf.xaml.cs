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
    /// Interaction logic for CosmeticScreenOf.xaml
    /// </summary>
    public partial class CosmeticScreenOf : UserControl
    {
        public CosmeticScreenOf(List<Cosmetic> cosmetics)
        {
            InitializeComponent();

            var dbManager = DBManager.getInstance();

            listCosmetic.ItemsSource = cosmetics;

        }


        private void Product_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var stack = sender as StackPanel;
            var data = stack.DataContext as Cosmetic;
            ProductDetailScreen screen = new ProductDetailScreen(data);
            this.Content = screen;
        }



    }
}
