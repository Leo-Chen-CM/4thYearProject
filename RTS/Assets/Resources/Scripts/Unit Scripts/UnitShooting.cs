using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class UnitShooting : MonoBehaviour
{
    //[SerializeField]
    //UnitFieldOfView m_unitFieldOfView;
    [SerializeField]
    Transform m_firingPoint;
    private float nextFire = 0f;
    [SerializeField]
    private LineRenderer m_lineRenderer;
    public float fireRate;

    [SerializeField]
    private float m_offset;

    [SerializeField]
    LayerMask m_layerMask;
    private void Start()
    {
        if (gameObject.tag == "Team1")
        {
            m_lineRenderer.material.color = Color.blue;
        }
        
        if (gameObject.tag == "Team2")
        {
            m_lineRenderer.material.color = Color.red;
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
                Vector3 offset = new Vector3(Random.Range(-m_offset, m_offset), 0, 0);

                RaycastHit2D hit = Physics2D.Raycast(m_firingPoint.position, transform.up + offset, 200.0f, m_layerMask);



                if (hit.transform.gameObject.TryGetComponent(out BaseUnit unit))
                {
                    hit.transform.gameObject.GetComponent<BaseUnit>().LoseHealth();
                }
                else if (hit.transform.gameObject.name == "Dummy")
                {
                    Debug.Log("Dummy hit");
                }
                else if (hit == false)
                {
                    Debug.Log("Laser shot");
                }

                StartCoroutine(ShootLaser(m_firingPoint.position, hit.point));
            }
        }
    }

    IEnumerator ShootLaser(Vector2 startPos, Vector2 endPos)
    {
        m_lineRenderer.enabled = true;
        m_lineRenderer.SetPosition(0, startPos);
        m_lineRenderer.SetPosition(1, endPos);
        yield return new WaitForSeconds(0.1f);
        m_lineRenderer.enabled = false;
    }
}
