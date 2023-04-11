using BehaviorTree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskPatrol : Node
{
    private Transform m_transform;
    private Transform[] m_wayPoints;

    private int m_currentWayPointsIndex = 0;

    private float m_waitTime = 1f;
    private float m_waitCounter = 0f;
    private bool m_waiting = false;
    public TaskPatrol(Transform transform, Transform[] controlPoints)
    {
        m_transform = transform;
        m_wayPoints = controlPoints;
    }
    public override NodeState Evaluate()
    {
        if (m_waiting)
        {
            m_waitCounter += Time.deltaTime;
            if (m_waitCounter >= m_waitTime)
            {
                m_waiting = false;
            }
            else
            {
                Transform wp = m_wayPoints[m_currentWayPointsIndex];
                if (Vector3.Distance(m_transform.position, wp.position) < 0.01f)
                {
                    m_transform.position = wp.position;
                    m_waitCounter = 0f;
                    m_waiting = true;

                    m_currentWayPointsIndex = (m_currentWayPointsIndex + 1) % m_wayPoints.Length;
                }
                else
                {

                }
            }
        }

        state = NodeState.RUNNING;
        return state;
    }
}
