using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BaseUnit : MonoBehaviour
{
    private Vector3 m_targetDestination;
    public Vector3 m_goal;
    public NavMeshAgent m_agent;
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

    private GameObject m_selectedGameObject;
    private GameObject m_viewVisualisation;
    [SerializeField]
    int m_health = 3;

    private void Awake()
    {
        m_rigidbody2D = GetComponent<Rigidbody2D>();
        m_unitFieldOfView = GetComponent<UnitFieldOfView>();
        m_agent = GetComponent<NavMeshAgent>();
        m_agent.updateRotation = false;
        m_agent.updateUpAxis = false;
        m_agent.updatePosition = false;
        m_targetDestination = transform.position;

        m_selectedGameObject = transform.Find("Selected").gameObject;
        m_viewVisualisation = transform.Find("View Visualisation").gameObject;

        SetSelectedVisible(false);
    }
    public void SetupTeam(string t_teamTag)
    {
        gameObject.tag = t_teamTag;
        if (gameObject.tag == "Team1")
        {
            gameObject.GetComponent<SpriteRenderer>().color = Color.blue;
        }

        if (gameObject.tag == "Team2")
        {
            gameObject.GetComponent<SpriteRenderer>().color = Color.red;
        }
    }
    public void SetSelectedVisible(bool t_visible)
    {
        m_selectedGameObject.SetActive(t_visible);
        m_viewVisualisation.SetActive(t_visible);
    }

    private void Start()
    {
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


    public void LoseHealth()
    {
        if (m_health > 1)
        {
            m_health--;
        }
        else
        {
            m_health--;
            Destroy(gameObject);
        }
    }
}
