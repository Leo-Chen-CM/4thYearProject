using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class GameManager : MonoBehaviour
{
    //Singleton instance of game manager
    public static GameManager instance;

    [Header("Game Attributes")]
    bool m_gamePaused; //Checks if the game is paused or not

    [SerializeField]
    int m_maxResources; //Maximum resources a player can have at a time.

    [SerializeField]
    GameObject m_pauseTextObject;//Gameobject that holds the pause text
    [SerializeField]
    int m_reserveGeneration = 1;//Passive amount of resources generated

    public bool m_debugMode;//Debug mode showing unit formation points

    public List<ControlPointController> m_controlPointControllers = new List<ControlPointController>();//List of all control points

    public List<ControlPointController> m_resourcePointControllers = new List<ControlPointController>();//List of all resource points

    [SerializeField]
    float m_generationTimer;//Time it takes to generate score or funding

    [SerializeField]
    [Range(0,2)] float m_timeScale;//Manages how fast the game is running

    //Attributes for Team 1
    [Header("Team 1 Attributes")]
    public int m_team1Reserves;
    public int m_team1Score;

    public List<ControlPointController> m_team1ControlledPoints = new List<ControlPointController>();

    public List<ControlPointController> m_team1ControlledResources = new List<ControlPointController>();

    [SerializeField]
    TextMeshProUGUI m_team1ScoreText;
    [SerializeField]
    TextMeshProUGUI m_team1ResourceText;


    //Attributes for Team 2
    [Header("Team 2 Attributes")]

    public int m_team2Reserves;

    public int m_team2Score;

    public List<ControlPointController> m_team2ControlledPoints = new List<ControlPointController>();

    public List<ControlPointController> m_team2ControlledResources = new List<ControlPointController>();

    [SerializeField]
    TextMeshProUGUI m_team2ScoreText;
    [SerializeField]
    TextMeshProUGUI m_team2ResourceText;
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

        if (Input.GetKeyDown(KeyCode.O))
        {
            m_debugMode = !m_debugMode;
        }

        Time.timeScale = m_timeScale;
    }


    //Need to check which control point belongs to who.
    public void PointsCaptured()
    {
        foreach (ControlPointController controlPointController in m_controlPointControllers)
        {
            if (controlPointController.GetTeamAffiliation() == "Team1" && !m_team1ControlledPoints.Contains(controlPointController))
            {
                m_team1ControlledPoints.Add(controlPointController);
            }
            else if (controlPointController.GetTeamAffiliation() == "Team2" && !m_team2ControlledPoints.Contains(controlPointController))
            {
                m_team2ControlledPoints.Add(controlPointController);
            }
        }

        foreach (ControlPointController resourcePointController in m_resourcePointControllers)
        {
            if (resourcePointController.GetTeamAffiliation() == "Team1" && !m_team1ControlledResources.Contains(resourcePointController))
            {
                m_team1ControlledResources.Add(resourcePointController);
            }
            else if (resourcePointController.GetTeamAffiliation() == "Team2" && !m_team2ControlledResources.Contains(resourcePointController))
            {
                m_team2ControlledResources.Add(resourcePointController);
            }
        }
    }

    
    //Need to check which control points were lost.
    public void PointsLost()
    {
        foreach (ControlPointController controlPointController in m_controlPointControllers)
        {
            if (controlPointController.GetTeamAffiliation() != "Team1")
            {
                m_team1ControlledPoints.Remove(controlPointController);
            }
            else if (controlPointController.GetTeamAffiliation() != "Team2")
            {
                m_team2ControlledPoints.Remove(controlPointController);
            }
        }

        foreach (ControlPointController resourcePointController in m_resourcePointControllers)
        {
            if (resourcePointController.GetTeamAffiliation() != "Team1")
            {
                m_team1ControlledResources.Remove(resourcePointController);
            }
            else if (resourcePointController.GetTeamAffiliation() != "Team2")
            {
                m_team2ControlledResources.Remove(resourcePointController);
            }
        }
    }

    
    /// <summary>
    /// Pauses the game when called
    /// </summary>
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

    //Generates Team 1's score
    IEnumerator GenerateScore1()
    {
        while (true)
        {
            m_team1Score += m_team1ControlledPoints.Count;
            m_team1ScoreText.text = "Team 1 Score: " + m_team1Score;
            yield return new WaitForSeconds(m_generationTimer);
        }
    }
    //Generates Team 2's score
    IEnumerator GenerateScore2()
    {
        while (true)
        {
            m_team2Score += m_team2ControlledPoints.Count;
            m_team2ScoreText.text = "Team 2 Score: " + m_team2Score;
            yield return new WaitForSeconds(m_generationTimer);
        }
    }

    //Generates Team 1's resources/funding
    IEnumerator GenerateFunding1()
    {
        while (true) 
        {
            m_team1Reserves += m_reserveGeneration + m_team1ControlledResources.Count;

            if (m_team1Reserves > m_maxResources)
            {
                m_team1Reserves = m_maxResources;
            }

            m_team1ResourceText.text = "Team 1 Resources: " + m_team1Reserves;

            yield return new WaitForSeconds(m_generationTimer);
        }

    }

    //Generates Team 2's resources/funding
    IEnumerator GenerateFunding2()
    {
        while (true) 
        {
            m_team2Reserves += m_reserveGeneration + m_team2ControlledResources.Count;

            if (m_team2Reserves > m_maxResources)
            {
                m_team2Reserves = m_maxResources;
            }

            m_team2ResourceText.text = "Team 2 Resources: " + m_team2Reserves;

            yield return new WaitForSeconds(m_generationTimer);
        }

    }
}
