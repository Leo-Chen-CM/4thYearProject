using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class ControlPointTrigger : MonoBehaviour
{
    private List<BaseUnit> m_unitsInControlPoint = new List<BaseUnit>();

    private void OnTriggerEnter2D(Collider2D collision)
    {
        m_unitsInControlPoint.Add(collision.gameObject.GetComponent<BaseUnit>());
        GameEventsManager.instance.ControlPointTriggerEnter(GetComponentInParent<ControlPointController>().id);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        m_unitsInControlPoint.Remove(collision.gameObject.GetComponent<BaseUnit>());
        GameEventsManager.instance.ControlPointTriggerEnter(GetComponentInParent<ControlPointController>().id);
    }

    public List<BaseUnit> GetUnitInControlPoint() { return m_unitsInControlPoint; }
}
