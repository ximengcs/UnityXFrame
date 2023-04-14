using DG.Tweening;
using UnityEngine;

namespace UnityXFrame.Core.UIs
{
    public class TestUIGroupHelper : UIGroupHelperBase
    {
        private Vector3 m_DefaultScale;

        protected override void OnInit()
        {
            base.OnInit();
            m_DefaultScale = Vector2.one * 0;
        }

        protected override void OnUIOpen(IUI ui)
        {
            InnerSetUIActive(ui, true);
            ui.Root.localScale = m_DefaultScale;
            ui.Root.DOScale(Vector3.one, 0.3f).OnComplete(() =>
            {
                InnerOpenUI(ui);
            });
        }

        protected override void OnUIClose(IUI ui)
        {
            ui.Root.DOScale(m_DefaultScale, 0.3f).OnComplete(() =>
            {
                InnerCloseUI(ui);
                InnerSetUIActive(ui, false);
            });
        }
    }
}
