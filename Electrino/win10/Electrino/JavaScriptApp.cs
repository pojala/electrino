using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Runtime.InteropServices;
using ChakraHost.Hosting;

namespace Electrino
{
    class JavaScriptApp
    {
        private JavaScriptSourceContext currentSourceContext = JavaScriptSourceContext.FromIntPtr(IntPtr.Zero);
        private JavaScriptRuntime runtime;
        private JavaScriptContext context;
        private JS.AbstractJSModule console;
        private JS.AbstractJSModule require;
        private JS.AbstractJSModule process;
        private JavaScriptValue jsAppGlobalObject;
        private static Queue taskQueue = new Queue();
        private static readonly JavaScriptPromiseContinuationCallback promiseContinuationDelegate = PromiseContinuationCallback;

        private static void PromiseContinuationCallback(JavaScriptValue task, IntPtr callbackState)
        {
            taskQueue.Enqueue(task);
            task.AddRef();
        }

        public string Init()
        {

            if (Native.JsCreateRuntime(JavaScriptRuntimeAttributes.EnableIdleProcessing, null, out runtime) != JavaScriptErrorCode.NoError)
                return "failed to create runtime.";

            if (Native.JsCreateContext(runtime, out context) != JavaScriptErrorCode.NoError)
                return "failed to create execution context.";

            if (Native.JsSetCurrentContext(context) != JavaScriptErrorCode.NoError)
                return "failed to set current context.";


            if (Native.JsSetPromiseContinuationCallback(promiseContinuationDelegate, IntPtr.Zero) != JavaScriptErrorCode.NoError)
                return "failed to setup callback for ES6 Promise";

            if (Native.JsProjectWinRTNamespace("Windows") != JavaScriptErrorCode.NoError)
                return "failed to project windows namespace.";

            if (Native.JsStartDebugging() != JavaScriptErrorCode.NoError)
                return "failed to start debugging.";


            if (Native.JsGetGlobalObject(out jsAppGlobalObject) != JavaScriptErrorCode.NoError)
                return "failed to get global object";

            console = new JS.JSConsole();
            require = new JS.JSRequire();
            process = new JS.JSProcess();
            JS.AbstractJSModule.AttachModule(jsAppGlobalObject, require);
            JS.AbstractJSModule.AttachModule(jsAppGlobalObject, console);
            JS.AbstractJSModule.AttachModule(jsAppGlobalObject, process);

            return "NoError";
        }
            
        public string RunScript(string script)
        {
            IntPtr returnValue;

            try
            {
                JavaScriptValue result;
                // failing because of "no context"
                if (Native.JsRunScript(script, currentSourceContext++, "", out result) != JavaScriptErrorCode.NoError)
                {
                    // Get error message and clear exception
                    JavaScriptValue exception;
                    if (Native.JsGetAndClearException(out exception) != JavaScriptErrorCode.NoError)
                        return "failed to get and clear exception";

                    JavaScriptPropertyId messageName;
                    if (Native.JsGetPropertyIdFromName("message",
                        out messageName) != JavaScriptErrorCode.NoError)
                        return "failed to get error message id";

                    JavaScriptValue messageValue;
                    if (Native.JsGetProperty(exception, messageName, out messageValue)
                        != JavaScriptErrorCode.NoError)
                        return "failed to get error message";

                    IntPtr message;
                    UIntPtr length;
                    if (Native.JsStringToPointer(messageValue, out message, out length) != JavaScriptErrorCode.NoError)
                        return "failed to convert error message";

                    return Marshal.PtrToStringUni(message);
                }

                // Execute promise tasks stored in taskQueue 
                while (taskQueue.Count != 0)
                {
                    JavaScriptValue task = (JavaScriptValue)taskQueue.Dequeue();
                    JavaScriptValue promiseResult;
                    JavaScriptValue[] args = new JavaScriptValue[1] { jsAppGlobalObject };
                    Native.JsCallFunction(task, args, 1, out promiseResult);
                    task.Release();
                }

                // Convert the return value.
                JavaScriptValue stringResult;
                UIntPtr stringLength;
                if (Native.JsConvertValueToString(result, out stringResult) != JavaScriptErrorCode.NoError)
                    return "failed to convert value to string.";
                if (Native.JsStringToPointer(stringResult, out returnValue, out stringLength) != JavaScriptErrorCode.NoError)
                    return "failed to convert return value.";
            }
            catch (Exception e)
            {
                return "chakrahost: fatal error: internal error: " + e.Message;
            }

            return Marshal.PtrToStringUni(returnValue);
        }

    }
}
