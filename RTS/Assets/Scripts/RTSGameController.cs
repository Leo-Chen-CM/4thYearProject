using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
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
    public List<BaseUnit> m_selectedUnits;
    // Update is called once per frame

    [SerializeField] private GameObject m_FormationPointPrefab = null;

    [SerializeField]
    Teams m_team = Teams.Team1;

    [SerializeField]
    Formations m_formation = Formations.Line;

    [SerializeField]
    private float m_spacing;

    [SerializeField]
    private float m_lineSpacing;

    public bool m_AI = false;

    private void HandleChange(TMP_Dropdown dropdown, int newIndex)
    {
        if (newIndex == 0)
        {
            if (dropdown.value == 0)
            {
                m_team = Teams.Team1;
            }
            if (dropdown.value == 1)
            {
                m_team = Teams.Team2;
            }
        }

        if (newIndex == 1)
        {
            if (dropdown.value == 0)
            {
                m_formation = Formations.Line;
            }
            if (dropdown.value == 1)
            {
                m_formation = Formations.Box;
            }
            if (dropdown.value == 2)
            {
                m_formation = Formations.Cheveron;
            }
        }

    }



    private void Awake()
    {
        m_selectedUnits = new List<BaseUnit>();
        m_selectedAreaTransform.gameObject.SetActive(false);
    }

    public TMP_Dropdown[] dropdowns;

    //public void HandleInputData(int val)
    //{

    //    if (val == 0)
    //    {
    //        m_team = Teams.Team1;
    //    }        
    //    if (val == 1)
    //    {
    //        m_team = Teams.Team2;
    //    }
    //}

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


                foreach (BaseUnit unitRTS in m_selectedUnits)
                {
                    unitRTS.SetSelectedVisible(false);
                    //unitRTS.m_agent.ToggleLeader(false);
                }

                m_selectedUnits.Clear();
                foreach (Collider2D collider2D in collider2DArray)
                {
                    BaseUnit unitRTS = collider2D.GetComponent<BaseUnit>();

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
                //Debug.Log(m_selectedUnits.Count);
            }


            if (Input.GetMouseButtonDown(1))
            {
                SetFormationPosition(Utility.ReturnMousePosition2D());
            }
        }



    }



    protected void SetFormationPosition(Vector3 t_destination)
    {
        List<Vector3> targetPositionList = new List<Vector3>();

        switch (m_formation)
        {
            case Formations.Line:
                targetPositionList = GetLinePositionList(t_destination);
                break;
            case Formations.Box:
                targetPositionList = GetBoxFormation(t_destination);
                break;
            case Formations.Cheveron:

                break;
            default:
                targetPositionList = GetLinePositionList(t_destination);
                break;
        }

       

        //int targetPositionListIndex = 0;

        //foreach (BaseUnit unit in m_selectedUnits)
        //{
        //    //if (unit == m_selectedUnits[0])
        //    //{
        //    //    unit.m_agent.SetTargetPosition(t_destination);
        //    //    targetPositionListIndex = (targetPositionListIndex + 1) % targetPositionList.Count;
        //    //}
        //    //else
        //    //{
        //        unit.SetTargetPosition(targetPositionList[targetPositionListIndex]);
        //        targetPositionListIndex = (targetPositionListIndex + 1) % targetPositionList.Count;
        //    //}

        //}

        //if (unit.m_agent.m_leader)
        //{
        //    unit.m_agent.SetTargetPosition();
        //}
    }

    protected List<Vector3> GetLinePositionList(Vector3 t_startPosition)
    {
        List<Vector3> positionList = new List<Vector3>();

        // Store unit count
        int amountUnits = m_selectedUnits.Count;
        // Multiple rows
        int minimumRowSize = 9;
        int rows = 1;

        if (amountUnits > minimumRowSize)
        {
            rows = ((amountUnits - 1) / minimumRowSize) + 1;
        }

        // Define units per row
        int unitsPerRow = ((amountUnits - 1) / rows) + 1;

        float spacing = 5;

        // Set start position
        Vector2 bottomLeft = new Vector2(-spacing * (unitsPerRow / 2f) + (spacing / 2f), spacing * (rows / 2f) - (spacing / 2f));

        Vector3 currentPosition = Vector3.zero;
        currentPosition.x = bottomLeft.x;
        currentPosition.y = bottomLeft.y;

        // Loop to create formation
        int currentRow = 1;
        for (int index = 0; index < amountUnits; index++)
        {
            // Check if last row has a different size than other rows
            if ((rows > 1) && (currentRow == rows) && (index % unitsPerRow == 0))
            {
                int unitsLastRow = amountUnits - (unitsPerRow * (rows - 1));

                int unitDifference = unitsPerRow - unitsLastRow;
                if (unitDifference > 0)
                {
                    currentPosition.x += spacing * (unitDifference / 2f);
                }
            }

            // If we have transform already, set new position
            if (positionList.Count > index)
            {
                positionList[index] = currentPosition;
            }
            else
            {
                // Instantiate a point parented to the leader, with a relative position
                GameObject go = Instantiate(m_FormationPointPrefab, transform);
                go.transform.localPosition = currentPosition;
                positionList.Add(go.transform.position);
            }

            if (((index + 1) % unitsPerRow) == 0)
            {
                // Update Y position and reset X
                currentPosition.y -= spacing;
                currentPosition.x = bottomLeft.x;
                currentRow++;
            }
            else
            {
                // Update X position
                currentPosition.x += spacing;
            }
        }

        return positionList;
    }

    private List<Vector3> GetBoxFormation(Vector3 t_startPosition)
    {
        List<Vector3> positionList = new List<Vector3>();

        // Calculate number of units per side of box formation
        int numUnitsPerSide = Mathf.CeilToInt(Mathf.Sqrt(m_selectedUnits.Count)) / 2;

        // Set positions and rotations of units
        int unitIndex = 0;

        for (int i = 0; i < m_selectedUnits.Count; i++)
        {
            Transform unit = m_selectedUnits[i].transform;

            if (unitIndex < numUnitsPerSide ||
                unitIndex >= m_selectedUnits.Count - numUnitsPerSide ||
                unitIndex % numUnitsPerSide == 0 ||
                (unitIndex + 1) % numUnitsPerSide == 0)
            {
                // Calculate position of unit based on formation size
                float x = (unitIndex % numUnitsPerSide) * m_spacing - (numUnitsPerSide - 1) * m_spacing / 2f;
                float y = (unitIndex / numUnitsPerSide) * m_spacing - (numUnitsPerSide - 1) * m_spacing / 2f;

                Vector3 offset = new Vector3(x, y, 0f);

                // Set position and rotation of unit
                unit.position = m_selectedUnits[0].transform.position + offset;

                positionList.Add(unit.position);
            }

            unitIndex++;
        }

        return positionList;
    }
}
