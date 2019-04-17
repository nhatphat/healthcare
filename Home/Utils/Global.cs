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
    }
}
