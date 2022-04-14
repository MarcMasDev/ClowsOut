using System;
using System.Collections.Generic;
public class State
{
    public Action OnEnter;

    public Action OnStay;

    public Action OnExit;
}

public class FSM<T> where T : Enum
{
    public T currentState;

    private Dictionary<T, State> States;
    Action ReEnter;
    Action Exit;
    public FSM(T initState)
    {
        States = new Dictionary<T, State>();
        foreach (T e in Enum.GetValues(typeof(T)))
        {
            States.Add(e, new State());
        }

        currentState = initState;
    }

    public void Update()
    {
        States[currentState].OnStay?.Invoke();
    }

    public void ChangeState(T newState)
    {
        States[currentState].OnExit?.Invoke();
        States[newState].OnEnter?.Invoke();

        currentState = newState;
    }

    public void SetOnStay(T state, Action f)
    {
        States[state].OnStay = f;
    }
    public void SetOnEnter(T state, Action f)
    {
        States[state].OnEnter = f;
    }
    public void SetOnExit(T state, Action f)
    {
        States[state].OnExit = f;
    }
    public void SetReEnter(Action f) {
        ReEnter = f;
    }
    public virtual void SetExit(Action f) 
    {
        Exit = f;
    }
}