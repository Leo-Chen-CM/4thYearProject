using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class FiniteStateMachineAI : MonoBehaviour
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
    private float m_lineOffset;

    [SerializeField]
    private States m_states = States.GatherForces;

    [SerializeField]
    private List<Transform> m_wayPoints = new List<Transform>();

    [SerializeField]
    private UnitSpawner m_spawner;

    private LayerMask m_entityLayer;

    private List<UnitRTS> m_selectedUnits;

    bool m_idling = true;

    bool m_ordersSent = false;

    [SerializeField]
    private int m_maxSoldiers;

    [SerializeField]
    private int m_ordersTimeDelay;

    private void Awake()
    {
        m_selectedUnits = new List<UnitRTS>();
    }

    void Start()
    {
        m_entityLayer = LayerMask.GetMask("EntityLayer");
        StartCoroutine(Orders());
    }

    //// Update is called once per frame
    //void Update()
    //{
    //    switch (m_states)
    //    {
    //        case States.Idle:
    //            AwaitingOrders();
    //            break;

    //        case States.GatherForces:
    //            GatherForces();
    //            break;

    //        case States.RallyPoint:
    //            m_states = States.Idle;
    //            //StartCoroutine(MoveToRallyPoint());
    //            break;

    //        case States.MoveOut:
    //            m_states = States.Idle;
    //            //StartCoroutine(MoveOut());
    //            break;
    //    }
    //}

    void GatherForces()
    {
        Collider2D[] collider2DArray = Physics2D.OverlapAreaAll(new Vector3(-50, 75, 0), new Vector3(50, 55, 0), m_entityLayer);
        m_selectedUnits.Clear();
        if (collider2DArray.Length >= m_maxSoldiers)
        {
            foreach (Collider2D collider2D in collider2DArray)
            {
                UnitRTS unitRTS = collider2D.GetComponent<UnitRTS>();

                if (unitRTS != null && unitRTS.gameObject.tag == "Team2")
                {
                    //unitRTS.SetSelectedVisible(true);
                    m_selectedUnits.Add(unitRTS);
                }
            }
            Debug.Log("The FSM has troopers rallied");

            m_states = States.RallyPoint;

        }
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
                    GatherForces();
                    yield return new WaitForEndOfFrame();
                    break;

                case States.RallyPoint:
                    //m_states = States.Idle;
                    //StartCoroutine(MoveToRallyPoint());
                    MoveToPoint(m_wayPoints[0].position);
                    yield return new WaitForSeconds(m_ordersTimeDelay);
                    m_states = States.MoveOut;
                    break;

                case States.MoveOut:
                    //m_states = States.Idle;
                    //StartCoroutine(MoveOut());
                    MoveToPoint(m_wayPoints[1].position);
                    yield return new WaitForSeconds(m_ordersTimeDelay);
                    m_states = States.GatherForces;
                    break;
            }
        }
    }

    //IEnumerator MoveToRallyPoint()
    //{
    //    //Debug.Log("Troops are heading towards the rallying point");
    //    MoveToPoint(m_wayPoints[0].position);
    //    yield return new WaitForSeconds(30);
    //    m_states = States.MoveOut;
    //}

    //IEnumerator MoveOut()
    //{
    //    //Debug.Log("Troops are now attacking the control point");
    //    MoveToPoint(m_wayPoints[1].position);
    //    yield return new WaitForSeconds(5);
    //    m_states = States.GatherForces;
    //}

    void MoveToPoint(Vector3 t_destination)
    {
        List<Vector3> targetPositionList = GetLinePositionList(t_destination);

        int targetPositionListIndex = 0;

        foreach (UnitRTS unit in m_selectedUnits)
        {
            unit.m_agent.SetTargetPosition(targetPositionList[targetPositionListIndex]);
            targetPositionListIndex = (targetPositionListIndex + 1) % targetPositionList.Count;
        }
    }


    private List<Vector3> GetLinePositionList(Vector3 t_startPosition)
    {
        List<Vector3> positionList = new List<Vector3>();

        float rowMax = Mathf.Ceil(m_selectedUnits.Count / 9f);

        for (int j = 0; j < rowMax; j++)
        {
            for (int i = 0; i < 9; i++)
            {
                if (i % 2 != 0)
                {
                    Vector3 offset = new Vector3(t_startPosition.x + m_lineOffset * i, t_startPosition.y - m_lineOffset * j, 0);
                    positionList.Add(offset);
                }
                else
                {
                    Vector3 offset = new Vector3(t_startPosition.x - m_lineOffset * i, t_startPosition.y - m_lineOffset * j, 0);
                    positionList.Add(offset);
                }
            }
        }

        return positionList;
    }

}
