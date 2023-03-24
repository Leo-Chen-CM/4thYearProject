using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameGUI : MonoBehaviour
{
    [SerializeField]
    Canvas m_canvas;

    private GameObject m_image;
    private void Start()
    {
        m_canvas = FindObjectOfType<Canvas>();
        m_image = GameObject.Find("Unit Being Built");
    }

    void CreateUnit()
    {

    }
}
