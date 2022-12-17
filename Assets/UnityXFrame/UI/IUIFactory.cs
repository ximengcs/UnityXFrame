using System;
using UnityEngine;
using UnityXFrame.Core.UIs;

namespace UnityXFrame.Core.UIs
{
    public interface IUIFactory
    {
        IUI Create(GameObject root, Type uiType);
    }
}
