using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
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
        public MainPage()
        {
            this.InitializeComponent();
            webView1.ScriptNotify += ScriptNotify;

            webView1.Navigate(new Uri("ms-appx-web:///test-app/index.html"));
        }

        private void webView_NavigationStarting(WebView sender, WebViewNavigationStartingEventArgs args)
        {
            AddRenderApis();
        }

        void ScriptNotify(object sender, NotifyEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine($"Event received from {e.CallingUri}: \"{e.Value}\"");
        }

        private void AddRenderApis()
        {
            webView1.AddWebAllowedObject("process", new RenderAPI.JSProcess());
        }
    }
}
