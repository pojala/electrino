using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChakraHost.Hosting;

namespace Electrino.JS
{
    class JSRequire : AbstractJSModule
    {
        private Dictionary<string, AbstractJSModule> modules = new Dictionary<string, AbstractJSModule>();

        public JSRequire() : base("require", true)
        {
            AddModule(new JSPath());
            AddModule(new JSUrl());
            AddModule(new JSElectrino());
        }

        private void AddModule(AbstractJSModule module)
        {
            modules.Add(module.GetId(), module);
        }

        protected override JavaScriptValue Main(JavaScriptValue callee, bool isConstructCall, JavaScriptValue[] arguments, ushort argumentCount, IntPtr callbackData)
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
