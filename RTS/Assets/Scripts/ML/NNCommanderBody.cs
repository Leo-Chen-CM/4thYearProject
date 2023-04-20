using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NNCommanderBody : MonoBehaviour
{
    UnitManager m_unitManager;
    UnitSpawner m_unitSpawner;
    // Start is called before the first frame update
    void Start()
    {
        m_unitManager = GetComponent<UnitManager>();
        m_unitSpawner = GetComponent<UnitSpawner>();
    }

    public float[] GetInputs()
    {
        float[] floats = {GameManager.instance.m_team1ControlledPoints.Count, m_unitSpawner.m_troopCount.Count, GameManager.instance.m_team1Reserves, GameManager.instance.m_team1Score};

        return floats;
    }
}
