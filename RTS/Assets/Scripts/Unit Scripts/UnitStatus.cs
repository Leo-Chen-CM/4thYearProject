using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class UnitStatus : MonoBehaviour
{
    [SerializeField]
    int m_health = 3;

    //public Vector3 m_targetPosition;
    //private delegate void OnReachTarget();
    //private OnReachTarget m_onReachTarget;

    public void SetupTeam(string t_teamTag)
    {
        gameObject.tag = t_teamTag;
        if (gameObject.tag == "Team1")
        {
            gameObject.GetComponent<SpriteRenderer>().color = Color.red;
        }

        if (gameObject.tag == "Team2")
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
