using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Il2CppNewtonsoft.Json.Linq;

using Il2CppSystem.Linq;

namespace MarrowCauldron
{
    public static class JObjectHelper
    {
        public static JToken Get(this JObject obj, string name)
        {
            var children = obj.Children();
            if (children?._enumerable.Any((System.Func<JToken, bool>)(x => x.Path.EndsWith(name))) == true)
            {
                return obj[name];
            }
            else
            {
                return null;
            }
        }

        public static JToken Get(this JToken obj, string name)
        {
            var children = obj.Children();
            if (children?._enumerable.Any((System.Func<JToken, bool>)(x => x.Path.EndsWith(name))) == true)
            {
                return obj[name];
            }
            else
            {
                return null;
            }
        }
    }
}