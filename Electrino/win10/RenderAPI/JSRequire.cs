using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation.Metadata;

namespace RenderAPI
{
    public delegate object RequireFunc(string name);

    [AllowForWeb]
    public sealed class JSRequire
    {
        private Dictionary<string, object> modules = new Dictionary<string, object>();

        public RequireFunc Main
        {
            get
            {
                RequireFunc del = (string name) =>
                {
                    modules.TryGetValue(name, out object module);
                    return module;
                };
                return del;
            }
        }

        public JSRequire()
        {
            modules.Add("os", new JSOs());
        }
    }
}
