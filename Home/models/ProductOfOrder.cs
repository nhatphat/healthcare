using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace Home.models
{
    public class ProductOfOrder : INotifyPropertyChanged
    {
        private int id;
        private string name;
        private int price;
        private int quantity;
        private int total;

        public int ID { get => id; set { id = value; } }
        public string Name { get => name; set { name = value; } }
        public int Price { get => price; set { price = value; } }
        public int Quantity { get => quantity;
            set {
                quantity = value;
                setTotal();
                notifyPropertyChanged("Quantity");
            }
        }
        //total auto update, don't need set
        public int Total { get => total;}

        private void setTotal()
        {
            this.total = this.price * this.quantity;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void notifyPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
