using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public float m_productionRate = 3f;

    bool m_gamePaused;

    [SerializeField]
    GameObject m_pauseTextObject;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;

        m_gamePaused = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            ToggleGamePause();
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
}
