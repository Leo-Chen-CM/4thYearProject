using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class MapArea : MonoBehaviour
{
    public event EventHandler OnCaptured;
    public event EventHandler OnUnitEnter;
    public event EventHandler OnUnitExit;
    public enum State
    {
        Neutral,
        Capturing,
        Captured,
        Contested,
        RevertCapture
    }


    private List<MapAreaCollider> m_mapAreaColliderList = new List<MapAreaCollider>();
    private float m_progress;
    string m_teamAffiliation;
    State m_state;
    float m_progressSpeed = 0.1f;
    [SerializeField]
    List<UnitRTS> m_unitMapAreasInsideList = new List<UnitRTS>();

    private void Awake()
    {
        m_teamAffiliation = string.Empty;
        foreach (Transform child in transform)
        {
            MapAreaCollider mapAreaCollider = child.GetComponent<MapAreaCollider>();
            if (mapAreaCollider != null)
            {
                m_mapAreaColliderList.Add(mapAreaCollider);
                //mapAreaCollider.OnUnitEnter += MapAreaCollider_OnUnitEnter;
                //mapAreaCollider.OnUnitExit += MapAreaCollider_OnUnitExit;
            }
        }

        m_state = State.Neutral;
    }

    //private void MapAreaCollider_OnUnitExit(object sender, EventArgs e)
    //{
    //    bool hasUnitInside = false;
    //    foreach (MapAreaCollider mapAreaCollider in m_mapAreaColliderList)
    //    {

    //        if (mapAreaCollider.GetUnitMapAreas().Count > 0)
    //        {
    //            hasUnitInside = true;
    //        }
    //    }

    //    if (!hasUnitInside)
    //    {
    //        OnUnitExit?.Invoke(this, EventArgs.Empty);
    //    }
    //}
    
    //private void MapAreaCollider_OnUnitEnter(object sender, EventArgs e)
    //{
    //    OnUnitEnter?.Invoke(this, EventArgs.Empty);
    //}
    // Update is called once per frame
    void Update()
    {
        //List<UnitMapAreas> m_unitMapAreasInsideList = new List<UnitMapAreas>();
        m_unitMapAreasInsideList.Clear();
        foreach (MapAreaCollider mapAreaCollider in m_mapAreaColliderList)
        {
            foreach (UnitRTS unitMapAreas in mapAreaCollider.GetUnitMapAreas())
            {
                if (!m_unitMapAreasInsideList.Contains(unitMapAreas))
                {
                    m_unitMapAreasInsideList.Add(unitMapAreas);

                    Debug.Log("Adds unit map areas to the list");
                }
            }
        }



        switch (m_state)
        {
            case State.Neutral:

                if (m_unitMapAreasInsideList.Count > 0)
                {
                    m_teamAffiliation = m_unitMapAreasInsideList[0].tag;
                    m_state = State.Capturing;
                }
                else
                {
                    if (m_progress > 0)
                    {
                        m_progress -= m_progressSpeed * Time.deltaTime;
                        Debug.Log("Unit Count inside control point: " + m_unitMapAreasInsideList.Count + "\n Progress: " + m_progress);
                    }

                    if (m_progress < 0)
                    {
                        m_progress = 0;
                        Debug.Log("Unit Count inside control point: " + m_unitMapAreasInsideList.Count + "\n Progress: " + m_progress);
                    }
                }
                break;
            case State.Capturing:

                CheckUnitsInArea();

                m_progress += m_progressSpeed * Time.deltaTime;

                if (m_progress >= 1f)
                {
                    m_state = State.Captured;
                    OnCaptured?.Invoke(this, EventArgs.Empty);

                }

                Debug.Log("Unit Count inside control point: " + m_unitMapAreasInsideList.Count + "; Progress: " + m_progress);
                break;
            case State.Captured:
                Debug.Log("Control point captured");

                CheckUnitsInArea();

                break;

            case State.RevertCapture:
                Debug.Log("Reverting Capture point");

                if (m_progress > 0)
                {
                    m_progress -= m_progressSpeed * Time.deltaTime;
                    Debug.Log("Taking capture point back from the enemy");
                }

                if (m_progress < 0)
                {
                    m_progress = 0;

                    m_state = State.Neutral;

                    Debug.Log("Capture point reset");
                }

                CheckUnitsInArea();

                break;
            case State.Contested:
                Debug.Log("Control point contested");

                if (m_unitMapAreasInsideList.Count > 1)
                {
                    for (int i = 1; i < m_unitMapAreasInsideList.Count; i++)
                    {
                        if (m_unitMapAreasInsideList[0].tag == m_unitMapAreasInsideList[i].tag)
                        {
                            m_state = State.Capturing;
                        }
                    }
                }
                else
                {
                    m_state = State.Capturing;
                }

                break;

            default:
                break;
        }

    }
    void CheckUnitsInArea()
    {
        //Check if there's an enemy unit inside your capture point.
        for (int i = 0; i < m_unitMapAreasInsideList.Count; i++)
        {
            if (m_teamAffiliation != m_unitMapAreasInsideList[i].tag)
            {
                if (m_unitMapAreasInsideList.Count > 1)
                {
                    if (m_state == State.Neutral)
                    {

                    }
                    m_state = State.RevertCapture;
                }
                else
                {
                    m_state = State.Contested;
                }

            }
        }

        if (m_unitMapAreasInsideList.Count <= 0)
        {
            m_state = State.Neutral;
        }
    }
    public float GetProgress()
    {
        return m_progress;
    }
}
