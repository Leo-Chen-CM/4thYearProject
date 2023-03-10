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
    public float fireRate;

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
        m_lineRenderer.enabled = false;
    }

    public void Shoot()
    {
        if (Time.time > nextFire)
        {
            nextFire = Time.time + fireRate;
            if (Physics2D.Raycast(transform.position, transform.up))
            {
                RaycastHit2D hit = Physics2D.Raycast(m_firingPoint.position, transform.up);
                //Draw2DRay(m_firingPoint.position, hit.point);

                StartCoroutine(ShootLaser(m_firingPoint.position, hit.point));



                if (hit.transform.gameObject.GetComponent<UnitRTS>())
                {
                    hit.transform.gameObject.GetComponent<UnitRTS>().LoseHealth();
                }
                else
                {
                    Debug.Log("Target hit");
                }
            }
            //Instantiate(m_bullet, m_firingPoint.position, m_firingPoint.transform.rotation);
        }
    }

    IEnumerator ShootLaser(Vector2 startPos, Vector2 endPos)
    {
        m_lineRenderer.enabled = true;

        //Color c = m_lineRenderer.material.color;
        //c.a = 1;
        //m_lineRenderer.material.color = c;
        m_lineRenderer.SetPosition(0, startPos);
        m_lineRenderer.SetPosition(1, endPos);
        StartCoroutine(Fade());
        yield return new WaitForSeconds(1f);
        m_lineRenderer.enabled = false;
    }

    IEnumerator Fade()
    {
        Color c = m_lineRenderer.material.color;
        for (float alpha = 1f; alpha >= 0; alpha -= 0.1f)
        {
            c.a = alpha;
            m_lineRenderer.material.color = c;
            yield return new WaitForSeconds(.01f);
        }
    }

    void Draw2DRay(Vector2 startPos, Vector2 endPos)
    {
        m_lineRenderer.SetPosition(0, startPos);
        m_lineRenderer.SetPosition(1, endPos);
    }

}
