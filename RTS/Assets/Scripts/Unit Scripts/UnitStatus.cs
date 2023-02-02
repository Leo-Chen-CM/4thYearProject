using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitStatus : MonoBehaviour
{
    [SerializeField]
    int m_health = 3;

    private void Start()
    {
        if (gameObject.tag == "Team 1")
        {
            gameObject.GetComponent<SpriteRenderer>().color = Color.red;
        }

        if (gameObject.tag == "Team 2")
        {
            gameObject.GetComponent<SpriteRenderer>().color = Color.blue;
        }
    }



    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Bullet")
        {
            if (m_health > 1)
            {
                m_health--;
                Destroy(collision.gameObject);
            }
            else
            {
                Destroy(gameObject);
            }

        }
    }
}
