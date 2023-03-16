using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitRTS : MonoBehaviour
{
    private GameObject m_selectedGameObject;
    private GameObject m_viewVisualisation;
    //private UnitMovement m_movePosition;
    public Trooper m_agent;
    [SerializeField]
    int m_health = 3;
    private void Awake()
    {
        m_agent = GetComponent<Trooper>();
        m_selectedGameObject = transform.Find("Selected").gameObject;
        m_viewVisualisation = transform.Find("View Visualisation").gameObject;
        //m_movePosition = GetComponent<UnitMovement>();


        SetSelectedVisible(false);
    }
    public void SetSelectedVisible(bool t_visible)
    {
        m_selectedGameObject.SetActive(t_visible);
        m_viewVisualisation.SetActive(t_visible);
    }

    //public void MoveTo(Vector3 t_targetPosition)
    //{
    //    m_movePosition.SetMovePosition(t_targetPosition);
    //}

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


    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    if (collision.gameObject.tag == "Bullet")
    //    {
    //        if (m_health > 1)
    //        {
    //            m_health--;
    //            Destroy(collision.gameObject);
    //        }
    //        else
    //        {
    //            Destroy(gameObject);
    //        }

    //    }
    //}

    public void LoseHealth()
    {
        if (m_health > 1)
        {
            m_health--;
        }
        else
        {
            m_health--;
            Destroy(gameObject);
        }
    }
}
