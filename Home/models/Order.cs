﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Home.models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;

    public partial class Order : INotifyPropertyChanged
    {
        public const int C_NEW = 0;
        public const int C_COMPLETED = 1;
        public const int C_CANCELED = 2;
        public const int C_DELETED = 3;

        public const string N_NEW = "Mới tạo";
        public const string N_COMPLETED = "Hoàn tất";
        public const string N_CANCELED = "Đã hủy";

        public static int getStatusCodeByName(string name)
        {
            switch (name)
            {
                case N_NEW: return C_NEW;
                case N_COMPLETED: return C_COMPLETED;
                case N_CANCELED: return C_CANCELED;
                default: return C_NEW;
            }
        }

        public static string getStatusNameByCode(int code)
        {
            switch (code)
            {
                case C_NEW: return N_NEW;
                case C_COMPLETED: return N_COMPLETED;
                case C_CANCELED: return N_CANCELED;
                default: return N_NEW;
            }
        }

        private int status;
        private int id;
        private string products;
        private int totalPrice;
        private DateTime createAt = DateTime.Now;
        private string customerTel;
        private string customerName;
        private string customerAddr;

        //parse detail product from products json
        private List<ProductOfOrder> listProducts = null;

        public int Status { get => status;
            set
            {
                status = value;
                notifyPropertyChanged("Status");
            }
        }
        public int ID { get => id; set { id = value; } }
        public string Products { get => products;
            set
            {
                products = value;
                notifyPropertyChanged("Products");
            }
        }
        public int TotalPrice { get => totalPrice;
            set
            {
                totalPrice = value;
                notifyPropertyChanged("TotalPrice");
            }
        }
        public DateTime CreateAt
        {
            get => createAt;
            set
            {
                createAt = value;
            }
        }
        public string CustomerTel { get => customerTel;
            set
            {
                customerTel = value;
                notifyPropertyChanged("CustomerTel");
            }
        }
        public string CustomerName { get => customerName;
            set
            {
                customerName = value;
                notifyPropertyChanged("CustomerName");
            }
        }
        public string CustomerAddr { get => customerAddr;
            set
            {
                customerAddr = value;
                notifyPropertyChanged("CustomerAddr");
            }
        }
        public List<ProductOfOrder> ListProducts{ get => listProducts;
            set
            {
                listProducts = value;
                notifyPropertyChanged("ListProducts");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void notifyPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
