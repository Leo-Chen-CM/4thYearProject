using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class MapAreaCollider : MonoBehaviour
{
    public event EventHandler OnUnitEnter;
    public event EventHandler OnUnitExit;

    private List<UnitRTS> m_unitRTS = new List<UnitRTS>();

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<UnitRTS>(out UnitRTS unitMapAreas))
        {
            m_unitRTS.Add(unitMapAreas);
            OnUnitEnter?.Invoke(this, EventArgs.Empty);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent<UnitRTS>(out UnitRTS unitMapAreas))
        {
            m_unitRTS.Remove(unitMapAreas);
            OnUnitExit?.Invoke(this, EventArgs.Empty);
        }
    }

    public List<UnitRTS> GetUnitMapAreas() { return m_unitRTS; }
}
