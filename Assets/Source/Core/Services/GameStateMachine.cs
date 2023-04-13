using UnityEngine;
using UnityEngine.Assertions;

// @TODO: Game States should be Started, Updated by GameStateMachine. If we choose to have separate FightStage then we should Start, Update it too

public class GameStateMachine : Service<GameStateMachine>
{
    private GameState m_CurrentState;
    public GameState CurrentState => m_CurrentState;

    private void Start()
    {
        var Object = new GameObject();
        var FightState = Object.AddComponent<FightGameState>();

        int Level = GameEnvironment.Instance.GetDebugOption<bool>("DebugLevel.bSpecificLevel") ?
            GameEnvironment.Instance.GetDebugOption<int>("DebugLevel.Level") :
            FightGameState.AnyIdx;

        int Stage = GameEnvironment.Instance.GetDebugOption<bool>("DebugLevel.bSpecificStage") ?
            GameEnvironment.Instance.GetDebugOption<int>("DebugLevel.Stage") :
            FightGameState.AnyIdx;

        int Spawner = GameEnvironment.Instance.GetDebugOption<bool>("DebugLevel.bSpecificSpawner") ?
            GameEnvironment.Instance.GetDebugOption<int>("DebugLevel.Spawner") :
            FightGameState.AnyIdx;

        FightState.Initialize(Level, Stage, Spawner);
        m_CurrentState = FightState;
    }

    // @INCOMPLETE: SwitchState(GameState)

    public T GetCurrentState<T>() where T: GameState
    {
        return (T)m_CurrentState;
    }
}
