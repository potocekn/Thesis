﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using Xamarin.Forms;
using AppBase;
using AppBase.Models;
using AppBase.UserSettingsHelpers;
using System.Globalization;

namespace AppBaseNamespace.ViewModels
{
    /// <summary>
    /// Class representing the view model of the main page. 
    /// Class remembers its title and has commands that redirect main page to settings or resources.
    /// </summary>
    public class MainPageViewModel
    {        
        public string previouslyChecked = "";

        public List<ResourceLanguageInfo> ResourceLanguages { get; set; }
        public string Title { get; }
        public Command LoadCheckboxes { get; }
        public Command GoToSettings { get; }
        public Command GoToResources { get; }

        public MainPageViewModel(Page page, App app,INavigation navigation, string previouslyChecked)
        {
            this.previouslyChecked = previouslyChecked;
            LoadCheckboxes = new Command(() => {
                navigation.PushAsync(new ResourceFormatSettingsPage(app, this));
            });
            GoToSettings = new Command(() => {
                navigation.PushAsync(new SettingsPage(app, this));
            });
            GoToResources = new Command(() => {
                navigation.PushAsync(new ResourcesPage(app, navigation));
            });
            ResourceLanguages = SeparateLanguages(app, navigation);
        }

        List<ResourceLanguageInfo> SeparateLanguages(App app, INavigation navigation)
        {
            List<ResourceLanguageInfo> result = new List<ResourceLanguageInfo>();
            foreach (var language in app.userSettings.ChosenResourceLanguages)
            {
                ResourceLanguageInfo resourceLanguageInfo = new ResourceLanguageInfo();

                resourceLanguageInfo.LanguageName = language;
                resourceLanguageInfo.PDFs = SeparatePDFsForLanguage(language, app);
                resourceLanguageInfo.ODTs = SeparateODTsForLanguage(language, app);
                resourceLanguageInfo.HTMLs = SeparateHTMLsForLanguage(language);
                resourceLanguageInfo.OpenResources = new Command(() =>
                {
                    navigation.PushAsync(new ResourcesPage(resourceLanguageInfo, navigation));
                });
                
                result.Add(resourceLanguageInfo);
            }
            return result;
        }

        List<ResourcesInfoPDF> SeparatePDFsForLanguage(string language, App app)
        {
            List<ResourcesInfoPDF> result = new List<ResourcesInfoPDF>();

            foreach (var item in app.resourcesPDF)
            {
                if (item.Language == language)
                {
                    result.Add(item);
                }
            }

            return result;
        }

        List<ResourcesInfoPDF> SeparateODTsForLanguage(string language, App app)
        {
            List<ResourcesInfoPDF> result = new List<ResourcesInfoPDF>();

            foreach (var item in app.resourcesODT)
            {
                if (item.Language == language)
                {
                    result.Add(item);
                }
            }

            return result;
        }

        List<HtmlRecord> SeparateHTMLsForLanguage(string language)
        {
            var allPages = App.Database.GetPagesAsync().Result;
            List<HtmlRecord> result = allPages.FindAll(x => {
                CultureInfo ci = new CultureInfo(x.PageLanguage);
                return ci.DisplayName == language;
                });            
            return result;
        }
    }
}
