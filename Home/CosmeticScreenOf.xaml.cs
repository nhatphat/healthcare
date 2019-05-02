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
        private MasterDataManager masterDataManager;
        List<Cosmetic> cosmetics;
        public CosmeticScreenOf(int categoryId)
        {
            InitializeComponent();

            masterDataManager = MasterDataManager.getInstance();
            cosmetics = masterDataManager.getAllCosmeticOfCategory(categoryId);

            if(cosmetics.Count == 0)
            {
                emptyCategory.Visibility = Visibility.Visible;
                paging.Visibility = Visibility.Collapsed;
                listCosmetic.Visibility = Visibility.Collapsed;
            }

            itemsPerPage = 10;
            double dbPage = cosmetics.Count / (itemsPerPage * 1.0);
            totalPages = dbPage < 1 ? 1 : dbPage == 1 ? 1 : (int)dbPage + 1;



            listCosmetic.ItemsSource = cosmetics.Skip((0) * 10)
                .Take(10); ;
            pagingInfoLabel.Content = $"Page {currentPage} of {totalPages}";
        }

        int currentPage = 1;
        int itemsPerPage = 10;
        int totalPages = 0;

        private void Product_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var stack = sender as StackPanel;
            var data = stack.DataContext as Cosmetic;
            ProductDetailScreen screen = new ProductDetailScreen(data);
            this.Content = screen;
        }

        private void PreviousButton_Click(object sender, RoutedEventArgs e)
        {
            if (currentPage > 1)
            {
                currentPage--;
                listCosmetic.ItemsSource = cosmetics
                    .Skip((currentPage - 1) * itemsPerPage)
                    .Take(itemsPerPage);
                pagingInfoLabel.Content = $"Page {currentPage} of {totalPages}";

            }
        }

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            if (currentPage < totalPages)
            {
                currentPage++;
                listCosmetic.ItemsSource = cosmetics
                    .Skip((currentPage - 1) * itemsPerPage)
                    .Take(itemsPerPage);
                pagingInfoLabel.Content = $"Page {currentPage} of {totalPages}";

            }
        }

    }
}
