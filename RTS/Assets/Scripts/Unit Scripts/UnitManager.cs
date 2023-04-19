using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    public List<BaseUnit> m_selectedUnits = new List<BaseUnit>();
    public List<GroupLeader> m_selectedGroupLeaders = new List<GroupLeader>();

    // Groups
    const int maxGroups = 9;
    public bool[] groups = new bool[maxGroups];
    // -------------------------
    // GETTERS & SETTERS
    // -------------------------

    public int GetMaxGroupCount()
    {
        return maxGroups;
    }
}
