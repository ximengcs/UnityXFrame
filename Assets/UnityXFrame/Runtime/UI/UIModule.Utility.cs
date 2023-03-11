using UnityEngine;

namespace UnityXFrame.Core.UIs
{
    public partial class UIModule
    {
        internal static void SetLayer(Transform root, IUIElement element, int layer)
        {
            Transform check = root.GetChild(layer);
            if (check.name == element.Name)
                return;

            bool find = false;
            Transform[] list = new Transform[root.childCount];

            int curIndex = 0;
            for (int i = 0; i < list.Length; i++, curIndex++)
            {
                Transform child = root.GetChild(i);
                if (!find && child.name == element.Name)
                {
                    find = true;
                    list[layer] = child;
                    if (layer != curIndex)
                        curIndex--;
                }
                else
                {
                    if (layer == curIndex)
                        curIndex++;
                    list[curIndex] = child;
                }
            }

            root.DetachChildren();
            foreach (Transform child in list)
                child.SetParent(root);
        }
    }
}
