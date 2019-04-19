using System.ComponentModel;

namespace Home.models
{
    public class Cosmetic : INotifyPropertyChanged
    {
        public static char COL_STATUS = 'A';
        public static char COL_ID = 'B';
        public static char COL_NAME = 'C';
        public static char COL_IMAGE_URL = 'D';
        public static char COL_PRICE = 'E';
        public static char COL_ORIGIN = 'F';
        public static char COL_DETAIL = 'G';


        private string id;
        private string name;
        private string image_url = "default.jpg";
        private uint price;
        private string origin;
        private string detail;

        // location
        public int row_in_db { get; set; }

        public string ID { get => id;
            set
            {
                id = value;
                notifyPropertyChanged("ID");
            }
        }

        public string Name { get => name;
            set
            {
                name = value;
                notifyPropertyChanged("Name");
            }
        }

        public string Image_url { get => image_url;
            set
            {
                image_url = value;
                notifyPropertyChanged("Image_url");
            }
        }

        public uint Price { get => price;
            set
            {
                price = value;
                notifyPropertyChanged("Price");
            }
        }

        public string Origin { get => origin;
            set
            {
                origin = value;
                notifyPropertyChanged("origin");
            }
        }

        public string Detail { get => detail;
            set
            {
                detail = value;
                notifyPropertyChanged("Detail");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void notifyPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
