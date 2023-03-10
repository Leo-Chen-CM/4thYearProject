using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class UnitShooting : MonoBehaviour
{
    //[SerializeField]
    //UnitFieldOfView m_unitFieldOfView;
    [SerializeField]
    GameObject m_bullet;
    [SerializeField]
    Transform m_firingPoint;
    private float nextFire = 0f;
    [SerializeField]
    private LineRenderer m_lineRenderer;

    private void Start()
    {
        if (gameObject.tag == "Team1")
        {
            m_lineRenderer.material.color = Color.red;
        }
        
        if (gameObject.tag == "Team2")
        {
            m_lineRenderer.material.color = Color.blue;
        }

    }

    public void Shoot()
    {
        if (Time.time > nextFire)
        {
            if (Physics2D.Raycast(transform.position, transform.up))
            {
                RaycastHit2D hit = Physics2D.Raycast(m_firingPoint.position, transform.up);
                Draw2DRay(m_firingPoint.position, hit.point);
            }
            else
            {
                Draw2DRay(m_firingPoint.position, m_firingPoint.transform.up * 15);
            }
            //Instantiate(m_bullet, m_firingPoint.position, m_firingPoint.transform.rotation);
        }
    }

    

    void Draw2DRay(Vector2 startPos, Vector2 endPos)
    {
        m_lineRenderer.SetPosition(0, startPos);
        m_lineRenderer.SetPosition(1, endPos);
    }

}
