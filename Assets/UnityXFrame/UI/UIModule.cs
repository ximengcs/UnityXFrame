using System;
using System.IO;
using UnityEngine;
using XFrame.Core;
using XFrame.Collections;
using XFrame.Modules.Pools;
using XFrame.Modules.XType;
using XFrame.Modules.Resource;
using XFrame.Modules.Diagnotics;
using UnityXFrame.Core.Resource;
using System.Collections.Generic;

namespace UnityXFrame.Core.UIs
{
    /// <summary>
    /// UI模块
    /// </summary>
    [CoreModule]
    [RequireModule(typeof(PoolModule))]
    public partial class UIModule : SingletonModule<UIModule>
    {
        private Canvas m_Canvas;
        private Transform m_Root;
        private TypeModule.System m_TypeSystem;
        private Dictionary<Type, IUI> m_UIMap;
        private Dictionary<Type, IUIFactory> m_Factorys;
        private XLinkList<IUIGroup> m_GroupList;

        #region Life Fun
        protected override void OnInit(object data)
        {
            base.OnInit(data);

            m_TypeSystem = TypeModule.Inst.GetOrNew<IUI>();
            InnerCheckCanvas(data);
            if (m_Canvas != null)
            {
                m_Root = m_Canvas.transform;
                m_UIMap = new Dictionary<Type, IUI>();
                m_GroupList = new XLinkList<IUIGroup>();
                m_Factorys = new Dictionary<Type, IUIFactory>();
                AddFactory<UI, UI.Factory>();
                AddFactory<MonoUI, MonoUI.Factory>();
            }
        }

        protected override void OnUpdate(float escapeTime)
        {
            base.OnUpdate(escapeTime);
            XLinkNode<IUIGroup> node = m_GroupList.First;
            while (node != null)
            {
                if (node.Value.IsOpen)
                    node.Value.OnUpdate();
                node = node.Next;
            }
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            XLinkNode<IUIGroup> node = m_GroupList.First;
            while (node != null)
            {
                node.Value.OnDestroy();
                node = node.Next;
            }
            m_GroupList.Dispose();
            m_GroupList = null;
        }
        #endregion

        #region Interface
        /// <summary>
        /// 主UI组
        /// </summary>
        public IUIGroup MainGroup
        {
            get { return InnerGetOrNewGroup(Constant.MAIN_GROUPUI, m_GroupList.Count); }
        }

        #region Open UI
        /// <summary>
        /// 打开UI，默认会在主UI组中打开
        /// </summary>
        /// <param name="uiType">UI类型</param>
        /// <param name="data">UI数据</param>
        /// <param name="useNavtive">是否为本地UI</param>
        /// <returns>UI实例</returns>
        public IUI Open(Type uiType, object data = default, bool useNavtive = false)
        {
            return Open(uiType, Constant.MAIN_GROUPUI, data, useNavtive);
        }

        /// <summary>
        /// 打开UI，默认会在主UI组中打开
        /// </summary>
        /// <typeparam name="T">UI类型</typeparam>
        /// <param name="data">UI数据</param>
        /// <param name="useNavtive">是否为本地UI</param>
        /// <returns>UI实例</returns>
        public T Open<T>(object data = default, bool useNavtive = false) where T : IUI
        {
            return (T)Open(typeof(T), data, useNavtive);
        }

        /// <summary>
        /// 打开UI，默认会在主UI组中打开
        /// </summary>
        /// <param name="uiName">UI名</param>
        /// <param name="data">UI数据</param>
        /// <param name="useNavtive">是否为本地UI</param>
        /// <returns>UI实例</returns>
        public IUI Open(string uiName, object data = default, bool useNavtive = false)
        {
            Type uiType = m_TypeSystem.GetByName(uiName);
            return Open(uiType, data, useNavtive);
        }

        /// <summary>
        /// 打开UI，在给定UI组中打开
        /// </summary>
        /// <param name="uiName">UI名</param>
        /// <param name="groupName">UI组名</param>
        /// <param name="data">UI数据</param>
        /// <param name="useNavtive">是否为本地UI</param>
        /// <returns>UI实例</returns>
        public IUI Open(string uiName, string groupName, object data = default, bool useNavtive = false)
        {
            Type uiType = m_TypeSystem.GetByName(uiName);
            return Open(uiType, groupName, data, useNavtive);
        }

        /// <summary>
        /// 打开UI，在给定UI组中打开
        /// </summary>
        /// <typeparam name="T">UI类型</typeparam>
        /// <param name="groupName">UI组名</param>
        /// <param name="data">UI数据</param>
        /// <param name="useNavtive">是否为本地UI</param>
        /// <returns>UI实例</returns>
        public T Open<T>(string groupName, object data = default, bool useNavtive = false) where T : IUI
        {
            return (T)Open(typeof(T), groupName, data, useNavtive);
        }

        /// <summary>
        /// 打开UI，在给定UI组中打开
        /// </summary>
        /// <param name="uiType">UI类型</param>
        /// <param name="groupName">UI组名</param>
        /// <param name="data">UI数据</param>
        /// <param name="useNavtive">是否为本地UI</param>
        /// <returns>UI实例</returns>
        public IUI Open(Type uiType, string groupName, object data = default, bool useNavtive = false)
        {
            IUIGroup group = InnerGetOrNewGroup(groupName, m_GroupList.Count);
            return InnerOpenUI(group, uiType, data, useNavtive);
        }

        /// <summary>
        /// 打开UI，在给定UI组中打开
        /// </summary>
        /// <param name="uiType">UI类型</param>
        /// <param name="group">UI组</param>
        /// <param name="data">UI数据</param>
        /// <param name="useNavtive">是否为本地UI</param>
        /// <returns>UI实例</returns>
        public IUI Open(Type uiType, IUIGroup group, object data = default, bool useNavtive = false)
        {
            return InnerOpenUI(group, uiType, data, useNavtive);
        }

        /// <summary>
        /// 打开UI，在给定UI组中打开
        /// </summary>
        /// <param name="ui">UI实例</param>
        /// <param name="groupName">UI组名</param>
        /// <param name="data">UI数据</param>
        /// <param name="useNavtive">是否为本地UI</param>
        /// <returns>UI实例</returns>
        public IUI Open(IUI ui, string groupName, object data = default, bool useNavtive = false)
        {
            IUIGroup group = InnerGetOrNewGroup(groupName, m_GroupList.Count);
            return InnerOpenUI(ui, group, data, useNavtive);
        }

        /// <summary>
        /// 打开UI，在给定UI组中打开
        /// </summary>
        /// <param name="ui">UI实例</param>
        /// <param name="group">UI组</param>
        /// <param name="data">UI数据</param>
        /// <param name="useNavtive">是否为本地UI</param>
        /// <returns>UI实例</returns>
        public IUI Open(IUI ui, IUIGroup group, object data = default, bool useNavtive = false)
        {
            return InnerOpenUI(ui, group, data, useNavtive);
        }

        /// <summary>
        /// 打开UI，在给定UI组中打开
        /// </summary>
        /// <typeparam name="T">UI类型</typeparam>
        /// <param name="group">UI组</param>
        /// <param name="data">UI数据</param>
        /// <param name="useNavtive">是否为本地UI</param>
        /// <returns>UI实例</returns>
        public T Open<T>(IUIGroup group, object data = default, bool useNavtive = false)
        {
            return (T)InnerOpenUI(group, typeof(T), data, useNavtive);
        }
        #endregion

        #region Close UI
        public void Close<T>() where T : IUI
        {
            InnerCloseUI(typeof(T));
        }

        public void Close(Type uiType)
        {
            InnerCloseUI(uiType);
        }

        public void Close(string uiName)
        {
            Type uiType = m_TypeSystem.GetByName(uiName);
            InnerCloseUI(uiType);
        }
        #endregion

        #region Get UI
        public IUI Get(Type uiType)
        {
            return InnerGetUI(uiType);
        }

        public T Get<T>() where T : IUI
        {
            return (T)InnerGetUI(typeof(T));
        }

        public IUI Get(string uiName)
        {
            Type uiType = m_TypeSystem.GetByName(uiName);
            return InnerGetUI(uiType);
        }
        #endregion

        /// <summary>
        /// 添加UI创建工厂
        /// </summary>
        /// <typeparam name="UIType">UI类型</typeparam>
        /// <typeparam name="T">工厂类型</typeparam>
        public void AddFactory<UIType, T>() where UIType : IUI where T : IUIFactory
        {
            Type uiType = typeof(UIType);
            if (m_Factorys.ContainsKey(uiType))
                return;
            m_Factorys[uiType] = (T)Activator.CreateInstance(typeof(T));
        }

        /// <summary>
        /// 获取(不存在时创建)UI组
        /// </summary>
        /// <param name="groupName">UI组名称</param>
        /// <param name="layer">UI组层级, 层级大的在层级小的上层显示</param>
        /// <returns>获取到的UI组</returns>
        public IUIGroup GetOrNewGroup(string groupName, int layer = -1)
        {
            if (layer == -1)
                layer = m_GroupList.Count;
            return InnerGetOrNewGroup(groupName, layer);
        }
        #endregion

        #region Inner Implement
        private IUIFactory InnerGetUIFactory(Type uiType)
        {
            if (m_Factorys.TryGetValue(uiType.BaseType, out IUIFactory factory))
                return factory;
            return default;
        }

        private void InnerCloseUI(Type uiTyp)
        {
            if (m_UIMap.TryGetValue(uiTyp, out IUI ui))
                ui.Close();
        }

        private IUI InnerOpenUI(IUIGroup group, Type uiType, object data, bool useNavtive)
        {
            if (!m_UIMap.TryGetValue(uiType, out IUI ui))
            {
                GameObject prefab;
                string uiPath = Path.Combine(Constant.UI_RES_PATH, uiType.Name);

                if (useNavtive)
                    prefab = NativeResModule.Inst.Load<GameObject>(uiPath);
                else
                    prefab = ResModule.Inst.Load<GameObject>(uiPath);


                if (prefab == null)
                {
                    Log.Error(nameof(UIModule), $"UI res {uiPath} dont exist.");
                    return default;
                }

                GameObject inst = GameObject.Instantiate(prefab);
                inst.name = uiType.Name;
                IUIFactory factory = InnerGetUIFactory(uiType);

                ui = factory.Create(inst, uiType);
                ui.OnInit(inst);
                m_UIMap[uiType] = ui;
            }

            return InnerOpenUI(ui, group, data, useNavtive);
        }

        private IUI InnerOpenUI(IUI ui, IUIGroup group, object data, bool useNavtive)
        {
            IUIGroup oldGroup = ui.Group;
            if (oldGroup != group)
            {
                oldGroup?.RemoveUI(ui);
                group.AddUI(ui);
                ui.OnGroupChange(group);
            }
            ui.Open(data);
            group.Open();
            return ui;
        }

        private void InnerCheckCanvas(object canvas)
        {
            if (canvas != null)
                m_Canvas = (Canvas)canvas;
            if (m_Canvas == null)
                m_Canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        }

        private IUI InnerGetUI(Type uiType)
        {
            if (m_UIMap.TryGetValue(uiType, out IUI ui))
                return ui;
            else
                return default;
        }

        private IUIGroup InnerGetOrNewGroup(string groupName, int layer)
        {
            XLinkNode<IUIGroup> node = m_GroupList.First;
            while (node != null)
            {
                if (node.Value.Name == groupName)
                    return node.Value;
                node = node.Next;
            }

            GameObject groupRoot = new GameObject(groupName, typeof(RectTransform), typeof(CanvasGroup));
            groupRoot.transform.SetParent(m_Root, false);
            IUIGroup group = new UIGroup(groupRoot, groupName, layer);
            group.OnInit();
            m_GroupList.AddLast(group);
            return group;
        }

        internal void SetUIGroupLayer(IUIGroup group, int layer)
        {
            layer = Mathf.Min(layer, m_GroupList.Count);
            layer = Mathf.Max(layer, 0);
            SetLayer(m_Root, group, layer);
        }
        #endregion
    }
}
