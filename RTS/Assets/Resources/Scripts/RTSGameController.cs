using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public enum Teams
{
    Team1,
    Team2
};

public enum Formations
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
    public List<BaseUnit> m_selectedUnits;
    // Update is called once per frame


    public Teams m_team = Teams.Team1;

    [SerializeField]
    Formations m_formation = Formations.Line;

    [SerializeField]
    private float m_offset;

    public bool m_AI = false;

    public TMP_Dropdown[] dropdowns;

    private void Awake()
    {
        m_selectedUnits = new List<BaseUnit>();
        m_selectedAreaTransform.gameObject.SetActive(false);
    }

    [SerializeField]
    TMP_Dropdown TeamSelector;

    [SerializeField]
    TMP_Dropdown FormationSelector;
    public void SetTeam()
    {
        if (TeamSelector.value == 0)
        {
            m_team = Teams.Team1;
        }

        if (TeamSelector.value == 1)
        {
            m_team = Teams.Team2;
        }
    }
    public void SetFormation()
    {
        if (FormationSelector.value == 0)
        {
            m_formation = Formations.Line;
        }
        else
        if (FormationSelector.value == 1)
        {
            m_formation = Formations.Box;
        }
        else
        if (FormationSelector.value == 2)
        {
            m_formation = Formations.Cheveron;
        }
    }

    public Formations GetCurrentFormation()
    {
        return m_formation;
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

        foreach (BaseUnit unit in m_selectedUnits)
        {
            unit.SetTargetPosition(targetPositionList[targetPositionListIndex]);
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

        for (int i = 0; i < rowMax; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                //positionList.AddRange(GetLinePositionList(t_startPosition, new float[] {9}));

                if (i % 2 != 0)
                {
                    Vector3 offset = new Vector3(t_startPosition.x + m_offset * i, t_startPosition.y - m_offset * j, 0);
                    positionList.Add(offset);
                }
                else
                {
                    Vector3 offset = new Vector3(t_startPosition.x - m_offset * i, t_startPosition.y - m_offset * j, 0);
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
