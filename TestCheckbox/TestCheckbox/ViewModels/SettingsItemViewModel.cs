﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using Xamarin.Forms;

namespace TestCheckbox.ViewModels
{
    public class SettingsItemViewModel: INotifyPropertyChanged
    {
        private bool isCheck;
        public bool IsChecked //{ get; set; }
        {
            get
            {
                return isCheck;
            }
            set
            {
                if (value != this.isCheck)
                {
                    this.isCheck = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public string Value { get; set; }
        public Command CheckedChangedCommand { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        
        public void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
