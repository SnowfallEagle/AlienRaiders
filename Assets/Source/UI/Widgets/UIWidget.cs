using UnityEngine;

public class UIWidget : CustomBehavior
{
    private UIWidget[] m_Children;

    protected virtual void Start()
    {
        m_Children = GetComponentsInChildren<UIWidget>(true);
    }

    public virtual void Show()
    {
        gameObject.SetActive(true);
    }

    public virtual void Hide()
    {
        gameObject.SetActive(false);

        foreach (var Child in m_Children)
        {
            if (Child != this)
            {
                Child.Hide();
            }
        }
    }
}
