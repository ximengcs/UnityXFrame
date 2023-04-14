using DG.Tweening;
using UnityEngine;

namespace UnityXFrame.Core.UIs
{
    public class TestMainGroupHelper : UIGroupHelperBase
    {
        protected override void OnUIOpen(IUI ui)
        {
            InnerSetUIActive(ui, true);
            Vector2 startPos = new Vector2(-ui.Root.sizeDelta.x, 0);
            ui.Root.anchoredPosition = startPos;
            DOTween.To(
                () => ui.Root.anchoredPosition,
                (pos) => ui.Root.anchoredPosition = pos,
                Vector2.zero, 0.3f).OnComplete(() =>
                {
                    InnerOpenUI(ui);
                });
        }

        protected override void OnUIClose(IUI ui)
        {
            Vector2 startPos = new Vector2(-ui.Root.sizeDelta.x, 0);
            DOTween.To(
                () => ui.Root.anchoredPosition,
                (pos) => ui.Root.anchoredPosition = pos,
                startPos, 0.3f).OnComplete(() =>
                {
                    InnerCloseUI(ui);
                    InnerSetUIActive(ui, false);
                });
        }
    }
}
