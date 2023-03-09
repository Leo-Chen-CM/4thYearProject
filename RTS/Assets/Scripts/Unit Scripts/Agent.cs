using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Agent : MonoBehaviour
{
    private Vector3 m_target;
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
        m_rigidbody2D= GetComponent<Rigidbody2D>();
        m_unitFieldOfView = GetComponent<UnitFieldOfView>();
        m_agent = GetComponent<NavMeshAgent>();
        m_agent.updateRotation = false;
        m_agent.updateUpAxis = false;
        m_agent.updatePosition = false;
        //m_target = transform.position;

        m_currentState = State.Move;
        m_goal = new Vector3(0, 0, 0);
        //m_target = m_goal;

    }

    private void Start()
    {
        m_agent.Warp(transform.position);
        SetTargetPosition(m_goal);
        SetAgentPosition();
    }

    // Update is called once per frame
    //void Update()
    //{
    //    //SetAgentPosition();


    //}

    private void FixedUpdate()
    {
        //switch (m_currentState)
        //{
        //    case State.Idle:
        //        Idle();
        //        break;

        //    case State.Move:
        //        MoveTo();
        //        break;

        //    case State.Shoot:

        //        break;
        //}
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
        if (Vector3.Distance(m_target, transform.position) <= m_agent.stoppingDistance)
        {
            m_agent.velocity = Vector3.zero;
            m_rigidbody2D.velocity = Vector3.zero;
            m_target = transform.position;
            m_destinationReached = true;

            //m_currentState = State.Idle;
        }

    }

    void RotateTowards()
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
        m_destinationReached = false;
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