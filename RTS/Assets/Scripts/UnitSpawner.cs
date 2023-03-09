using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSpawner : MonoBehaviour
{

    public float m_spawnTime;
    float m_nextSpawn = 0;
    public int m_maxUnits;
    public int m_currentUnits;
    public GameObject m_unit;
    public int m_reserves;
    public Transform m_spawnPoint;
    public int m_rotation;
    // Update is called once per frame
    void Update()
    {
        if (m_currentUnits < m_maxUnits && Time.time > m_nextSpawn && m_reserves != 0)
        {
            m_spawnPoint.position = new Vector3(Random.Range(-45, 45), m_spawnPoint.position.y + Random.Range(-1,1), 0);
            m_nextSpawn = Time.time + m_spawnTime;
            Quaternion rotation = Quaternion.Euler(0, 0, m_rotation);
            GameObject newUnit = Instantiate(m_unit, m_spawnPoint.position,rotation);
            newUnit.GetComponent<UnitRTS>().SetupTeam(tag);
            m_currentUnits++;
            m_reserves--;
        }
    }
}
