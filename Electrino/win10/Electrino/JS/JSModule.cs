using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChakraHost.Hosting;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Electrino.JS
{
    abstract class AbstractJSModule
    {
        private JavaScriptValue module;
        private string id;

        public static void AttachModule(JavaScriptValue module, AbstractJSModule subModule)
        {
            Debug.Assert(Native.JsSetProperty(module, JavaScriptPropertyId.FromString(subModule.GetId()),
                subModule.GetModule(), false) == JavaScriptErrorCode.NoError, "Failed to attach module");
        }

        public static void AttachModule(AbstractJSModule module, AbstractJSModule subModule)
        {
            AttachModule(module.GetModule(), subModule);
        }

        private static void AttachMethod(AbstractJSModule module, JavaScriptNativeFunction method, string id)
        {
            JavaScriptValue requireToString;
            Debug.Assert(Native.JsCreateFunction(method, IntPtr.Zero, out requireToString) == JavaScriptErrorCode.NoError, "Failed to create method");

            Debug.Assert(Native.JsSetProperty(module.GetModule(), JavaScriptPropertyId.FromString(id), requireToString, false) 
                == JavaScriptErrorCode.NoError, "Failed to define tostring on require");
        }

        public static string JSValToString(JavaScriptValue val)
        {
            val = val.ConvertToString();
            IntPtr returnValue;
            UIntPtr stringLength;
            Debug.Assert(Native.JsStringToPointer(val, out returnValue, out stringLength) == JavaScriptErrorCode.NoError, "Failed to convert return value.");
            return Marshal.PtrToStringUni(returnValue);
        }

        public AbstractJSModule(string id)
        {
            this.id = id;

            Debug.Assert(Native.JsCreateObject(out module) == JavaScriptErrorCode.NoError, "Failed to create module");
        }

        public void AttachModule(AbstractJSModule subModule)
        {
            AttachModule(this, subModule);
        }

        public void AttachMethod(JavaScriptNativeFunction method, string id)
        {
            AttachMethod(this, method, id);
        }

        public JavaScriptValue GetModule()
        {
            return module;
        }

        public string GetId()
        {
            return id;
        }
    }
}
