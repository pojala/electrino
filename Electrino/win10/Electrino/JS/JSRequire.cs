using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChakraHost.Hosting;

namespace Electrino.JS
{
    class JSRequire
    {
        private Dictionary<string, AbstractJSModule> modules = new Dictionary<string, AbstractJSModule>();
        private string id = "require";

        public JSRequire(JavaScriptValue global)
        {
            AddModule(new JSPath());
            AddModule(new JSUrl());
            AbstractJSModule.AttachMethod(global, Main, id);
        }

        private void AddModule(AbstractJSModule module)
        {
            this.modules.Add(module.GetId(), module);
        }

        private JavaScriptValue Main(JavaScriptValue callee, bool isConstructCall, JavaScriptValue[] arguments, ushort argumentCount, IntPtr callbackData)
        {
            string moduleKey = AbstractJSModule.JSValToString(arguments[1]);
            AbstractJSModule module;
            if (!modules.TryGetValue(moduleKey, out module))
            {
                return JavaScriptValue.Undefined;
            }
            return module.GetModule();
        }
    }
}
