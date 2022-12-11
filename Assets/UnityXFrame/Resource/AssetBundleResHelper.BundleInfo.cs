using UnityEngine;

namespace UnityXFrame.Core
{
    public partial class AssetBundleResHelper
    {
        private class BundleInfo
        {
            public AssetBundle Bundle;
            public BundleInfo[] Dependencies;

            public string Name { get; }

            public BundleInfo(string name)
            {
                Name = name;
            }

            public Object Load(string res)
            {
                InnerCheckState();
                return Bundle.LoadAsset(res);
            }

            public void LoadAsync(string res, System.Action<Object> complete)
            {
                InnerCheckState();

                AssetBundleRequest request = Bundle.LoadAssetAsync<Object>(res);
                request.completed += (op) =>
                {
                    AssetBundleRequest abOp = op as AssetBundleRequest;
                    complete(abOp.asset);
                };
            }

            public T Load<T>(string res) where T : Object
            {
                InnerCheckState();
                return Bundle.LoadAsset<T>(res);
            }

            public void LoadAsync<T>(string res, System.Action<T> complete) where T : Object
            {
                InnerCheckState();

                AssetBundleRequest request = Bundle.LoadAssetAsync<T>(res);
                request.completed += (op) =>
                {
                    AssetBundleRequest abOp = op as AssetBundleRequest;
                    complete((T)abOp.asset);
                };
            }

            public void Unload()
            {
                Bundle.Unload(true);
            }

            private void InnerCheckState()
            {
                foreach (BundleInfo info in Dependencies)
                    info.InnerCheckState();
                if (Bundle == null)
                    Bundle = AssetBundle.LoadFromFile(Name);
            }
        }
    }
}
