using Newtonsoft.Json.Bson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEngine.GraphicsBuffer;

public class UnitMovement : MonoBehaviour
{

    [SerializeField]
    private float m_movementSpeed;
    [SerializeField]
    private float m_rotationSpeed;
    [SerializeField]
    private float m_rotationModifier;

    private Vector3 m_movePosition;
    private Rigidbody2D m_rigidbody2D;
    private Vector3 m_velocityVector;

    [SerializeField]
    private UnitFieldOfView UnitFieldOfView;
    private void Awake()
    {
        m_rigidbody2D= GetComponent<Rigidbody2D>();
        UnitFieldOfView= GetComponent<UnitFieldOfView>();
        m_movePosition = transform.position;   
    }


    public void SetMovePosition(Vector3 t_movePosition)
    {
        m_movePosition = t_movePosition;
        Vector3 moveDirection = (m_movePosition - transform.position).normalized;
        SetVelocity(moveDirection);
    }

    public void SetVelocity(Vector3 t_velocityVector)
    {
        m_velocityVector = t_velocityVector;
        m_rigidbody2D.velocity = m_velocityVector * m_movementSpeed;
    }

    private void Update()
    {
        if (Vector3.Distance(m_movePosition, transform.position) < 0.5f)
        {
            m_velocityVector = Vector3.zero;
            m_rigidbody2D.velocity = Vector3.zero;
            m_movePosition= transform.position;
        }
    }

    private void FixedUpdate()
    {
        if (Vector3.Distance(m_movePosition, transform.position) < 2f)
        {
            m_rigidbody2D.velocity = m_rigidbody2D.velocity * 0.9f;
        }

        if (m_movePosition != transform.position && !UnitFieldOfView.m_enemySpotted)
        {
             Vector3 vectorToTarget = m_movePosition - transform.position;
             float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg - m_rotationModifier;
             Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
            transform.rotation = Quaternion.Slerp(transform.rotation, q, Time.deltaTime * m_rotationSpeed);
        }
    }
}