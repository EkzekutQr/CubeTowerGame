using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FSM<T> where T : IState
{
    private readonly Dictionary<Type, T> _states;

    public T CurrentState { get; protected set; }

    public FSM(IEnumerable<T> states)
    {
        _states = states.ToDictionary(s => s.GetType());
    }

    public void SetState<TState>() where TState : T => SetState(typeof(TState));

    public void SetState<TState, TPayload>(TPayload payload) where TState : T, IPayloadedState<TPayload>
    {
        SetState<TState>();

        if (CurrentState is IPayloadedState<TPayload> payloadedState)
            payloadedState.OnEnter(payload);
    }

    public void SetState(Type stateType)
    {
        Debug.Assert(typeof(T).IsAssignableFrom(stateType));

        CurrentState?.Exit();

        if (_states.TryGetValue(stateType, out T state))
        {
            CurrentState = state;
            CurrentState.Enter();
        }
        else
            Debug.LogError("No state with type " + stateType);
    }
}

public abstract class MonoStateBase<TOwner> : IState where TOwner : MonoBehaviour
{
    protected readonly TOwner owner;

    protected MonoStateBase(TOwner owner)
    {
        this.owner = owner;
    }

    public virtual void Enter() { }

    public virtual void Exit() { }
}

public interface IState
{
    void Enter();
    void Exit();
}
public interface IPayloadedState<in T>
{
    void OnEnter(T payload);
}
