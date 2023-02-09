using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Agent : MonoBehaviour
{
    private Vector3 m_target;
    NavMeshAgent m_agent;
    Rigidbody2D m_rigidbody2D;
    [SerializeField]
    private float m_rotationSpeed;
    [SerializeField]
    private float m_rotationModifier;
    [SerializeField]
    private UnitFieldOfView m_unitFieldOfView;

    private void Awake()
    {
        m_rigidbody2D= GetComponent<Rigidbody2D>();
        m_unitFieldOfView = GetComponent<UnitFieldOfView>();
        m_agent = GetComponent<NavMeshAgent>();
        m_agent.updateRotation = false;
        m_agent.updateUpAxis = false;
        m_agent.updatePosition = false;
        m_target = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        //SetAgentPosition();
        m_rigidbody2D.velocity = m_agent.velocity;
        m_agent.nextPosition = m_rigidbody2D.position;

        if (Vector3.Distance(m_target, transform.position) <= m_agent.stoppingDistance)
        {
            m_agent.velocity = Vector3.zero;
            m_rigidbody2D.velocity = Vector3.zero;
            m_target = transform.position;
        }

    }

    private void FixedUpdate()
    {
        if (m_target != transform.position && !m_unitFieldOfView.m_enemySpotted)
        {
            //Vector3 vectorToTarget = m_target - transform.position;
            //float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg - m_rotationModifier;
            //Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
            //transform.rotation = Quaternion.Slerp(transform.rotation, q, Time.deltaTime * m_rotationSpeed);
            transform.rotation = Quaternion.LookRotation(Vector3.forward, m_rigidbody2D.velocity * m_rotationSpeed);
        }
    }

    public void SetTargetPosition(Vector3 t_position)
    {
        m_target = t_position;
        SetAgentPosition();

    }


    void SetAgentPosition()
    {
        Vector3 moveDirection = (m_target - transform.position).normalized;
        
        m_agent.velocity = moveDirection * 5;

        m_agent.SetDestination(new Vector3(m_target.x, m_target.y, 0));
    }
}