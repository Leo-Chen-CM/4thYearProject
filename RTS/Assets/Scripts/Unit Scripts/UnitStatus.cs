using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitStatus : MonoBehaviour
{
    [SerializeField]
    int m_health = 3;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Bullet")
        {
            if (m_health > 1)
            {
                m_health--;
            }
            else
            {
                Destroy(gameObject);
            }

        }
    }
}
