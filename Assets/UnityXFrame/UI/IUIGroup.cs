
namespace UnityXFrame.Core.UIs
{
    /// <summary>
    /// UI组
    /// 当打开UI组时，所有处于打开状态的UI会被打开
    /// 当关闭UI组时，所有UI会被关闭
    /// </summary>
    public interface IUIGroup : IUIElement
    {
        /// <summary>
        /// 是否处于打开状态
        /// </summary>
        bool IsOpen { get; }

        /// <summary>
        /// 组内UI整体不透明度
        /// </summary>
        float Alpha { get; set; }

        /// <summary>
        /// UI组层级, 层级大的在层级小的上层显示
        /// </summary>
        int Layer { get; set; }

        /// <summary>
        /// 打开UI组
        /// </summary>
        void Open();

        /// <summary>
        /// 关闭UI组
        /// </summary>
        void Close();

        #region Life Fun
        /// <summary>
        /// 组内打开UI生命周期
        /// </summary>
        /// <param name="ui">打开的UI</param>
        protected internal void OpenUI(IUI ui, object data);

        /// <summary>
        /// 组内关闭UI生命周期
        /// </summary>
        /// <param name="ui"></param>
        protected internal void CloseUI(IUI ui);

        /// <summary>
        /// 添加UI
        /// </summary>
        /// <param name="ui">需要添加的UI</param>
        protected internal void AddUI(IUI ui);

        /// <summary>
        /// 移除UI
        /// </summary>
        /// <param name="ui">需要移除的UI</param>
        protected internal void RemoveUI(IUI ui);

        /// <summary>
        /// 设置UI层级
        /// </summary>
        /// <param name="ui">需要设置的UI</param>
        /// <param name="layer">设置的层级</param>
        protected internal void SetUILayer(IUI ui, int layer);

        /// <summary>
        /// 初始化生命周期
        /// </summary>
        protected internal void OnInit();

        /// <summary>
        /// 更新生命周期
        /// </summary>
        protected internal void OnUpdate();

        /// <summary>
        /// 销毁生命周期
        /// </summary>
        protected internal void OnDestroy();
        #endregion
    }
}
