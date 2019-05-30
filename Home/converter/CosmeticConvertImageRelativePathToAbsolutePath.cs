using Home.Utils;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Cache;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace Home.converter
{
    public class CosmeticConvertImageRelativePathToAbsolutePath : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var imagePath = $"{Global.getBaseFolder()}Images\\cosmetic\\{value.ToString()}";

            BitmapImage image = null;
            if (File.Exists(imagePath))
            {
                image = new BitmapImage();
                image.BeginInit();
                image.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
                //image.CacheOption = BitmapCacheOption.None;
                //image.UriCachePolicy = new RequestCachePolicy(RequestCacheLevel.BypassCache);
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.UriSource = new Uri(imagePath);
                image.EndInit();
            }
            return image;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class CategoryConvertImageRelativePathToAbsolutePath : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var imagePath = $"{Global.getBaseFolder()}Images\\category\\{value.ToString()}";

            BitmapImage image = null;
            if (File.Exists(imagePath))
            {
                image = new BitmapImage();
                image.BeginInit();
                image.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
                //image.CacheOption = BitmapCacheOption.None;
                //image.UriCachePolicy = new RequestCachePolicy(RequestCacheLevel.BypassCache);
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.UriSource = new Uri(imagePath);
                image.EndInit();
            }
            return image;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
