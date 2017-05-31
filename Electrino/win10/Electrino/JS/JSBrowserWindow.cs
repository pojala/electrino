using ChakraHost.Hosting;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace Electrino.JS
{
    class JSBrowserWindow : AbstractJSModule
    {
        public JSBrowserWindow() : base("BrowserWindow", true)
        {

        }

        protected override JavaScriptValue Main(JavaScriptValue callee, bool isConstructCall, JavaScriptValue[] arguments, ushort argumentCount, IntPtr callbackData)
        {
            if (!isConstructCall)
            {
                return JavaScriptValue.CreateTypeError(JavaScriptValue.FromString("BrowserWindow must be constructed"));
            }

            JToken options;
            try
            {
                options = JavaScriptValueToJTokenConverter.Convert(arguments[1]);
            }
            catch (Exception)
            {
                // If no object is passed to the BrowserWindow constructor we'll provide a default one
                options = JObject.Parse("{ width: 800, height: 600 }");
            }

            JSBrowserWindowInstance instance = new JSBrowserWindowInstance(options);
            return instance.GetModule();
        }
    }

    class JSBrowserWindowInstance : AbstractJSModule
    {
        private JToken options;
        // I don't really like putting this here, I think we should probably have a class that contains all our constants
        private const int defaultWindowWidth = 800, defaultWindowHeight = 600;
        private Dictionary<string, List<Tuple<JavaScriptValue, JavaScriptValue>>> listeners = new Dictionary<string, List<Tuple<JavaScriptValue, JavaScriptValue>>>();

        public JSBrowserWindowInstance(JToken options) : base("BrowserWindowInstance")
        {
            this.options = options;

            AttachMethod(LoadURL, "loadURL");
            AttachMethod(On, "on");

            if (this.options != null)
            {
                int width = 0, height = 0;

                Int32.TryParse(options["width"].ToString(), out width);
                Int32.TryParse(options["height"].ToString(), out height);

                if (width <= 0) width = defaultWindowWidth;
                if (height <= 0) height = defaultWindowHeight;

                App.NewWindow(width, height);
            }
            else
            {
                App.NewWindow(defaultWindowWidth, defaultWindowHeight);
            }
        }
        protected JavaScriptValue LoadURL(JavaScriptValue callee, bool isConstructCall, JavaScriptValue[] arguments, ushort argumentCount, IntPtr callbackData)
        {
            string url = JSValToString(arguments[1]);
            if (!MainPage.LoadURL(url))
            {
                App.Log("Failed to load url " + url);
            }
            return JavaScriptValue.Undefined;
        }

        private JavaScriptValue On(JavaScriptValue callee, bool isConstructCall, JavaScriptValue[] arguments, ushort argumentCount, IntPtr callbackData)
        {
            string key = JSValToString(arguments[1]);
            List<Tuple<JavaScriptValue, JavaScriptValue>> eventListeners;
            if (listeners.ContainsKey(key))
            {
                listeners.TryGetValue(key, out eventListeners);
            }
            else
            {
                eventListeners = new List<Tuple<JavaScriptValue, JavaScriptValue>>();
                listeners.Add(key, eventListeners);
            }
            eventListeners.Add(Tuple.Create(arguments[2], arguments[0]));
            arguments[2].AddRef();
            return JavaScriptValue.Undefined;
        }
    }
}
