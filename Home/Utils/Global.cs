using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public static void copyFileTo(string filePath, string desFolder)
        {
            File.Copy(filePath, desFolder);
        }
    }
}
