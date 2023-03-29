using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class MapAreaCollider : MonoBehaviour
{
    public event EventHandler OnUnitEnter;
    public event EventHandler OnUnitExit;

    private List<UnitMapAreas> m_unitMapAreas = new List<UnitMapAreas>();

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<UnitMapAreas>(out UnitMapAreas unitMapAreas))
        {
            m_unitMapAreas.Add(unitMapAreas);
            OnUnitEnter?.Invoke(this, EventArgs.Empty);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent<UnitMapAreas>(out UnitMapAreas unitMapAreas))
        {
            m_unitMapAreas.Remove(unitMapAreas);
            OnUnitExit?.Invoke(this, EventArgs.Empty);
        }
    }

    public List<UnitMapAreas> GetUnitMapAreas() { return m_unitMapAreas; }
}
