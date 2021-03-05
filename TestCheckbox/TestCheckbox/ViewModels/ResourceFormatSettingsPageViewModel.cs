﻿using System.Collections.Generic;
using AppBaseNamespace.ViewModels;
using System.Linq;
using Xamarin.Forms;
using AppBase.ViewModels;

namespace AppBaseNamespace
{
    internal class ResourceFormatSettingsPageViewModel
    {
        public List<ResourceFormatSettingsItemViewModel> Switches { get; }
        public List<ItemViewModel> Languages { get; set; }
        App app;
        MainPageViewModel mainPageViewModel;
        public ResourceFormatSettingsPageViewModel(App app, MainPageViewModel mainPageViewModel, List<string> languages ,List<ResourceFormatSettingsItemViewModel> switches)
        {
            this.app = app;
            this.mainPageViewModel = mainPageViewModel;
            
            Languages = languages
                .Where(x => !string.IsNullOrEmpty(x))
                .Select(x => new ItemViewModel()
                {
                    IsChecked = app.userSettings.ChosenResourceLanguages.Contains(x),
                    Value = x,
                    CheckedChangedCommand = new Command(() => {                        
                    })
                })
                .ToList();
            Switches = switches;            
        }

        public void OnToggled(object sender, ToggledEventArgs e)
        {            
            foreach (var item in Switches)
            {                
                if (item.CorrespondingSwitch == (sender as Switch))
                {
                    if (item.Name == "wifi")
                    {
                        HandleWifiChange((sender as Switch).IsToggled);                        
                    }
                    else
                    {
                        HandleFormatChange(item.Name, (sender as Switch).IsToggled);                        
                    }
                    
                }
            }
        }

        void HandleWifiChange(bool isToggled)
        {
            app.userSettings.DownloadOnlyWithWifi = isToggled;
            app.SaveUserSettings();
        }
        void HandleFormatChange(string name, bool isToggled)
        {
            if (isToggled)
            {
                if (!app.userSettings.Formats.Contains(name))
                {
                    app.userSettings.Formats.Add(name);
                }
            }
            else
            {
                if (app.userSettings.Formats.Contains(name))
                {
                    app.userSettings.Formats.Remove(name);
                }
            }
            app.SaveUserSettings();
        }

        public void OnCheckBoxCheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            foreach (var item in Languages)
            {
                if (item.Value == ((sender as CheckBox).BindingContext as ItemViewModel).Value)
                {
                    if ((sender as CheckBox).IsChecked && !app.userSettings.ChosenResourceLanguages.Contains(item.Value))
                    {
                        app.userSettings.ChosenResourceLanguages.Add(item.Value);
                        app.SaveUserSettings();
                        break;
                    }
                    else if (!(sender as CheckBox).IsChecked && app.userSettings.ChosenResourceLanguages.Contains(item.Value))
                    {
                        app.userSettings.ChosenResourceLanguages.Remove(item.Value);
                        app.SaveUserSettings();
                        break;
                    }
                }
            }
        }
    }
}