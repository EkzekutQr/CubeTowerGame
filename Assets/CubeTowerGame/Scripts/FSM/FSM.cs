using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSM
{
    public IState CurrentState { get; private set; }

    public void ChangeState(IState newState)
    {
        CurrentState?.Exit();
        CurrentState = newState;
        CurrentState?.Enter();
    }
}

public interface IState
{
    void Enter();
    void Exit();
}
