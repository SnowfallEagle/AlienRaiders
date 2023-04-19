using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

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
        if (!Widget)
        {
            NoEntry.Assert();
            return;
        }

        if (!Widget.bShown)
        {
            Widget.OnShow();
        }
    }

    public void ShowWidget<T>() where T : UIWidget
    {
        ShowWidget(GetWidget<T>());
    }

    public void HideWidget(UIWidget Widget)
    {
        if (!Widget)
        {
            NoEntry.Assert();
            return;
        }

        if (Widget.bShown)
        {
            Widget.OnHide();
        }
    }

    public void HideWidget<T>() where T : UIWidget
    {
        HideWidget(GetWidget<T>());
    }

    public T GetWidget<T>() where T : UIWidget
    {
        if (!m_Widgets.TryGetValue(typeof(T), out UIWidget Widget))
        {
            NoEntry.Assert($"There're no widget with type { Widget.GetType().Name }!"); ;
            return null;
        }

        return (T)Widget;
    }
}
