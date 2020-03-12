using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChakraHost.Hosting;
using Windows.ApplicationModel;

namespace Electrino.JS
{
    class JSProcess : AbstractJSModule
    {
        private Dictionary<string, AbstractJSModule> modules = new Dictionary<string, AbstractJSModule>();

        public JSProcess() : base("process")
        {
            AttachProperty(JavaScriptValue.FromString("win32"), "platform");
            AttachProperty(JavaScriptValue.FromString(Package.Current.Id.Architecture.ToString()), "arch");
            AttachModule(new JSProcessVersions());
        }
    }

    class JSProcessVersions : AbstractJSModule
    {
        private Dictionary<string, AbstractJSModule> modules = new Dictionary<string, AbstractJSModule>();

        public JSProcessVersions() : base("versions")
        {
            AttachProperty(JavaScriptValue.FromString("electrino"), "0.1.0");
        }
    }
}
