using UnityEngine.Assertions;

public class GameStateMachine : Service<GameStateMachine>
{
    private GameState m_CurrentState;
    public GameState CurrentState => m_CurrentState;

    public delegate void OnStateExitedSignature();
    public OnStateExitedSignature OnStateExited;

    private void Update()
    {
        m_CurrentState?.Update();
    }

    public void SwitchState(GameState State)
    {
        Assert.IsNotNull(State);

        m_CurrentState?.Exit();
        OnStateExited?.Invoke();

        m_CurrentState = State;
        m_CurrentState.Start();

        RenderingService.Instance.UpdateAppearance();
    }

    public T GetCurrentState<T>() where T: GameState
    {
        return m_CurrentState as T;
    }
}
