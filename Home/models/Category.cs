//------------------------------------------------------------------------------
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

    public partial class Category : INotifyPropertyChanged
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Category()
        {
            this.Cosmetics = new HashSet<Cosmetic>();
        }

        public static char COL_ICON = 'I';

        private int id;
        private string name;
        private string icon = "default-category-icon.ico";
        private int status = 0;
        public int ID
        {
            get => id;
            set
            {
                id = value;
                notifyPropertyChanged("ID");
            }
        }

        public string Name
        {
            get => name;
            set
            {
                name = value;
                notifyPropertyChanged("Name");
            }
        }

        public string Icon
        {
            get => icon;
            set
            {
                icon = value;
                notifyPropertyChanged("Icon");
            }
        }

        public int Status
        {
            get => status;
            set
            {
                status = value;
                notifyPropertyChanged("Status");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void notifyPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Cosmetic> Cosmetics { get; set; }

    }
}
