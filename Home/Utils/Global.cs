using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Cache;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Home.Utils
{
    public class Global
    {
        /// <summary>
        /// lấy ra thư mục gốc chứa project
        /// </summary>
        /// <returns></returns>
        public static string getBaseFolder()
        {
            String current_folder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..");
            string base_folder = Directory.GetParent(current_folder).ToString();

            return base_folder;
        }


        /// <summary>
        /// hỗ trợ chọn file
        /// </summary>
        /// <returns>
        ///     file nếu thành công
        ///     
        /// lưu ý: 
        ///     nếu muốn lấy ra path của file vừa chọn
        ///         +> file.FileName
        ///     nếu muốn chỉ lấy tên file vừa chọn
        ///         +> file.SafeFileName
        /// </returns>
        public static OpenFileDialog getFile()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "jpg files(*.jpg)|*.jpg|PNG files(*.png)|*.png|ico files(*.ico)|*.ico";
            openFileDialog.InitialDirectory = @"C:\";
            openFileDialog.Title = "Choose file";
            openFileDialog.CheckFileExists = true;
            openFileDialog.CheckPathExists = true;
            openFileDialog.Multiselect = false;

            if (openFileDialog.ShowDialog() == true)
            {
                return openFileDialog;
            }

            return null;
        }

        /// <summary>
        /// Xóa file
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static bool deleteFile(string filePath)
        {
            try
            {
                File.Delete(filePath);
                return true;
            }
            catch { }

            return false;
        }

        public static BitmapImage loadBitmapImageFrom(string path)
        {
            try
            {
                BitmapImage image = new BitmapImage();
                image.BeginInit();
                image.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.UriSource = new Uri(path);
                image.EndInit();
                return image;
            }
            catch { }

            return null;
        }

        public static BitmapImage getImage()
        {
            OpenFileDialog fileDialog = new OpenFileDialog();

            fileDialog.Filter = "ICO files(*.ico)|*.ico|PNG files(*.png)|*.png|JPG files(*.jpg)|*.jpg";

            if (fileDialog.ShowDialog() == true)
            {
                var imagePath = fileDialog.FileName;

                BitmapImage imageBitmap = new BitmapImage();
                imageBitmap.BeginInit();
                imageBitmap.CacheOption = BitmapCacheOption.None;
                imageBitmap.UriCachePolicy = new RequestCachePolicy(RequestCacheLevel.BypassCache);
                imageBitmap.CacheOption = BitmapCacheOption.OnLoad;
                imageBitmap.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
                imageBitmap.UriSource = new Uri(imagePath, UriKind.RelativeOrAbsolute);
                imageBitmap.EndInit();
                return imageBitmap;
            }
            return null;
        }

        /// <summary>
        /// copy file đến thư mục khác
        /// </summary>
        /// <param name="filePath">path của file muốn copy </param>
        /// <param name="desFolder"> path của folde muốn copy tới
        /// lưu ý: kèm theo cả tên muốn lưu sau khi copy
        /// ví dụ:
        /// 
        ///     var fileCopy = "C:\icon.png"
        ///     var newFileName_sau_khi_copy = "ten_moi.png"
        ///     var desFolder = "D:\image\" + newFileName_sau_khi_copy
        ///     
        ///     copyFileTo(fileCopy, desFolder)
        ///     
        /// </param>
        public static string copyFileTo(string filePath, string desFolder)
        {
            try
            {
                File.Copy(filePath, desFolder, true);
            }
            catch (Exception ex)
            {

                return ex.ToString();
            }
            return "done";
        }

        /// <summary>
        /// Lấy đuôi file của hình ảnh upload lên
        /// </summary>
        /// <param name="path">path của hình ảnh muốn lấy đuôi file</param>
        /// <returns>đuôi file(ex: .ico|.jpg|.png)</returns>
        public static string getExtensionOfFile(string path)
        {
            string[] pathSplit = path.Split('/');
            string[] fullname = pathSplit[pathSplit.Length - 1].Split('.');
            string extension = "." + fullname[fullname.Length - 1];
            return extension;
        }


        /// <summary>
        /// Bỏ dấu tiếng việt
        /// </summary>
        /// <param name="text"> Chuỗi Cần Bỏ Dấu</param>
        /// <returns>chuoi can bo dau</returns>
        public static string NonUnicode(string text)
        {
            string[] arr1 = new string[] { "á", "à", "ả", "ã", "ạ", "â", "ấ", "ầ", "ẩ", "ẫ", "ậ", "ă", "ắ", "ằ", "ẳ", "ẵ", "ặ",
            "đ",
            "é","è","ẻ","ẽ","ẹ","ê","ế","ề","ể","ễ","ệ",
            "í","ì","ỉ","ĩ","ị",
            "ó","ò","ỏ","õ","ọ","ô","ố","ồ","ổ","ỗ","ộ","ơ","ớ","ờ","ở","ỡ","ợ",
            "ú","ù","ủ","ũ","ụ","ư","ứ","ừ","ử","ữ","ự",
            "ý","ỳ","ỷ","ỹ","ỵ",};
            string[] arr2 = new string[] { "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a",
            "d",
            "e","e","e","e","e","e","e","e","e","e","e",
            "i","i","i","i","i",
            "o","o","o","o","o","o","o","o","o","o","o","o","o","o","o","o","o",
            "u","u","u","u","u","u","u","u","u","u","u",
            "y","y","y","y","y",};

            text = text.ToLower();
            for (int i = 0; i < arr1.Length; i++)
            {
                text = text.Replace(arr1[i], arr2[i]);
            }
            return text;
        }


        /// <summary>
        /// Tạo tên file ảnh từ tên của category hoặc cosmetic
        /// </summary>
        /// <param name="ownerName">tên của caregory hoặc cosmetic</param>
        /// <returns>ten-file-co-dang-nhu-the-nay</returns>
        public static string makeFileNameBy(string ownerName)
        {
            string nonUnicode = NonUnicode(ownerName);
            string standard = nonUnicode.Trim();
            while (standard.Contains("  "))
            {
                standard = standard.Replace("  ", " ");

            }
            string finalName = standard.Replace(' ', '-');
            return finalName;
        }


        /// <summary>
        /// Check xem có phải người dùng sử dụng icon hay ảnh mặc định | cũ đối với sửa danh mục hoặc sản phẩm không
        /// </summary>
        /// <param name="sourceFile"></param>
        /// <param name="oldFileName"></param>
        /// <returns>
        /// True = default | old
        /// False = another image
        /// </returns>
        public static bool isUsingtheOldFile(string sourceFile, string oldFileName)
        {
            var root = "file:///" + $"{getBaseFolder().Replace("\\", "/")}";
            if (sourceFile == "pack://application:,,,/Images/category/" + oldFileName || sourceFile == "pack://application:,,,/Images/cosmetic/" + oldFileName 
                || sourceFile == $"{root}/Images/category/" + oldFileName || sourceFile == $"{root}/Images/cosmetic/" + oldFileName)
            {
                return true;
            }
            return false;
        }
                    
    }

}
