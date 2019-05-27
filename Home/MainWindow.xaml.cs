using Home.models;
using Home.Utils;
using Microsoft.Win32;
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
        //using for excel
        //private DBManager dbManager;

        //using for SQL Server
        public delegate void categoryChanged();

        public interface ICategoryChangeListenter{
            event categoryChanged OnCategoryChangeListener;
        }

        private MasterDataManager masterDataManager;

        public bool? Form { get; private set; }

        public MainWindow()
        {
            InitializeComponent();
            //dbManager = DBManager.getInstance();
            masterDataManager = MasterDataManager.getInstance();

            //listCategory.ItemsSource = dbManager.getAllCategoryName();
            loadAllCategory();
            
            

            this.Closed += new EventHandler(closed);
        }

        private void loadAllCategory()
        {
            listCategory.ItemsSource = masterDataManager.getAllCategory();
        }

        private void closed(object sender, EventArgs e)
        {
            // save all changed again
            masterDataManager.saveChanged();
        }

        private void BtnOpenMenu_Click(object sender, RoutedEventArgs e)
        {
            btnCloseMenu.Visibility = Visibility.Visible;
            btnOpenMenu.Visibility = Visibility.Collapsed;
  

        }

        private void BtnCloseMenu_Click(object sender, RoutedEventArgs e)
        {
            btnOpenMenu.Visibility = Visibility.Visible;
            btnCloseMenu.Visibility = Visibility.Collapsed;
    

        }

        private void Category_click(object sender, MouseButtonEventArgs e)
        {
            var stackPanel = sender as StackPanel;
            var data_context = stackPanel.DataContext;
            var category_selected = data_context as Category;
            currentCatogoryName.Text = category_selected.Name;

            addChildForm(new CosmeticScreenOf(category_selected.ID));
        }

        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
           
        }

        private void managerProduct_click(object sender, MouseButtonEventArgs e)
        {
            var file = Global.getFile();
            if(file != null)
            {
                Global.copyFileTo(file.FileName, $"{Global.getBaseFolder()}\\Images\\{file.SafeFileName}");
            }
        }

        private void addChildForm(UserControl child)
        {
            parentForm.Content = child;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            currentCatogoryName.Text = "Trang chủ";
            addChildForm(new HomeScreen());
        }

        private void Grid_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            currentCatogoryName.Text = "Trang chủ";
            addChildForm(new HomeScreen());
        }

        private void BtnCloseWindow_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnMinimize_click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void BtnMinimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

      
        private void CatogoryManager_Click(object sender, RoutedEventArgs e)
        {
            currentCatogoryName.Text = "Quản lý danh mục";
            var managerCategoryScreen = new ManagerCatogoryScreen();
            managerCategoryScreen.OnCategoryChangeListener += () =>
            {
                loadAllCategory();
            };
            addChildForm(managerCategoryScreen);
        }

        private void ProductManager_Click(object sender, RoutedEventArgs e)
        {
            currentCatogoryName.Text = "Quản lý sản phẩm";
            addChildForm(new ManagerProductScreen());
        }
        
    }
}
