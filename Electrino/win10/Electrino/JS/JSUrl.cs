using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChakraHost.Hosting;

namespace Electrino.JS
{
    class JSUrl : AbstractJSModule
    {
        public JSUrl() : base("url")
        {
            AttachMethod(Format, "format");
        }

        private static JavaScriptValue Format(JavaScriptValue callee, bool isConstructCall, JavaScriptValue[] arguments, ushort argumentCount, IntPtr callbackData)
        {
            JavaScriptValue obj = arguments[1];
            string pathName = JSValToString(obj.GetProperty(JavaScriptPropertyId.FromString("pathname"))).Replace("\\", "/");
            string protocol = JSValToString(obj.GetProperty(JavaScriptPropertyId.FromString("protocol")));
            return JavaScriptValue.FromString(protocol + "//" + pathName);
        }
    }
}
