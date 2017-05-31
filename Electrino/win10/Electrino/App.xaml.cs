using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using System.Diagnostics;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.ViewManagement;

namespace Electrino
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    sealed partial class App : Application
    {
        private JavaScriptApp jsApp = new JavaScriptApp();
        private static App instance;
        private LaunchActivatedEventArgs launchArgs;

        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            instance = this;
            this.InitializeComponent();
            this.Suspending += OnSuspending;
        }

        public static void Log(string msg)
        {
#if DEBUG
            Debug.WriteLine(msg);
#endif
        }

        private async void Run()
        {
            string js = await ReadJS("main.js");
            Log(jsApp.Init());
            Log(jsApp.RunScript(js));
            Ready();
        }

        private async Task<string> ReadJS(string filename)
        {
            StorageFile file = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///test-app/" + filename));
            string js;
            using (StreamReader sRead = new StreamReader(await file.OpenStreamForReadAsync()))
                js = await sRead.ReadToEndAsync();
            return js;
        }

        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used such as when the application is launched to open a specific file.
        /// </summary>
        /// <param name="e">Details about the launch request and process.</param>
        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {
            launchArgs = e;
            Run();
        }

        private void Ready() { 
            if (JS.JSApp.GetInstance() != null)
            {
                JS.JSApp.GetInstance().Call("ready");
            }
        }

        private void Suspended()
        {
            if (JS.JSApp.GetInstance() != null)
            {
                JS.JSApp.GetInstance().Call("window-all-closed");
            }
        }

        public static void NewWindow(int width, int height)
        {
            if (instance == null)
            {
                Log("App no ready yet");
                return;
            }
            App.instance._NewWindow(width, height);
        }

        private void _NewWindow(int width, int height)
        {
            ApplicationView.PreferredLaunchViewSize = new Size(width, height);
            ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.PreferredLaunchViewSize;            

            Frame rootFrame = Window.Current.Content as Frame;

            // Do not repeat app initialization when the Window already has content,
            // just ensure that the window is active
            if (rootFrame == null)
            {
                // Create a Frame to act as the navigation context and navigate to the first page
                rootFrame = new Frame();

                rootFrame.NavigationFailed += OnNavigationFailed;
                
                // Place the frame in the current Window
                Window.Current.Content = rootFrame;
            }

            if (rootFrame.Content == null)
            {
                // When the navigation stack isn't restored navigate to the first page,
                // configuring the new page by passing required information as a navigation
                // parameter
                rootFrame.Navigate(typeof(MainPage), launchArgs.Arguments);
            }
            // Ensure the current window is active
            Window.Current.Activate();
        }

        /// <summary>
        /// Invoked when Navigation to a certain page fails
        /// </summary>
        /// <param name="sender">The Frame which failed navigation</param>
        /// <param name="e">Details about the navigation failure</param>
        void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        /// <summary>
        /// Invoked when application execution is being suspended.  Application state is saved
        /// without knowing whether the application will be terminated or resumed with the contents
        /// of memory still intact.
        /// </summary>
        /// <param name="sender">The source of the suspend request.</param>
        /// <param name="e">Details about the suspend request.</param>
        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            //TODO: Save application state and stop any background activity
            deferral.Complete();
            Suspended();
        }
    }
}
