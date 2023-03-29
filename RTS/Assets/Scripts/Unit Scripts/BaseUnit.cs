using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static UnityEditor.VersionControl.Asset;

public class BaseUnit : MonoBehaviour
{
    private Vector3 m_targetDestination;
    public Vector3 m_goal;
    NavMeshAgent m_agent;
    Rigidbody2D m_rigidbody2D;
    [SerializeField]
    private float m_rotationSpeed;
    [SerializeField]
    private float m_rotationModifier;
    [SerializeField]
    private UnitFieldOfView m_unitFieldOfView;

    [SerializeField]
    private bool m_destinationReached = false;

    public bool m_leader = false;

    [SerializeField]
    private bool m_toggleGoal = false;
    enum State
    {
        Idle,
        Move,
        Shoot
    }

    [SerializeField]
    private State m_currentState;

    private void Awake()
    {
        m_rigidbody2D = GetComponent<Rigidbody2D>();
        m_unitFieldOfView = GetComponent<UnitFieldOfView>();
        m_agent = GetComponent<NavMeshAgent>();
        m_agent.updateRotation = false;
        m_agent.updateUpAxis = false;
        m_agent.updatePosition = false;
        m_targetDestination = transform.position;
    }

    private void Start()
    {
        if (m_toggleGoal)
        {
            m_agent.Warp(transform.position);
            SetTargetPosition(m_goal);
        }

        SetAgentPosition();
    }

    private void FixedUpdate()
    {

        m_agent.nextPosition = m_rigidbody2D.position;
        m_rigidbody2D.velocity = m_agent.velocity;

        if (m_destinationReached == false)
        {
            CheckPosition();
        }
        else
        {
            Idle();
        }

        RotateTowards();

    }

    void Idle()
    {
        m_agent.SetDestination(new Vector3(transform.position.x, transform.position.y, 0));
    }

    void CheckPosition()
    {
        if (Vector3.Distance(m_targetDestination, transform.position) <= m_agent.stoppingDistance)
        {
            m_agent.velocity = Vector3.zero;
            m_rigidbody2D.velocity = Vector3.zero;
            m_targetDestination = transform.position;
            m_destinationReached = true;

            //m_currentState = State.Idle;
        }

    }

    void RotateTowards()
    {
        if (m_targetDestination != transform.position && !m_unitFieldOfView.m_enemySpotted)
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
        m_destinationReached = false;
        m_targetDestination = t_position;
        SetAgentPosition();
    }


    void SetAgentPosition()
    {
        Vector3 moveDirection = (m_targetDestination - transform.position).normalized;

        m_agent.velocity = moveDirection * 5;

        m_agent.SetDestination(new Vector3(m_targetDestination.x, m_targetDestination.y, 0));
    }

    public void ToggleLeader(bool t_toggle)
    {
        m_leader = t_toggle;
    }
}
