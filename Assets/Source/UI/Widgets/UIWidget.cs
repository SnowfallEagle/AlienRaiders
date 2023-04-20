public class UIWidget : CustomBehavior
{
    private bool m_bShown = false;
    public bool bShown => m_bShown;

    private UIWidget[] m_Children;

    public virtual void Initialize()
    {
        m_Children = GetComponentsInChildren<UIWidget>(true);
    }

    /** UIService call it! Use UIService.Show() to show widget */
    public virtual void OnShow()
    {
        gameObject.SetActive(true);
        m_bShown = true;
    }

    /** UIService call it! Use UIService.Hide() to hide widget */
    public virtual void OnHide()
    {
        gameObject.SetActive(false);

        if (m_Children != null)
        {
            foreach (var Child in m_Children)
            {
                if (Child != this)
                {
                    Child.OnHide();
                }
            }
        }

        m_bShown = false;
    }
}
