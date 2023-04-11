using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapAreaUI : MonoBehaviour
{
    [System.Serializable]
    public class MapAreaImage
    {
        public Image m_uiImage;
        public MapArea m_mapArea;

        [SerializeField]
        private List<MapArea> m_mapAreaList = new List<MapArea>();
        private List<Image> m_progressImages;
        public Image m_progressImage;
    }

    [SerializeField]
    private List<MapAreaImage> m_mapAreaImages = new List<MapAreaImage>();


    private void Start()
    {
        foreach(MapAreaImage mapAreaImage in m_mapAreaImages)
        {
            mapAreaImage.m_mapArea.OnCaptured += (object sender, EventArgs e) =>
            {
                mapAreaImage.m_uiImage.color = Color.green;
            };


        }
    }
}
