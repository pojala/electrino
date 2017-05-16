using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChakraHost.Hosting;

namespace Electrino.JS
{
    class JSPath : AbstractJSModule
    {
        public JSPath() : base("path")
        {
            AttachMethod(Join, "join");
        }

        private static JavaScriptValue Join(JavaScriptValue callee, bool isConstructCall, JavaScriptValue[] arguments, ushort argumentCount, IntPtr callbackData)
        {
            string[] args = new string[arguments.Length - 1];
            for (int i = 1; i < arguments.Length; i++)
            {
                args[i - 1] = JSValToString(arguments[i]);
            }
            return JavaScriptValue.FromString(String.Join("\\", args));
        }
    }
}
