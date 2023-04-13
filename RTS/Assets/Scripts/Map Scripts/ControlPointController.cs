using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class ControlPointController : MonoBehaviour
{
    public enum CaptureStates
    {
        Neutral,
        Capturing,
        Captured,
        Contested,
        Reverting,
        RevertingBackToOtherTeam,
    }

    public int id;
    private float m_progress;
    float m_progressSpeed = 0.2f;
    [SerializeField]
    string m_teamAffiliation;
    [SerializeField]
    CaptureStates m_captureState;

    [SerializeField]
    List<UnitRTS> m_unitInsideControlPointList = new List<UnitRTS>();
    ControlPointTrigger m_controlPointTrigger;

    bool m_previouslyOwned;
    bool m_capturedStatus;

    [SerializeField]
    Color m_controlPointColor;
    private void Start()
    {
        m_controlPointColor = Color.white;
        m_progress = 0f;
        m_controlPointTrigger = GetComponentInChildren<ControlPointTrigger>();
        m_teamAffiliation = string.Empty;

        GameEventsManager.instance.OnControlPointTriggerEnter += OnControlPointEnter;
        GameEventsManager.instance.OnControlPointTriggerExit += OnControlPointExit;
        GameEventsManager.instance.OnControlPointCapture+= OnControlPointCapture;
        GameEventsManager.instance.OnControlPointLoss+= OnControlPointLoss;
        m_captureState = CaptureStates.Neutral;
    }

    private void OnControlPointEnter(int t_id)
    {
        //if (t_id == id)
        //{
        //    Debug.Log("Units in control point " + id + ": " + m_unitInsideControlPointList.Count);
        //}
    }
    private void OnControlPointExit(int t_id)
    {
        //if (t_id == id)
        //{
        //    Debug.Log("Units in control point " + id + ": " + m_unitInsideControlPointList.Count);
        //}
    }

    private void OnControlPointCapture(int t_id)
    {
        if (t_id == id)
        {
            Debug.Log("Control point " + id + " was captured by " + m_teamAffiliation);
            GameManager.instance.PointsCaptured();
        }
    }
    private void OnControlPointLoss(int t_id)
    {
        if (t_id == id)
        {
            Debug.Log("Control point " + id + " was lost");
            GameManager.instance.PointsLost();
        }
    }

    // Update is called once per frame
    void Update()
    {
        m_unitInsideControlPointList.Clear();

        //Checks if any of them are null
        for (int i = 0; i < m_controlPointTrigger.GetUnitInControlPoint().Count; i++)
        {
            if (m_controlPointTrigger.GetUnitInControlPoint()[i] == null)
            {
                m_controlPointTrigger.GetUnitInControlPoint().RemoveAt(i);
            }
        }

        if (m_controlPointTrigger.GetUnitInControlPoint().Count > 0)
        {
            foreach (UnitRTS unit in m_controlPointTrigger.GetUnitInControlPoint())
            {

                if (!m_unitInsideControlPointList.Contains(unit))
                {
                    m_unitInsideControlPointList.Add(unit);
                }
            }
        }


        switch (m_captureState)
        {
            case CaptureStates.Neutral:
                if (m_unitInsideControlPointList.Count > 0)
                {
                    m_captureState = CaptureStates.Capturing;
                    m_teamAffiliation = m_unitInsideControlPointList[0].gameObject.tag;
                    setColor(m_unitInsideControlPointList[0].gameObject.GetComponent<SpriteRenderer>().color);
                }
                break;
            case CaptureStates.Capturing:

                Capture();

                break;
            case CaptureStates.Captured:
                CheckWhosInControlPoint();
                break;
            case CaptureStates.Contested:

                Contested();

                break;
            case CaptureStates.Reverting:

                Reverting();

                break;
            case CaptureStates.RevertingBackToOtherTeam:

                RevertBackToOtherTeam();

                break;
            default:

                break;
        }

    }

    private void Capture()
    {
        CheckWhosInControlPoint();

        m_progress += m_progressSpeed * Time.deltaTime;

        if (m_progress >= 1)
        {
            m_progress = 1;
            m_captureState = CaptureStates.Captured;
            m_teamAffiliation = m_unitInsideControlPointList[0].gameObject.tag;
            m_capturedStatus = true;
            m_previouslyOwned = true;
            GameEventsManager.instance.ControlPointCapture(id);
        }

        //Debug.Log("Control point " + id + " progress: " + m_progress);

    }

    private void Contested()
    {


        //Unit 1 continues capturing if Unit 2 dies
        //Go back to capturing
        if (m_unitInsideControlPointList.Count > 1)
        {
            for (int i = 1; i < m_unitInsideControlPointList.Count; i++)
            {
                if (m_unitInsideControlPointList[0].tag == m_unitInsideControlPointList[i].tag)
                {
                    m_captureState = CaptureStates.Capturing;
                }
            }
        }
        //Unit 2 reverts the capture if Unit 1 dies.
        //Unit 2 then starts capturing for their team
        else
        {
            if (m_unitInsideControlPointList[0].tag == m_teamAffiliation)
            {
                m_captureState = CaptureStates.Capturing;
            }
            else
            {
                m_captureState = CaptureStates.Reverting;
                m_capturedStatus = false;
            }
        }
    }
    /// <summary>
    /// Two types of reverting captures
    /// A: Enemy unit is taking the point and then leaves/dies before it's fully captured. Goes back to neutral 
    /// B: Enemy unit is taking an already captured point and then leaves/dies. Reverts capture back to original team
    /// </summary>
    private void Reverting()
    {
        CheckWhosInControlPoint();

        if (m_progress > 0)
        {
            m_progress -= m_progressSpeed * Time.deltaTime;
        }

        if (m_progress <= 0)
        {
            m_progress = 0;

            m_teamAffiliation = string.Empty;
            m_captureState = CaptureStates.Neutral;
            m_previouslyOwned = false;
            GameEventsManager.instance.ControlPointLoss(id);
        }

        //Debug.Log("Reverting control point " + id + " progress: " + m_progress);
    }

    private void RevertBackToOtherTeam()
    {
        CheckWhosInControlPoint();
        m_progress += m_progressSpeed * Time.deltaTime;

        if (m_progress >= 1)
        {
            m_progress = 1;
            m_captureState = CaptureStates.Captured;
            m_capturedStatus = true;
        }
    }

    void CheckWhosInControlPoint()
    {
        if (m_unitInsideControlPointList.Count > 1)
        {
            foreach (UnitRTS unit in m_unitInsideControlPointList)
            {
                if (unit != m_unitInsideControlPointList[0])
                {
                    m_captureState = CaptureStates.Contested;
                }
            }
        }
        else if (m_unitInsideControlPointList.Count == 1)
        {
            if (m_unitInsideControlPointList[0].tag == m_teamAffiliation && m_capturedStatus == false)
            {
                m_captureState = CaptureStates.Capturing;
            }
            else if (m_unitInsideControlPointList[0].tag != m_teamAffiliation)
            {
                m_captureState = CaptureStates.Reverting;
                m_capturedStatus = false;
            }
        }
        else
        {
            if (m_unitInsideControlPointList.Count == 0 && m_previouslyOwned == true && m_capturedStatus == false)
            {
                m_captureState = CaptureStates.RevertingBackToOtherTeam;
            }
            else if (m_unitInsideControlPointList.Count == 0 && m_previouslyOwned == false && m_capturedStatus == false)
            {
                m_captureState = CaptureStates.Reverting;
            }
        }
    }

    public float GetProgress()
    {
        return m_progress;
    }

    void setColor(Color t_color)
    {
        Color c = t_color;

        c.a = 0.25f;

        m_controlPointColor = c;
    }

    public Color GetColor()
    {
        return m_controlPointColor;
    }

    public string GetTeamAffiliation()
    {
        return m_teamAffiliation;
    }
}
