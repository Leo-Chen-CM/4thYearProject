using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField]
    int m_speed;
    [SerializeField]
    Rigidbody2D m_rigidbody2D;

    private void Start()
    {
        m_rigidbody2D.velocity= transform.up * m_speed;
        Destroy(gameObject, 5f);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Cover" || collision.gameObject.tag == "Wall")
        {
            Destroy(this);
        }
    }
}
