﻿using AppBase.Helpers;
using AppBase.ViewModels;
using AppBaseNamespace.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppBaseNamespace
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ResourceFormatSettingsPage : ContentPage
    {       
        public ResourceFormatSettingsPage(App app, MainPageViewModel mainPageViewModel)
        {
            InitializeComponent();            
            
            List<ResourceFormatSettingsItem> switches = new List<ResourceFormatSettingsItem>();
            
            switches.Add(new ResourceFormatSettingsItem(wifiSwitch, "wifi"));
            switches.Add(new ResourceFormatSettingsItem(pdfSwitch, pdfLabel.Text));
            switches.Add(new ResourceFormatSettingsItem(htmlSwitch, htmlLabel.Text));
            switches.Add(new ResourceFormatSettingsItem(odtSwitch, odtLabel.Text));

            BindingContext = new ResourceFormatSettingsPageViewModel(app, mainPageViewModel, app.availableLanguages, switches);

            if (app.userSettings.DownloadOnlyWithWifi) wifiSwitch.IsToggled = true;
            foreach (var item in app.userSettings.Formats)
            {
                if (item == pdfLabel.Text) pdfSwitch.IsToggled = true;
                if (item == odtLabel.Text) odtSwitch.IsToggled = true;
                if (item == htmlLabel.Text) htmlSwitch.IsToggled = true;
            }
                       
        }
        void OnCheckBoxCheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            (BindingContext as ResourceFormatSettingsPageViewModel).OnCheckBoxCheckedChanged(sender, e);
        }

        void OnToggled(object sender, ToggledEventArgs e)
        {
            (BindingContext as ResourceFormatSettingsPageViewModel).OnToggled(sender, e);
        }

        public void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            (BindingContext as ResourceFormatSettingsPageViewModel).TapGestureRecognizer_Tapped(sender, e);
        }

        private void RequestUpdateButton_Clicked(object sender, EventArgs e)
        {
            (BindingContext as ResourceFormatSettingsPageViewModel).RequestUpdate(this);
        }
    }
}