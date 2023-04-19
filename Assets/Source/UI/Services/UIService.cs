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

    public void ShowWidget(UIWidget Widget)
    {
        if (!Widget.bShown)
        {
            Widget.OnShow();
        }
    }

    public void ShowWidget<T>() where T : UIWidget
    {
        if (!m_Widgets.TryGetValue(typeof(T), out UIWidget Widget))
        {
            NoEntry.Assert($"There're no hud with type { typeof(T).Name }!");
            return;
        }

        ShowWidget(Widget);
    }

    public void HideWidget(UIWidget Widget)
    {
        if (Widget.bShown)
        {
            Widget.OnHide();
        }
    }

    public void HideWidget<T>() where T : UIWidget
    {
        if (!m_Widgets.TryGetValue(typeof(T), out UIWidget Widget))
        {
            NoEntry.Assert($"There're no hud with type { Widget.GetType().Name }!"); ;
            return;
        }

        HideWidget(Widget);
    }
}
