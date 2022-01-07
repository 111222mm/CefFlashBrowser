﻿namespace CefFlashBrowser.Models
{
    public class Settings
    {
        public bool FirstStart { get; set; }
        public string Language { get; set; }
        public SearchEngine.Engine SearchEngine { get; set; }
        public NavigationType.Type NavigationType { get; set; }
        public WindowSizeInfo BrowserWindowSizeInfo { get; set; }
        public WindowSizeInfo SwfWindowSizeInfo { get; set; }



        public static Settings Default => new Settings
        {
            FirstStart = true,
            Language = "zh-CN",
            SearchEngine = Models.SearchEngine.Engine.Baidu,
            NavigationType = Models.NavigationType.Type.Automatic,
            BrowserWindowSizeInfo = null,
            SwfWindowSizeInfo = null
        };
    }
}