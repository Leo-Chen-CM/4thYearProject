using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    //public float m_productionRate = 3f;

    bool m_gamePaused;

    [SerializeField]
    GameObject m_pauseTextObject;

    [SerializeField]
    int m_team1Reserves;
    [SerializeField]
    int m_team2Reserves;

    [SerializeField]
    int m_team1Score;

    [SerializeField]
    int m_team2Score;

    [SerializeField]
    int m_reserveGeneration = 1;

    [SerializeField]
    float m_timer;

    [SerializeField]
    List<ControlPointController> m_controlPointControllers = new List<ControlPointController>();

    [SerializeField]
    List<ControlPointController> m_resourcePointControllers = new List<ControlPointController>();

    int m_team1ControlPoints = 0;
    int m_team2ControlPoints = 0;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;

        m_gamePaused = false;

        m_team1Score = 0;
        m_team2Score = 0;

        m_team1Reserves = 0;
        m_team2Reserves = 0;

        StartCoroutine(GenerateFunding1());
        StartCoroutine(GenerateFunding2());
        StartCoroutine(GenerateScore1());
        StartCoroutine(GenerateScore2());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            ToggleGamePause();
        }
    }


    //Need to check which control point belongs to who.
    public void Points()
    {
        foreach (ControlPointController controlPointController in m_controlPointControllers)
        {
            if (controlPointController.GetTeamAffiliation() == "Team1")
            {
                m_team1ControlPoints += 1;
            }
            else if (controlPointController.GetTeamAffiliation() == "Team2")
            {
                m_team1ControlPoints += 1;
            }
        }
    }

    

    private void ToggleGamePause()
    {
        m_gamePaused = !m_gamePaused;

        if (m_gamePaused)
        {
            Time.timeScale = 0;
            m_pauseTextObject.SetActive(m_gamePaused);
        }
        else
        {
            Time.timeScale = 1;
            m_pauseTextObject.SetActive(m_gamePaused);
        }
    }

    IEnumerator GenerateScore1()
    {
        while (true)
        {
            m_team1Score += m_team1ControlPoints;

            yield return new WaitForSeconds(m_timer);
        }
    }

    IEnumerator GenerateScore2()
    {
        while (true)
        {
            m_team2Score += m_team2ControlPoints;

            yield return new WaitForSeconds(m_timer);
        }
    }
    IEnumerator GenerateFunding1()
    {
        while (true) 
        {
            m_team1Reserves += m_reserveGeneration;

            if (m_team1Reserves > 1000)
            {
                m_team1Reserves = 1000;
            }

            yield return new WaitForSeconds(m_timer);
        }

    }
    IEnumerator GenerateFunding2()
    {
        while (true) 
        {
            m_team2Reserves += m_reserveGeneration;

            if (m_team2Reserves > 1000)
            {
                m_team2Reserves = 1000;
            }

            yield return new WaitForSeconds(m_timer);
        }

    }
}
