using UnityEngine.Assertions;

public class GameStateMachine : Service<GameStateMachine>
{
    private GameState m_CurrentState;
    public GameState CurrentState => m_CurrentState;

    private void Update()
    {
        m_CurrentState?.Update();
    }

    public void SwitchState(GameState State)
    {
        Assert.IsNotNull(State);

        m_CurrentState?.Exit();

        m_CurrentState = State;
        m_CurrentState.Start();
    }

    public T GetCurrentState<T>() where T: GameState
    {
        return (T)m_CurrentState;
    }
}
