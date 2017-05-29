using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using ChakraHost.Hosting;

namespace Electrino.JS
{
    class JSConsole : AbstractJSModule
    {
        public JSConsole() : base("console")
        {
            AttachMethod(Log, "log");
        }

        private static JavaScriptValue Log(JavaScriptValue callee, bool isConstructCall, JavaScriptValue[] arguments, ushort argumentCount, IntPtr callbackData)
        {
            string[] args = new string[arguments.Length - 1];
            for (int i = 1; i < arguments.Length; i++)
            {
                args[i - 1] = JSValToString(arguments[i]);
            }
            App.Log(String.Join(", ", args));
            return JavaScriptValue.Undefined;
        }
    }
}
