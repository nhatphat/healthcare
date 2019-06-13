using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Home.models
{
    public class StatisticalByDate : INotifyPropertyChanged
    {
        private DateTime date;
        private int quantity;
        private string name;
        

        public DateTime Date
        {
            get => date;
            set
            {
                date = value;
                setName();
                notifyPropertyChanged("Date");
            }
        }

        public int Quantity
        {
            get => quantity;
            set
            {
                quantity = value;
                notifyPropertyChanged("Quantity");
            }
        }

        public string Name { get => name; }

        private void setName()
        {
            this.name = $"{date.Day}/{date.Month}/{date.Year}";
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void notifyPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
