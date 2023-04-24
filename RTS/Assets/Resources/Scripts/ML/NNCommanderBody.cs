using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NNCommanderBody : MonoBehaviour
{
    UnitManager m_unitManager;
    AIUnitSpawner m_unitSpawner;
    // Start is called before the first frame update
    void Start()
    {
        m_unitSpawner = GetComponent<AIUnitSpawner>();
        m_unitManager = GetComponent<UnitManager>();
    }

    public float[] GetInputs()
    {
        float[] floats = { m_unitSpawner.m_troopCount.Count, GameManager.instance.m_team1ControlledPoints.Count, GameManager.instance.m_team1Score, GameManager.instance.m_team1Reserves};

        return floats;
    }


    
    public void DecideAction()
    {
        float best = NNCommanderBrain.instance.m_outputs[0];
        int index = 0;
        for (int i = 1; i < NNCommanderBrain.instance.m_outputs.Length; i++)
        {
            if (NNCommanderBrain.instance.m_outputs[i] > best)
            {
                best = NNCommanderBrain.instance.m_outputs[i];
                index = i;
            }
        }

        switch (index)
        {
            case 0:
                //Generate a group of units
                Debug.Log("Make more units: " + NNCommanderBrain.instance.m_outputs[0]);
                m_unitSpawner.SpawnUnit();

                break;
            case 1:
                //Send units to cp
                Debug.Log("Send units to CP: " + NNCommanderBrain.instance.m_outputs[1]);

                break;
            case 2:
                //Send units to rp
                Debug.Log("Send units to RP: " + NNCommanderBrain.instance.m_outputs[2]);
                break;

            default:
                break;
        }
    }


    private void SendUnits()
    {

    }

    private void Rally()
    {
        int randomUnit = Random.Range(0, m_unitSpawner.m_troopCount.Count);


        Collider2D[] collider2DArray = Physics2D.OverlapCircleAll(m_unitSpawner.m_troopCount[randomUnit].transform.position, 5);

        if (collider2DArray.Length > 5)
        {
            foreach (Collider2D collider2D in collider2DArray)
            {
                BaseUnit unitRTS = collider2D.GetComponent<BaseUnit>();

                if (unitRTS == null || unitRTS.gameObject.tag != gameObject.tag)
                {
                    continue;
                }

                GroupLeader leader = unitRTS.GetLeader();
                //Has no group then keep it loose
                if (leader == null)
                {
                    if (!m_unitManager.m_selectedUnits.Contains(unitRTS))
                    {
                        m_unitManager.m_selectedUnits.Add(unitRTS);
                        unitRTS.SetSelectedVisible(true);
                    }
                }
                //Has a group so get everyone from that group
                else
                {
                    if (m_unitManager.m_selectedGroupLeaders.Contains(leader))
                        continue;

                    foreach (BaseUnit unit in leader.units)
                    {
                        unit.SetSelectedVisible(true);
                    }
                    m_unitManager.m_selectedGroupLeaders.Add(leader);
                }
            }
        }


    }
}

