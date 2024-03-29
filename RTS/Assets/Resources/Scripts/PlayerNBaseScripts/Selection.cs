using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;

public class Selection : MonoBehaviour
{
    protected UnitManager m_unitManager;
    protected Vector3 m_startPosition;
    [SerializeField]
    GameObject m_GroupLeaderPrefab;

    RTSGameController m_controller;

    private void Start()
    {
        m_unitManager = GetComponent<UnitManager>();
        m_controller = GetComponent<RTSGameController>();
    }
    /// <summary>
    /// Selects all units in the overlap area
    /// </summary>
    protected void SelectUnits()
    {

        Collider2D[] collider2DArray = Physics2D.OverlapAreaAll(m_startPosition, Utility.ReturnMousePosition2D());

        if (collider2DArray.Length > 0)
        {

            foreach (Collider2D collider2D in collider2DArray)
            {
                BaseUnit unitRTS = collider2D.GetComponent<BaseUnit>();

                if (unitRTS == null || unitRTS.gameObject.tag != m_controller.m_team.ToString())
                {
                    continue;
                }

                GroupLeader leader = unitRTS.GetLeader();
                //Has no group then keep it loose
                if (leader == null)
                {
                    if (!m_unitManager.m_selectedUnits.Contains(unitRTS))
                    {
                        m_unitManager.m_selectedUnits.Add(unitRTS);
                        unitRTS.SetSelectedVisible(true);
                    }
                }
                //Has a group so get everyone from that group
                else
                {
                    if (m_unitManager.m_selectedGroupLeaders.Contains(leader))
                        continue;

                    foreach (BaseUnit unit in leader.units)
                    {
                        unit.SetSelectedVisible(true);
                    }
                    m_unitManager.m_selectedGroupLeaders.Add(leader);
                }
            }

        }
        else
        {
            ClearSelection();
        }

    }

    // Move all selected units to target
    protected void MoveSelection(Vector3 target)
    {
        // Loop over all agents
        foreach (BaseUnit unit in m_unitManager.m_selectedUnits)
        {
            // Set a new target
            unit.SetTargetPosition(target);
        }

        // Loop over all leaders
        foreach (GroupLeader leader in m_unitManager.m_selectedGroupLeaders)
        {
            // Set a new target
            leader.SetTarget(target);
        }
    }
    void ClearSelection()
    {
        if (m_unitManager.m_selectedUnits.Count == 0 && m_unitManager.m_selectedGroupLeaders.Count == 0)
        {
            return;
        }

        foreach (BaseUnit unit in m_unitManager.m_selectedUnits)
        {
            unit.SetSelectedVisible(false);
        }
        m_unitManager.m_selectedUnits.Clear();

        foreach (GroupLeader leader in m_unitManager.m_selectedGroupLeaders)
        {
            // Loop over all units in group
            foreach (BaseUnit unit in leader.units)
            {
                unit.SetSelectedVisible(false);
            }
        }

        m_unitManager.m_selectedGroupLeaders.Clear();
    }

    protected void GroupSelection()
    {
        // Only execute when we have a selection
        if (m_unitManager.m_selectedUnits.Count == 0 && m_unitManager.m_selectedGroupLeaders.Count == 0)
            return;

        bool newLeader = false;
        if (m_unitManager.m_selectedGroupLeaders.Count == 0 || m_unitManager.m_selectedGroupLeaders.Count >= 2)
            newLeader = true;

        // Set invalid index
        int freeIndex = -1;


        if (newLeader)
        {
            for (int index = 0; index < m_unitManager.groups.Length; index++)
            {
                // Get the first free index
                if (m_unitManager.groups[index] == false)
                {
                    freeIndex = index;
                    break;
                }
            }
        }
        else
        {
            freeIndex = m_unitManager.m_selectedGroupLeaders[0].groupID;
        }
        // Do not execute if we don't have a free index
        if (freeIndex == -1)
        {
            Debug.LogError("Group limit reached.");
            return;
        }

        // Set minimum and maximum to opposite values
        Vector3 minimum = new Vector3(Mathf.Infinity, Mathf.Infinity, Mathf.Infinity);
        Vector3 maximum = new Vector3(-Mathf.Infinity, -Mathf.Infinity, -Mathf.Infinity);

        // Loop over all units
        foreach (BaseUnit unit in m_unitManager.m_selectedUnits)
        {
            Vector3 unitPosition = unit.transform.position;

            if (unitPosition.x < minimum.x)
                minimum.x = unitPosition.x;
            if (unitPosition.x > maximum.x)
                maximum.x = unitPosition.x;

            if (unitPosition.y < minimum.y)
                minimum.y = unitPosition.y;
            if (unitPosition.y > maximum.y)
                maximum.y = unitPosition.y;

            if (unitPosition.z < minimum.z)
                minimum.z = unitPosition.z;
            if (unitPosition.z > maximum.z)
                maximum.z = unitPosition.z;
        }

        Vector3 middle = (minimum + maximum) / 2f;

        GroupLeader leader;
        if (newLeader)
        {
            // Instantiate a leader in middle of furthest units
            GameObject go = Instantiate(m_GroupLeaderPrefab, middle, Quaternion.identity);
            leader = go.GetComponent<GroupLeader>();

            // Assign group ID
            leader.groupID = freeIndex;
            m_unitManager.groups[freeIndex] = true;
        }
        else
        {
            leader = m_unitManager.m_selectedGroupLeaders[0];
            //leader.SetLocation(middle);
            //leader.units.Clear();
        }

        // Loop over all units
        foreach (BaseUnit unit in m_unitManager.m_selectedUnits)
        {
            // Add to leader list of units, set unit leader and material to group color
            leader.units.Add(unit);
            unit.AssignLeader(leader);
        }


        leader.SetFormation(m_controller.GetCurrentFormation());

        if (newLeader)
        {
            // Add leader to selected leaders
            m_unitManager.m_selectedGroupLeaders.Add(leader);
        }

        m_unitManager.m_selectedUnits.Clear();
    }
}
