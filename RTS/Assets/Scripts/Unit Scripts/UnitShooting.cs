using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitShooting : MonoBehaviour
{
    //[SerializeField]
    //UnitFieldOfView m_unitFieldOfView;
    [SerializeField]
    GameObject m_bullet;
    [SerializeField]
    Transform m_firingPoint;
    private float nextFire = 0f;
    public float fireRate;

    public void ShootBullet()
    {
        if (Time.time > nextFire)
        {
            nextFire = Time.time + fireRate;
            Instantiate(m_bullet, m_firingPoint.position, m_firingPoint.transform.rotation);
        }
    }
}
