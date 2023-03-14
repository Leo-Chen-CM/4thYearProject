using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseState
{
    public string m_name;
    protected StateMachine m_stateMachine;
    public BaseState(string name, StateMachine stateMachine)
    {
        this.m_name= name;
        this.m_stateMachine= stateMachine;

    }

    public virtual void Enter() { }
    public virtual void UpdateLogic() { }
    public virtual void UpdatePhysics() { }
    public virtual void Exit() { }
}
