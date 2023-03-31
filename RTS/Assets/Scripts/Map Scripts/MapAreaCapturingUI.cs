using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapAreaCapturingUI : MonoBehaviour
{
    [SerializeField]
    private List<MapArea> m_mapAreaList = new List<MapArea>();
    private MapArea m_mapArea;
    private List<Image> m_progressImages;
    public Image m_progressImage;

    private void Start()
    {
        foreach (MapArea mapArea in m_mapAreaList)
        {
            //mapArea.OnUnitEnter += MapArea_OnUnitEnter;
            //mapArea.OnUnitExit += MapArea_OnUnitExit;
        }

    }

    private void Update()
    {
        foreach (MapArea mapArea in m_mapAreaList)
        {
            //Check which specific mapArea has units in it
            //mapArea.
        }

        //m_progressImage.fillAmount = m_mapArea.GetProgress();
    }

    //private void MapArea_OnUnitExit(object sender, EventArgs e)
    //{
    //    //Do something?
    //}

    //private void MapArea_OnUnitEnter(object sender, EventArgs e)
    //{
    //    m_mapArea = sender as MapArea;
    //}
}
