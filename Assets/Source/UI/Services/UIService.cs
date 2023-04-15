using System.Collections.Generic;
using UnityEngine;

public class UIService : Service<UIService>
{
    private Dictionary<System.Type, UIWidget> m_Widgets = new Dictionary<System.Type, UIWidget>();

    private void Start()
    {
        UIWidget[] Widgets = Resources.FindObjectsOfTypeAll<UIWidget>();

        foreach (var Widget in Widgets)
        {
            m_Widgets[Widget.GetType()] = Widget;
            Widget.gameObject.SetActive(false);
        }
    }

    public void Show(UIWidget Widget)
    {
        if (!Widget.bShown)
        {
            Widget.OnShow();
        }
    }

    public void Show<T>() where T : UIWidget
    {
        if (!m_Widgets.TryGetValue(typeof(T), out UIWidget Widget))
        {
            NoEntry.Assert($"There're no hud with type { typeof(T).Name }!");
            return;
        }

        Show(Widget);
    }

    public void Hide(UIWidget Widget)
    {
        if (Widget.bShown)
        {
            Widget.OnHide();
        }
    }

    public void Hide<T>() where T : UIWidget
    {
        if (!m_Widgets.TryGetValue(typeof(T), out UIWidget Widget))
        {
            NoEntry.Assert($"There're no hud with type { Widget.GetType().Name }!"); ;
            return;
        }

        Hide(Widget);
    }
}
