using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Adapted from EezehDev Coordinated Formations
/// </summary>
public class PlayerSelection : Selection
{
    [SerializeField]
    private Transform m_selectedAreaTransform;

    // Update is called once per frame
    void Update()
    {
        //Checks if any units selected has died and removes them from the list
        for (int i = 0; i < m_unitManager.m_selectedUnits.Count; i++)
        {
            if (m_unitManager.m_selectedUnits[i] == null)
            {
                m_unitManager.m_selectedUnits.Remove(m_unitManager.m_selectedUnits[i]);
            }
        }

        //Enables the selection area gameobject and assigns the starting position of the transform to the first instance of where the mouse was.
        if (Input.GetMouseButtonDown(0))
        {
            m_selectedAreaTransform.gameObject.SetActive(true);
            m_startPosition = Utility.ReturnMousePosition2D();
        }

        //When dragging the mouse around, the transform scales based on the distance from the starting position to where the mouse is.
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

            SelectUnits();
            GroupSelection();
        }


        if (Input.GetMouseButtonDown(1))
        {
            MoveSelection(Utility.ReturnMousePosition2D());
        }
    }
}
