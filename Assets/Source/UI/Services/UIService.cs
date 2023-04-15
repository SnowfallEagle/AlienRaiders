using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UIService : Service<UIService>
{
    private Dictionary<System.Type, UIWidget> m_Widgets = new Dictionary<System.Type, UIWidget>();

    private void Start()
    {
        UIWidget[] Widgets = FindObjectsOfType<UIWidget>();

        foreach (var Widget in Widgets)
        {
            m_Widgets[Widget.GetType()] = Widget;
            Widget.gameObject.SetActive(false);
        }
    }

    public void Show<T>() where T : UIWidget
    {
        if (!m_Widgets.TryGetValue(typeof(T), out UIWidget Widget))
        {
            NoEntry.Assert($"There're no hud with type {Widget.GetType().Name}!");
            return;
        }
        Widget.gameObject.SetActive(true);
    }

    public void Hide<T>() where T : UIWidget
    {
        if (!m_Widgets.TryGetValue(typeof(T), out UIWidget Hud))
        {
            NoEntry.Assert($"There're no hud with type {Hud.GetType().Name}!"); ;
            return;
        }
        Hud.gameObject.SetActive(false);
    }
}
