using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class FiniteStateMachineAI : RTSGameController
{
    /// <summary>
    /// This AI is meant to represent Field Marhsal Douglas Haig
    /// The Blackadder representation of him.
    /// Where he has devised a brilliant tactical plan to secure final victory in the field.
    /// 
    /// Unfortunately that plan is for the troopers to march in to the field in lines of 9 to the capture point
    /// </summary>


    enum States
    {
        //Idle,
        GatherForces,
        RallyPoint,
        MoveOut
    }

    [SerializeField]
    private States m_states = States.GatherForces;

    [SerializeField]
    private List<Transform> m_wayPoints = new List<Transform>();

    [SerializeField]
    private List<Transform> m_rallyPoints = new List<Transform>();

    //[SerializeField]
    //private UnitSpawner m_spawner;

    private LayerMask m_entityLayer;

    [SerializeField]
    private int m_maxSoldiers;

    [SerializeField]
    private int m_ordersTimeDelay;

    [SerializeField]
    private Transform[] m_selectionArea;

    [SerializeField]
    Formations m_formations = Formations.Line;

    private void Awake()
    {
        m_selectedUnits = new List<BaseUnit>();
    }

    void Start()
    {
        m_entityLayer = LayerMask.GetMask("EntityLayer");
        m_AI = true;
        StartCoroutine(Orders());
    }
    IEnumerator Orders()
    {
        while (true)
        {
            switch (m_states)
            {
                //case States.Idle:
                //    //AwaitingOrders();
                //    break;

                case States.GatherForces:
                    yield return new WaitForSeconds(m_ordersTimeDelay);
                    GatherForces();

                    break;

                case States.RallyPoint:
                    //m_states = States.Idle;
                    //StartCoroutine(MoveToRallyPoint());
                    //MoveToPoint(m_wayPoints[0].position);
                    SetFormationPosition(m_rallyPoints[Random.Range(0,m_rallyPoints.Count)].position);
                    yield return new WaitForSeconds(m_ordersTimeDelay);
                    m_states = States.MoveOut;
                    break;

                case States.MoveOut:
                    //m_states = States.Idle;
                    //StartCoroutine(MoveOut());
                    SetFormationPosition(m_wayPoints[Random.Range(0, m_wayPoints.Count)].position + new Vector3(Random.Range(-5, 5), Random.Range(-5, 5), 0));
                    //MoveToPoint(m_wayPoints[1].position + new Vector3(Random.Range(-5, 5), Random.Range(-5, 5), 0));
                    yield return new WaitForSeconds(m_ordersTimeDelay);
                    m_states = States.GatherForces;
                    break;
            }
        }
    }


    void GatherForces()
    {
        Collider2D[] collider2DArray;

        collider2DArray = Physics2D.OverlapAreaAll(m_selectionArea[0].position, m_selectionArea[1].position, m_entityLayer);

        //if (gameObject.tag == "Team1")
        //{


        //    collider2DArray = Physics2D.OverlapAreaAll(new Vector3(-250, 75, 0), new Vector3(-175, -75, 0), m_entityLayer);
        //}
        //else
        //{
        //    collider2DArray = Physics2D.OverlapAreaAll(new Vector3(175, 75, 0), new Vector3(250, -75, 0), m_entityLayer);
        //}

        m_selectedUnits.Clear();

        foreach (Collider2D collider2D in collider2DArray)
        {
            BaseUnit unitRTS = collider2D.GetComponent<BaseUnit>();

            if (unitRTS != null && unitRTS.gameObject.tag == gameObject.tag)
            {
                //unitRTS.SetSelectedVisible(true);
                m_selectedUnits.Add(unitRTS);
            }
        }
        Debug.Log("The FSM has troopers rallied");

        

        m_states = States.RallyPoint;
    }

    //void MoveToPoint(Vector3 t_destination)
    //{
    //    List<Vector3> targetPositionList = GetLinePositionList(t_destination);

    //    int targetPositionListIndex = 0;

    //    foreach (UnitRTS unit in m_selectedUnits)
    //    {
    //        unit.m_agent.SetTargetPosition(targetPositionList[targetPositionListIndex]);
    //        targetPositionListIndex = (targetPositionListIndex + 1) % targetPositionList.Count;
    //    }
    //}


}
