using System;
using System.Reflection;
using System.Threading.Tasks;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace UnityXFrame.Core.Resource
{
    public partial class AddressablesHelper
    {
        private static class Ext
        {
            private static MethodInfo s_LoadAssetAsync;

            public static void OnInit()
            {
                Type type = typeof(Addressables);
                foreach (MethodInfo info in type.GetMethods())
                {
                    if (info.Name == "LoadAssetAsync" && info.GetParameters()[0].ParameterType == typeof(object))
                    {
                        s_LoadAssetAsync = info;
                        break;
                    }
                }
                
            }

            public static object LoadAssetAsync(string resPath, Type resType)
            {
                object handle = s_LoadAssetAsync.MakeGenericMethod(resType).Invoke(null, new object[] { resPath });
                return handle;
            }
        }
    }
}
