using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChakraHost.Hosting;
using System.Diagnostics;

namespace Electrino.JS
{
    class JSApp : AbstractJSModule
    {
        private Dictionary<string, List<Tuple<JavaScriptValue, JavaScriptValue>>> listeners = new Dictionary<string, List<Tuple<JavaScriptValue, JavaScriptValue>>>();
        private static JSApp instance;
        public JSApp() : base("app")
        {
            instance = this;
            AttachMethod(On, "on");
            AttachMethod(Quit, "quit");
        }

        public static JSApp GetInstance()
        {
            return instance;
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

        public void Call(string key)
        {
            List<Tuple<JavaScriptValue, JavaScriptValue>> eventListeners;
            listeners.TryGetValue(key, out eventListeners);
            if (eventListeners != null)
            {
                foreach (Tuple<JavaScriptValue, JavaScriptValue> listener in eventListeners)
                {
                    listener.Item1.CallFunction(new JavaScriptValue[] { listener.Item2 });
                }
            }
        }

        private JavaScriptValue Quit(JavaScriptValue callee, bool isConstructCall, JavaScriptValue[] arguments, ushort argumentCount, IntPtr callbackData)
        {
            Windows.UI.Xaml.Application.Current.Exit();
            return JavaScriptValue.Undefined;
        }
    }
}
