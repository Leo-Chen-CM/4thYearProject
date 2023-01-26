using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitRTS : MonoBehaviour
{
    private GameObject m_selectedGameObject;
    private UnitMovement m_movePosition;
    private void Awake()
    {
        m_selectedGameObject = transform.Find("Selected").gameObject;
        m_movePosition = GetComponent<UnitMovement>();
        SetSelectedVisible(false);
    }


    public void SetSelectedVisible(bool t_visible)
    {
        m_selectedGameObject.SetActive(t_visible);
    }

    public void MoveTo(Vector3 t_targetPosition)
    {
        m_movePosition.SetMovePosition(t_targetPosition);
    }
}
