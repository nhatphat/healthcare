using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Home.models
{
    public class StaticalByMonth : INotifyPropertyChanged
    {
        private int month;
        private int year;
        private int revenue;
        private string name;

        public int Month { get => month;
            set
            {
                month = value;
                notifyPropertyChanged("Month");
            }
        }

        public int Year
        {
            get => year;
            set
            {
                year = value;
                setName();
                notifyPropertyChanged("Year");
            }
        }

        public int Revenue
        {
            get => revenue;
            set
            {
                revenue = value;
                notifyPropertyChanged("Revenue");
            }
        }

        public string Name { get => name; }

        private void setName()
        {
            this.name = $"{month}/{year}";
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void notifyPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
