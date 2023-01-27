using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitFieldOfView : MonoBehaviour
{

    [SerializeField]
    float m_radius = 10f;
    [SerializeField]
    [Range(1, 360)] float m_angle = 45f;

    public LayerMask m_targetLayer;
    public LayerMask m_obstructionLayer;

    public GameObject m_entityRef;

    public bool m_enemySpotted { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        m_entityRef = GameObject.FindGameObjectWithTag("Player");
        StartCoroutine(FOVCheck());
    }

    private IEnumerator FOVCheck()
    {
        WaitForSeconds wait = new WaitForSeconds(0.2f);

        while (true)
        {
            yield return wait;
            FOV();
        }
    
    }

    private void FOV()
    {
        Collider2D[] rangeCheck = Physics2D.OverlapCircleAll(transform.position, m_radius, m_targetLayer);

        if (rangeCheck.Length > 0)
        {
            Transform target = rangeCheck[0].transform;
            Vector2 directionToTarget = (target.position - transform.position).normalized;

            if (Vector2.Angle(transform.up, directionToTarget) < m_angle / 2)
            {
                float distanceToTarget = Vector2.Distance(transform.position, target.position);

                if (!Physics2D.Raycast(transform.position, directionToTarget, distanceToTarget, m_obstructionLayer))
                {
                    m_enemySpotted = true;
                }
                else
                {
                    m_enemySpotted = false;
                }
            }
            else
            {
                m_enemySpotted = false;
            }
        }
        else if (m_enemySpotted)
        {
            m_enemySpotted = false;
        }

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        UnityEditor.Handles.DrawWireDisc(transform.position, Vector3.forward, m_radius);

        Vector3 angle01 = DirectionFromAngle(-transform.eulerAngles.z, -m_angle / 2);
        Vector3 angle02 = DirectionFromAngle(-transform.eulerAngles.z, m_angle / 2);


        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, transform.position + angle01 * m_radius);
        Gizmos.DrawLine(transform.position, transform.position + angle02 * m_radius);

        if (m_enemySpotted)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, m_entityRef.transform.position);
        }

    }





    private Vector2 DirectionFromAngle(float t_eulerY, float t_angleInDegrees)
    {
        t_angleInDegrees += t_eulerY;

        return new Vector2(Mathf.Sin(t_angleInDegrees * Mathf.Deg2Rad), Mathf.Cos(t_angleInDegrees * Mathf.Deg2Rad));
    }
}


