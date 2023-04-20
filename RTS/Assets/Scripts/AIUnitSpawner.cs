using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIUnitSpawner : MonoBehaviour
{
    public float m_spawnTime = 3;
    public int m_maxUnits;
    //public int m_currentUnits;
    public GameObject m_unit;
    public Transform m_spawnPoint;
    public int m_rotation;
    public List<GameObject> m_troopCount = new List<GameObject>();

    [SerializeField]
    GameObject m_spawnArea;
    int m_unitsQueued = 0;
    [SerializeField]
    float m_nextSpawn;

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < m_troopCount.Count; i++)
        {
            if (m_troopCount[i] == null)
            {
                m_troopCount.Remove(m_troopCount[i]);
            }
        }
    }

    public void SpawnUnit()
    {
        if (m_troopCount.Count < m_maxUnits && Time.time > m_nextSpawn && GameManager.instance.m_team1Reserves != 0 && m_unitsQueued < 9)
        {
            Vector3 spawn = m_spawnPoint.position + new Vector3(Random.Range(-m_spawnArea.transform.localScale.x / 3, m_spawnArea.transform.localScale.x / 3), Random.Range(-m_spawnArea.transform.localScale.y / 3, m_spawnArea.transform.localScale.y / 3), 0);
            m_nextSpawn = Time.time + m_spawnTime;
            Quaternion rotation = Quaternion.Euler(0, 0, m_rotation);
            GameObject newUnit = Instantiate(m_unit, spawn, rotation);
            newUnit.GetComponent<BaseUnit>().SetupTeam(tag);
            m_troopCount.Add(newUnit);
            GameManager.instance.m_team1Reserves--;
            m_unitsQueued--;
        }
    }
}
