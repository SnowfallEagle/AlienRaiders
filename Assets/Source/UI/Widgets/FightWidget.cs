public class FightWidget : UIWidget
{
    public void OnPauseClicked()
    {
        UIService.Instance.Show<PauseWidget>();
    }
}
