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
        public static string getBaseFolder()
        {
            String current_folder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..");
            string base_folder = Directory.GetParent(current_folder).ToString();

            return base_folder;
        }

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

        public static void copyFileTo(string filePath, string desFolder)
        {
            File.Copy(filePath, desFolder);
        }
    }
}
