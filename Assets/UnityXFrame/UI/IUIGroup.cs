
namespace UnityXFrame.Core.UIs
{
    /// <summary>
    /// UI组
    /// 当打开UI组时，所有处于打开状态的UI会被打开
    /// 当关闭UI组时，所有UI会被关闭
    /// </summary>
    public interface IUIGroup
    {
        /// <summary>
        /// 标识名
        /// </summary>
        string Name { get; }

        /// <summary>
        /// 是否处于打开状态
        /// </summary>
        bool IsOpen { get; }

        /// <summary>
        /// 组内UI整体不透明度
        /// </summary>
        float Alpha { get; set; }

        /// <summary>
        /// UI组层级
        /// </summary>
        int Layer { get; set; }

        /// <summary>
        /// 组内打开UI生命周期
        /// </summary>
        /// <param name="ui">打开的UI</param>
        protected internal void OnOpenUI(IUI ui);

        /// <summary>
        /// 组内关闭UI生命周期
        /// </summary>
        /// <param name="ui"></param>
        protected internal void OnCloseUI(IUI ui);

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

        /// <summary>
        /// 打开UI组
        /// </summary>
        void Open();

        /// <summary>
        /// 关闭UI组
        /// </summary>
        void Close();
    }
}
