using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Home.models
{
    public class StatisticalProductsContributeByDate : INotifyPropertyChanged
    {
        private string name;
        private float contribute;
        
        public string Name
        {
            get => name;
            set
            {
                name = value;
                notifyPropertyChanged("Name");
            }
        }

        public float Contribute
        {
            get => contribute;
            set
            {
                contribute = value;
                notifyPropertyChanged("Contribute");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void notifyPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
