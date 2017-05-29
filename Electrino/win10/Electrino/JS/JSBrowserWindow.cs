using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChakraHost.Hosting;
using System.Diagnostics;

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

            JSBrowserWindowInstance instance = new JSBrowserWindowInstance();
            return instance.GetModule();
        }
    }

    class JSBrowserWindowInstance : AbstractJSModule
    {
        private Dictionary<string, List<Tuple<JavaScriptValue, JavaScriptValue>>> listeners = new Dictionary<string, List<Tuple<JavaScriptValue, JavaScriptValue>>>();

        public JSBrowserWindowInstance() : base("BrowserWindowInstance")
        {
            AttachMethod(LoadURL, "loadURL");
            AttachMethod(On, "on");
            App.NewWindow();
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
