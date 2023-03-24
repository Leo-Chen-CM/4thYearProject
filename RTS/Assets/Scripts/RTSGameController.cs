using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

enum Teams
{
    Team1,
    Team2
};

enum Formations
{
    Line,
    Box,
    Cheveron
}

public class RTSGameController : MonoBehaviour
{
    [SerializeField]
    private Transform m_selectedAreaTransform;
    private Vector3 m_startPosition;
    protected List<UnitRTS> m_selectedUnits;
    // Update is called once per frame

    [SerializeField]
    Teams m_team = Teams.Team1;

    [SerializeField]
    Formations m_formation = Formations.Line;

    [SerializeField]
    private float m_lineOffset;

    protected bool m_AI = false;



    private void Awake()
    {
        m_selectedUnits = new List<UnitRTS>();
        m_selectedAreaTransform.gameObject.SetActive(false);
    }



    public void HandleInputData(int val)
    {
        
        if (val == 0)
        {
            m_team = Teams.Team1;
        }        
        if (val == 1)
        {
            m_team = Teams.Team2;
        }
    }
    void Update()
    {
        if (!m_AI)
        {
            //Checks if any units selected has died and removes them from the list
            for (int i = 0; i < m_selectedUnits.Count; i++)
            {
                if (m_selectedUnits[i] == null)
                {
                    m_selectedUnits.Remove(m_selectedUnits[i]);
                }
            }

            if (Input.GetMouseButtonDown(0))
            {
                m_selectedAreaTransform.gameObject.SetActive(true);
                m_startPosition = Utility.ReturnMousePosition2D();
            }

            if (Input.GetMouseButton(0))
            {

                Vector3 currentMousePosition = Utility.ReturnMousePosition2D();
                Vector3 lowerLeft = new Vector3
                    (
                        Mathf.Min(m_startPosition.x, currentMousePosition.x),
                        Mathf.Min(m_startPosition.y, currentMousePosition.y)
                    );

                Vector3 upperRight = new Vector3
                    (
                        Mathf.Max(m_startPosition.x, currentMousePosition.x),
                        Mathf.Max(m_startPosition.y, currentMousePosition.y)
                    );

                m_selectedAreaTransform.position = lowerLeft;
                m_selectedAreaTransform.localScale = upperRight - lowerLeft;
            }


            if (Input.GetMouseButtonUp(0))
            {
                m_selectedAreaTransform.gameObject.SetActive(false);

                Collider2D[] collider2DArray = Physics2D.OverlapAreaAll(m_startPosition, Utility.ReturnMousePosition2D());


                foreach (UnitRTS unitRTS in m_selectedUnits)
                {
                    unitRTS.SetSelectedVisible(false);
                    unitRTS.m_agent.ToggleLeader(false);
                }

                m_selectedUnits.Clear();
                foreach (Collider2D collider2D in collider2DArray)
                {
                    UnitRTS unitRTS = collider2D.GetComponent<UnitRTS>();

                    //if (collider2D == collider2DArray[0])
                    //{
                    //    unitRTS.m_agent.ToggleLeader(true);
                    //}

                    if (unitRTS != null && unitRTS.gameObject.tag == m_team.ToString())
                    {
                        unitRTS.SetSelectedVisible(true);
                        m_selectedUnits.Add(unitRTS);
                    }

                }
                Debug.Log(m_selectedUnits.Count);
            }


            if (Input.GetMouseButtonDown(1))
            {
                SetFormationPosition(Utility.ReturnMousePosition2D());
            }
        }



    }



    protected void SetFormationPosition(Vector3 t_destination)
    {
        Vector3 moveToPosition = t_destination;

        List<Vector3> targetPositionList = GetLinePositionList(moveToPosition);

        switch (m_formation)
        {
            case Formations.Line:
                targetPositionList = GetLinePositionList(moveToPosition);
                break;
            case Formations.Box:

                break;
            case Formations.Cheveron:

                break;
            default:
                targetPositionList = GetLinePositionList(moveToPosition);
                break;
        }

       

        int targetPositionListIndex = 0;

        foreach (UnitRTS unit in m_selectedUnits)
        {
            unit.m_agent.SetTargetPosition(targetPositionList[targetPositionListIndex]);
            targetPositionListIndex = (targetPositionListIndex + 1) % targetPositionList.Count;
        }

        //if (unit.m_agent.m_leader)
        //{
        //    unit.m_agent.SetTargetPosition();
        //}
    }

    protected List<Vector3> GetLinePositionList(Vector3 t_startPosition)
    {
        List<Vector3> positionList = new List<Vector3>();
        //positionList.Add(t_startPosition);

        float rowMax = Mathf.Ceil(m_selectedUnits.Count / 9f);

        for (int j = 0; j < rowMax; j++)
        {
            for (int i = 0; i < 9; i++)
            {
                //positionList.AddRange(GetLinePositionList(t_startPosition, new float[] {9}));

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

    private List<Vector3> GetBoxFormation(Vector3 t_startPosition)
    {
        List<Vector3> positionList = new List<Vector3>();
        //positionList.Add(t_startPosition);
        float perLine = m_selectedUnits.Count / 4;
        perLine = Mathf.Ceil(perLine);
        for (int j = 0; j < 4; j++)
        {
            for (int i = 0; i < perLine; i++)
            {
                if (perLine % 2 == 0)
                {

                }
            }
        }
        return positionList;
    }
}
