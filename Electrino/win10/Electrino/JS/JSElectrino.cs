using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChakraHost.Hosting;

namespace Electrino.JS
{
    class JSElectrino : AbstractJSModule
    {
        public JSElectrino() : base("electrino")
        {
            AttachModule(new JSApp());
            AttachModule(new JSBrowserWindow());
        }
    }
}
