using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateMachine : CustomBehaviour
{
    [SerializeField] protected GameState m_InitialState;

    private GameState m_CurrentState;
    public GameState CurrentState => m_CurrentState;

    private void Start()
    {
        SetState(m_InitialState);
    }

    private void SetState(GameState State)
    {
        if (!State)
        {
            return;
        }

        m_CurrentState = Instantiate(State);
    }
}
