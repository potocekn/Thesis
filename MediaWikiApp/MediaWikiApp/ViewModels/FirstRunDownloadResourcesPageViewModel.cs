﻿using AppBase.Helpers;
using AppBase.Resources;
using AppBaseNamespace;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace AppBase.ViewModels
{
    /// <summary>
    /// ViewModel for the very first downloading of resources based on the first user selections.
    /// </summary>
    class FirstRunDownloadResourcesPageViewModel
    {  
        public FirstRunDownloadResourcesPageViewModel()
        {
                        
        }

        /// <summary>
        /// Method for downloading the resources.
        /// </summary>
        /// <param name="app">Reference to the current app.</param>
        /// <param name="page">reference to the doenload page (the page from which the method is called)
        /// - needed for displaying messages</param>
        /// <returns>Boolean if the download was successful.</returns>
        public async Task<bool> Download(App app, FirstRunDownloadResourcesPage page)
        {
            if (!UpdateSyncHelpers.CanDownload(app))
            {
                await ShowPopupHelpers.ShowOKPopup(page,
                    AppResources.ResourcesDownloadedTitle_Text, AppResources.ResourcesDownloadedUnsuccessful_Text, 300, 250);
                return false;
            }
            else
            {
                await ShowPopupHelpers.ShowOKPopup(page, 
                    AppResources.ResourcesDownloadStartTitle_Text, AppResources.DownloadingSelectedResourcesLabel_Text, 300, 200);
                await UpdateSyncHelpers.DownloadResources(app);
                UpdateApp(app);
                return true;
            }            
        }

        /// <summary>
        /// Method for updating the application. The method sets the proper language and sets the user settings flags
        /// which indicate if the first run initialization is finished.
        /// </summary>
        /// <param name="app">The referrence to the current app.</param>
        void UpdateApp(App app)
        {
            app.userSettings.WasFirstDownload = true;
            string shortcut = LanguagesTranslationHelper.GetLanguageShortcut(app.userSettings.AppLanguage);
            CultureInfo language = new CultureInfo(shortcut);
            Thread.CurrentThread.CurrentUICulture = language;
            CultureInfo.CurrentUICulture = language;
            AppResources.Culture = language;
            app.ReloadApp(shortcut, app.userSettings.AppLanguage);
        }        
    }
}
