using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEventsManager : MonoBehaviour
{
    public static GameEventsManager instance;

    private void Awake()
    {
        instance = this;
    }

    public event Action<int> OnControlPointTriggerEnter;

    public void ControlPointTriggerEnter(int id)
    {
        OnControlPointTriggerEnter?.Invoke(id);
    }

    public event Action<int> OnControlPointTriggerExit;

    public void ControlPointTriggerExit(int id)
    {
        OnControlPointTriggerExit?.Invoke(id);
    }

    public event Action<int> OnControlPointCapture;

    public void ControlPointCapture(int id)
    {
        OnControlPointCapture?.Invoke(id);
    }

}
