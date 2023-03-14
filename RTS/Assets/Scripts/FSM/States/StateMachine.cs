using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    BaseState m_currentState;
    // Update is called once per frame

    private void Start()
    {
        m_currentState = GetInitialState();
        if (m_currentState != null)
        {
            m_currentState.Enter();
        }
    }
    void Update()
    {
        if (m_currentState != null)
        {
            m_currentState.UpdateLogic();
        }
    }

    private void LateUpdate()
    {
        if(m_currentState != null)
        {
            m_currentState.UpdatePhysics();
        }
    }


    public void ChangeState(BaseState newState)
    {
        m_currentState.Exit();

        m_currentState = newState;
        m_currentState.Enter();
    }

    protected virtual BaseState GetInitialState()
    {
        return null;
    }

    private void OnGUI()
    {
        string content = m_currentState != null ? m_currentState.m_name : "{No current state}";
        GUILayout.Label($"<color='black'><size=40>{content}</size></color>");
    }
}
