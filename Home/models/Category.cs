using System.ComponentModel;

namespace Home.models
{
    public class Category : INotifyPropertyChanged
    {
        public static char COL_ICON = 'I';
            

        private string name;
        private string icon;
        private BindingList<Cosmetic> cosmetics;

        public string Name { get => name;
            set
            {
                name = value;
                notifyPropertyChanged("Name");
            }
        }

        public BindingList<Cosmetic> Cosmetics { get => cosmetics;
            set
            {
                cosmetics = value;
                notifyPropertyChanged("Cosmetics");
            }
        }

        public string Icon { get => icon;
            set
            {
                icon = value;
                notifyPropertyChanged("Icon");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void notifyPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
