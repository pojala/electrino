using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Electrino
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private static MainPage instance = null;
        public MainPage()
        {
            instance = this;
            InitializeComponent();
            webView1.ScriptNotify += ScriptNotify;
            webView1.ContainsFullScreenElementChanged += webView1_ContainsFullScreenElementChanged;

            //webView1.Navigate(new Uri("ms-appx-web:///test-app/index.html"));
        }

        public static bool LoadURL(string url)
        {
            if (instance == null)
            {
                return false;
            }
            instance.webView1.Navigate(new Uri(url));
            return true;
        }

        private void WebView_NavigationStarting(WebView sender, WebViewNavigationStartingEventArgs args)
        {
            AddRenderApis();
        }

        void ScriptNotify(object sender, NotifyEventArgs e)
        {
            App.Log($"Event received from {e.CallingUri}: \"{e.Value}\"");
        }

        private void AddRenderApis()
        {
            webView1.AddWebAllowedObject("process", new RenderAPI.JSProcess());
            webView1.AddWebAllowedObject("require", new RenderAPI.JSRequire().Main);
        }

        private void webView1_ContainsFullScreenElementChanged(WebView sender, object args)
        {
            var applicationView = ApplicationView.GetForCurrentView();

            if (sender.ContainsFullScreenElement)
            {
                applicationView.TryEnterFullScreenMode();
            }
            else if (applicationView.IsFullScreenMode)
            {
                applicationView.ExitFullScreenMode();
            }
        }
    }
}
