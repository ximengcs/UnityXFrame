using System;
using UnityEngine;
using XFrame.Core;

namespace UnityXFrame.Core.UIs
{
    public class UIModule : SingletonModule<UIModule>
    {
        private Canvas m_Canvas;

        protected override void OnInit(object data)
        {
            base.OnInit(data);
            if (data != null)
                m_Canvas = (Canvas)data;
        }

        public void Open(Type type)
        {

        }

        public void Open<T>() where T : IUI
        {

        }

        public void Open(string group, Type type)
        {
             
        }

        public void Open(IUIGroup group)
        {

        }
    }
}
