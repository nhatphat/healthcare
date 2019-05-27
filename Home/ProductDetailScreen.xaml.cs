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
    /// Interaction logic for ProductDetailScreen.xaml
    /// </summary>
    public partial class ProductDetailScreen : UserControl
    {
        public ProductDetailScreen(Cosmetic data)
        {
            InitializeComponent();

            Cosmetic product = data;
            productFullDetail.DataContext = data;

        }
    }
       
}
