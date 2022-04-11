﻿using CefFlashBrowser.Models.Data;
using CefFlashBrowser.Utils;
using CefFlashBrowser.Views.Dialogs.JsDialogs;
using CefSharp;
using CefSharp.Wpf;
using System;
using System.Diagnostics;
using System.IO;

namespace CefFlashBrowser.FlashBrowser
{
    public abstract class FlashBrowserBase : ChromiumWebBrowser
    {
        public static readonly string CachePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Caches\");
        public static readonly string FlashPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Assets\Plugins\pepflashplayer.dll");

        private static readonly string EmptyExePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Assets\CefFlashBrowser.EmptyExe.exe");

        protected override void OnIsBrowserInitializedChanged(bool oldValue, bool newValue)
        {
            base.OnIsBrowserInitializedChanged(oldValue, newValue);

            if (newValue)
            {
                Cef.UIThreadTaskFactory.StartNew(() =>
                {
                    var requestContext = GetBrowser().GetHost().RequestContext;
                    var flag = requestContext.SetPreference("profile.default_content_setting_values.plugins", 1, out string err);

                    if (!flag)
                    {
                        var title = LanguageManager.GetString("title_error");
                        Dispatcher.Invoke(() => JsAlertDialog.ShowDialog(err, title));
                    }
                });
            }
        }

        /// <summary>
        /// This method should be called when the program starts
        /// </summary>
        public static void InitCefFlash()
        {
            if (Cef.IsInitialized)
                return;

            Environment.SetEnvironmentVariable("ComSpec", EmptyExePath); //Remove black popup window

            CefSettings settings = new CefSettings()
            {
                Locale = GlobalData.Settings.Language,
                CachePath = CachePath
            };

#if !DEBUG
            settings.LogSeverity = LogSeverity.Disable;
#endif

            settings.CefCommandLineArgs["enable-system-flash"] = "1";
            settings.CefCommandLineArgs.Add("ppapi-flash-version", FileVersionInfo.GetVersionInfo(FlashPath).FileVersion.Replace(',', '.'));
            settings.CefCommandLineArgs.Add("ppapi-flash-path", FlashPath);
            Cef.Initialize(settings);
        }
    }
}
