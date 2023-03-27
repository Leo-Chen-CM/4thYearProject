using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapAreaCollider : MonoBehaviour
{
    private List<UnitMapAreas> m_unitMapAreas = new List<UnitMapAreas>();

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<UnitMapAreas>(out UnitMapAreas unitMapAreas))
        {
            m_unitMapAreas.Add(unitMapAreas);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent<UnitMapAreas>(out UnitMapAreas unitMapAreas))
        {
            m_unitMapAreas.Remove(unitMapAreas);
        }
    }

    public List<UnitMapAreas> GetUnitMapAreas() { return m_unitMapAreas; }
}
