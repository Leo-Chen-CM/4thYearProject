using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class ControlPointTrigger : MonoBehaviour
{
    private List<UnitRTS> m_unitsInControlPoint = new List<UnitRTS>();

    private void OnTriggerEnter2D(Collider2D collision)
    {
        m_unitsInControlPoint.Add(collision.gameObject.GetComponent<UnitRTS>());
        GameEventsManager.instance.ControlPointTriggerEnter(GetComponentInParent<ControlPointController>().id);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        m_unitsInControlPoint.Remove(collision.gameObject.GetComponent<UnitRTS>());
        GameEventsManager.instance.ControlPointTriggerEnter(GetComponentInParent<ControlPointController>().id);
    }

    public List<UnitRTS> GetUnitInControlPoint() { return m_unitsInControlPoint; }
}
