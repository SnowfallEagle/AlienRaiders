using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class MenuWidget : UIWidget
{
    [SerializeField] private Image m_FadeImage;

    private BehaviorComponent m_BehaviorComponent;

    protected override void Start()
    {
        base.Start();

        Assert.IsNotNull(m_FadeImage);

        m_BehaviorComponent = InitializeComponent<BehaviorComponent>();
    }

    public override void OnShow()
    {
        base.OnShow();

        m_FadeImage.color = new Color(0f, 0f, 0f, 0f);
        m_FadeImage.raycastTarget = false;
    }

    public void OnPlayClicked()
    {
        Debug.Log("Clicked");

        m_FadeImage.raycastTarget = true;
        m_BehaviorComponent.AddAction(
            new BHUIAction_FadeImage(m_FadeImage).AddOnActionFinished((_, _) =>
            {
                GameStateMachine.Instance.SwitchState(new FightGameState());
            })
        );
    }

    public void OnMuteClicked()
    {
        NotImplemented.Assert();
    }
}
