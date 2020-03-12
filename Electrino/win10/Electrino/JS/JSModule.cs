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
        private bool asFunction;

        public static void AttachModule(JavaScriptValue module, AbstractJSModule subModule)
        {
            if (Native.JsSetProperty(module, JavaScriptPropertyId.FromString(subModule.GetId()),
                subModule.GetModule(), false) != JavaScriptErrorCode.NoError)
            {
                throw new Exception("Failed to attach module");
            }
        }

        public static void AttachModule(AbstractJSModule module, AbstractJSModule subModule)
        {
            AttachModule(module.GetModule(), subModule);
        }


        public static void AttachMethod(JavaScriptValue module, JavaScriptNativeFunction method, string id)
        {
            JavaScriptValue requireToString;
            if (Native.JsCreateFunction(method, IntPtr.Zero, out requireToString) != JavaScriptErrorCode.NoError)
            {
                throw new Exception("Failed to create method");
            }
            if (Native.JsSetProperty(module, JavaScriptPropertyId.FromString(id), requireToString, false)
                != JavaScriptErrorCode.NoError)
            {
                throw new Exception("Failed to define tostring on require");
            }
        }

        public static void AttachMethod(AbstractJSModule module, JavaScriptNativeFunction method, string id)
        {
            AttachMethod(module.GetModule(), method, id);
        }


        public static void AttachProperty(JavaScriptValue module, JavaScriptValue property, string id)
        {
            if (Native.JsSetProperty(module, JavaScriptPropertyId.FromString(id),
                property, false) != JavaScriptErrorCode.NoError)
            {
                throw new Exception("Failed to attach property");
            }
        }

        public static void AttachProperty(AbstractJSModule module, JavaScriptValue method, string id)
        {
            AttachProperty(module.GetModule(), method, id);
        }

        public static string JSValToString(JavaScriptValue val)
        {
            val = val.ConvertToString();
            IntPtr returnValue = IntPtr.Zero;
            UIntPtr stringLength;
            if (Native.JsStringToPointer(val, out returnValue, out stringLength) != JavaScriptErrorCode.NoError)
            {
                throw new Exception("Failed to convert return value.");
            }
            return Marshal.PtrToStringUni(returnValue);
        }

        /// <summary>
        /// Construct a module either as an Object or function
        /// If function is used then the class must override the Main method
        /// </summary>
        /// <param name="id"></param>
        /// <param name="asFunction"></param>
        public AbstractJSModule(string id, bool asFunction = false)
        {
            this.id = id;
            this.asFunction = asFunction;

            if (asFunction)
            {
                if (Native.JsCreateFunction(Main, IntPtr.Zero, out module) != JavaScriptErrorCode.NoError)
                {
                    throw new Exception("Failed to create function");
                }
            }
            else
            {
                if (Native.JsCreateObject(out module) != JavaScriptErrorCode.NoError)
                {
                    throw new Exception("Failed to create module");
                }
            }

            AttachMethod(ToString, "toString");
        }

        public void AttachModule(AbstractJSModule subModule)
        {
            AttachModule(this, subModule);
        }

        public void AttachMethod(JavaScriptNativeFunction method, string id)
        {
            AttachMethod(this, method, id);
        }

        public void AttachProperty(JavaScriptValue property, string id)
        {
            AttachProperty(this, property, id);
        }

        public JavaScriptValue GetModule()
        {
            return module;
        }

        public string GetId()
        {
            return id;
        }

        protected JavaScriptValue ToString(JavaScriptValue callee, bool isConstructCall, JavaScriptValue[] arguments, ushort argumentCount, IntPtr callbackData)
        {
            // TODO: Track members and list recursively
            return JavaScriptValue.FromString("[" + (asFunction ? "Function" : "Module") + ": " + id + "]");
        }

        protected virtual JavaScriptValue Main(JavaScriptValue callee, bool isConstructCall, JavaScriptValue[] arguments, ushort argumentCount, IntPtr callbackData)
        {
            return JavaScriptValue.Undefined;
        }
    }
}
