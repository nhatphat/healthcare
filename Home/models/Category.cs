using System.ComponentModel;

namespace Home.models
{
    public class Category : INotifyPropertyChanged
    {
        private string name;
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

        public event PropertyChangedEventHandler PropertyChanged;

        private void notifyPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
