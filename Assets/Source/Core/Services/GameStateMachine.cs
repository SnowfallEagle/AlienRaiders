using UnityEngine;
using UnityEngine.Assertions;

/* @TODO:
    Game States should be Started, Updated by GameStateMachine.
    If we choose to have separate FightStage then we should Start, Update it too
    Also, we'll manually remove timers from states and stages
*/

public class GameStateMachine : Service<GameStateMachine>
{
    private GameState m_CurrentState;
    public GameState CurrentState => m_CurrentState;

    public void SwitchState(GameState State)
    {
        if (m_CurrentState)
        {
            Destroy(m_CurrentState.gameObject);
        }

        State.name = State.GetType().Name;
        m_CurrentState = State;
    }

    public T GetCurrentState<T>() where T: GameState
    {
        return (T)m_CurrentState;
    }
}
