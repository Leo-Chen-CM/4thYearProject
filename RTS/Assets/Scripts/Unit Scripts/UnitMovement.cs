using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitMovement : MonoBehaviour
{

    [SerializeField]
    private float m_movementSpeed;

    private Vector3 m_movePosition;
    private Rigidbody2D m_rigidbody2D;
    private Vector3 m_velocityVector;
    private void Awake()
    {
        m_rigidbody2D= GetComponent<Rigidbody2D>();
        m_movePosition = transform.position;   
    }


    public void SetMovePosition(Vector3 t_movePosition)
    {
        this.m_movePosition = t_movePosition;
    }

    public void SetVelocity(Vector3 t_velocityVector)
    {
        this.m_velocityVector = t_velocityVector;
    }

    private void Update()
    {
        Vector3 moveDirection = (m_movePosition - transform.position).normalized;
        SetVelocity(moveDirection);
        if (Vector3.Distance(m_movePosition, transform.position) < 1f)
        {
            moveDirection = Vector3.zero;
            SetVelocity(moveDirection);
        }
    }

    private void FixedUpdate()
    {
        m_rigidbody2D.velocity = m_velocityVector * m_movementSpeed; 
    }
}
