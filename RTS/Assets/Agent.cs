using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Agent : MonoBehaviour
{
    private Vector3 m_target;
    NavMeshAgent m_agent;
    Rigidbody2D m_rigidbody2D;
    private void Awake()
    {
        m_rigidbody2D= GetComponent<Rigidbody2D>();
        m_agent= GetComponent<NavMeshAgent>();
        m_agent.updateRotation = false;
        m_agent.updateUpAxis = false;
        m_agent.updatePosition = false;
    }

    // Update is called once per frame
    void Update()
    {
        SetTargetPosition();
        //SetAgentPosition();
        //m_rigidbody2D.velocity = m_agent.velocity;
        m_agent.nextPosition = m_rigidbody2D.position;
    }



    void SetTargetPosition()
    {
        if (Input.GetMouseButtonDown(1))
        {
            m_target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            SetAgentPosition();
        }
    }


    void SetAgentPosition()
    {
        Vector3 moveDirection = (m_target - transform.position).normalized;
        
        m_agent.velocity = moveDirection * 5;

        m_agent.SetDestination(new Vector3(m_target.x, m_target.y, transform.position.z));
    }
}