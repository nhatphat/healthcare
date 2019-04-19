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
        public CosmeticScreenOf(Category category)
        {
            InitializeComponent();

            var dbManager = DBManager.getInstance();

            this.DataContext = category;

            if(category.Cosmetics.Count > 0)
            {
                listCosmetic.ItemsSource = category.Cosmetics;
            }

        }

        private void Category_click(object sender, MouseButtonEventArgs e)
        {

        }
    }
}
