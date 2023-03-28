using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class MapArea : MonoBehaviour
{
    public event EventHandler OnCaptured;
    public enum State
    {
        Neutral,
        Captured
    }


    private List<MapAreaCollider> m_mapAreaColliderList = new List<MapAreaCollider>();
    private float m_progress;

    State m_state;

    private void Awake()
    {
        foreach (Transform child in transform)
        {
            MapAreaCollider mapAreaCollider = child.GetComponent<MapAreaCollider>();
            if (mapAreaCollider != null)
            {
                m_mapAreaColliderList.Add(mapAreaCollider);
            }
        }

        m_state = State.Neutral;
    }

    // Update is called once per frame
    void Update()
    {
        switch (m_state)
        {
            case State.Neutral:

                List<UnitMapAreas> unitMapAreasInsideList = new List<UnitMapAreas>();

                foreach (MapAreaCollider mapAreaCollider in m_mapAreaColliderList)
                {
                    foreach (UnitMapAreas unitMapAreas in mapAreaCollider.GetUnitMapAreas())
                    {
                        if (!unitMapAreasInsideList.Contains(unitMapAreas))
                        {
                            unitMapAreasInsideList.Add(unitMapAreas);
                        }
                    }
                }

                float progresSpeed = 1f;
                m_progress += unitMapAreasInsideList.Count * progresSpeed * Time.deltaTime;

                Debug.Log("Unit Count inside control point: " + unitMapAreasInsideList.Count + "; Progress: " + m_progress);

                if (m_progress >= 1f)
                {
                    m_state = State.Captured;
                    OnCaptured?.Invoke(this, EventArgs.Empty);
                    Debug.Log("Captured");
                }

                break;
            case State.Captured:
                break;
            default:
                break;
        }




    }
}
