using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControlPointUI : MonoBehaviour
{
    [Serializable]
    public class MapAreaImage
    {
        public ControlPointController m_capturePoint;
        public Image m_progressImage;
    }

    [SerializeField]
    private List<MapAreaImage> m_mapAreaImages = new List<MapAreaImage>();

    private void Update()
    {
        foreach (MapAreaImage mapAreaImage in m_mapAreaImages)
        {
            mapAreaImage.m_progressImage.fillAmount = mapAreaImage.m_capturePoint.GetProgress();
            //mapAreaImage.m_progressImage.fillAmount = 0;
            mapAreaImage.m_progressImage.color = mapAreaImage.m_capturePoint.GetColor();
        }
    }

    //Capturing UI needs two things.
    //The progress
    //The color of the team


}
