using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateManager : MonoBehaviour
{
    State m_currentState;
    // Update is called once per frame
    void Update()
    {
        
    }

    private void RunStateMachine()
    {
        State nextState = m_currentState?.RunCurrentState();

        if (nextState != null)
        {
            SwitchToTheNextState(nextState);
        }
    }




    private void SwitchToTheNextState(State t_nextState)
    {
        m_currentState= t_nextState;
    }
}
