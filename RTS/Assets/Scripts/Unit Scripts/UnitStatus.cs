using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitStatus : MonoBehaviour
{
    [SerializeField]
    int m_health = 3;
    [SerializeField]
    int m_teamNumber = 0;

    private void Start()
    {
        if (gameObject.tag == "Team 1")
        {
            m_teamNumber = 1;
            gameObject.GetComponent<SpriteRenderer>().color = Color.red;
        }
    }

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
