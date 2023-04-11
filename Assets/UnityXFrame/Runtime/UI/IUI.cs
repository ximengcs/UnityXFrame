using UnityEngine;
using XFrame.Modules.Containers;

namespace UnityXFrame.Core.UIs
{
    /// <summary>
    /// UI
    /// </summary>
    public interface IUI : IUIElement, IContainer
    {
        /// <summary>
        /// 根节点
        /// </summary>
        Transform Root { get; }

        /// <summary>
        /// 是否处于打开的状态
        /// </summary>
        bool IsOpen { get; }

        /// <summary>
        /// UI层级, 层级大的在层级小的上层显示
        /// </summary>
        int Layer { get; set; }

        /// <summary>
        /// 打开UI
        /// </summary>
        /// <param name="data">数据</param>
        void Open();

        /// <summary>
        /// 关闭UI
        /// </summary>
        void Close();

        /// <summary>
        /// UI所在组
        /// </summary>
        IUIGroup Group { get; }

        #region Life Fun
        /// <summary>
        /// 打开生命周期，每次UI打开时被调用
        /// </summary>
        /// <param name="data">数据</param>
        protected internal void OnOpen();

        /// <summary>
        /// 关闭生命周期，每次UI关闭时被调用
        /// </summary>
        protected internal void OnClose();

        /// <summary>
        /// 初始化生命周期
        /// </summary>
        /// <param name="inst">UI实体</param>
        protected internal void OnInit(GameObject inst);

        /// <summary>
        /// UI组改变生命周期
        /// </summary>
        /// <param name="newGroup"></param>
        protected internal void OnGroupChange(IUIGroup newGroup);

        /// <summary>
        /// UI更新生命周期
        /// </summary>
        protected internal void OnUpdate();

        /// <summary>
        /// UI销毁生命周期
        /// </summary>
        protected internal void OnDestroy();

        /// <summary>
        /// 设置层级，可选择是否刷新
        /// </summary>
        protected internal void SetLayer(int layer, bool refresh);
        #endregion
    }
}
