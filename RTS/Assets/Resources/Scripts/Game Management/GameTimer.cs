using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameTimer : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI m_timerText;
    public float m_timeLeft;
    public bool m_timerOn = true;

    /// <summary>
    /// Updates the timer by subtracting the time left with Time.deltatime;
    /// </summary>
    private void Update()
    {
        if (m_timerOn)
        {
            if (m_timeLeft > 0)
            {
                m_timeLeft -= Time.deltaTime;

            }
            else
            {
                m_timeLeft = 0;
                m_timerOn = false;
            }
            UpdateTimerText(m_timeLeft);
        }
    }

    /// <summary>
    /// Updates the text
    /// </summary>
    /// <param name="t_currentTime">Gets the current time passed</param>
    void UpdateTimerText(float t_currentTime)
    {
        t_currentTime += 1;

        float minutes = Mathf.FloorToInt(t_currentTime / 60);
        float seconds = Mathf.FloorToInt(t_currentTime % 60);

        m_timerText.text = "Time: " + string.Format("{0:00}:{1:00}", minutes, seconds);
    }

}
