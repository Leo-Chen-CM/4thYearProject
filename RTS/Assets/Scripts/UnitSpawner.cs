using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
public class UnitSpawner : MonoBehaviour
{

    public float m_spawnTime = 3;
    public int m_maxUnits;
    //public int m_currentUnits;
    public GameObject m_unit;
    public int m_reserves;
    public Transform m_spawnPoint;
    public int m_rotation;
    [SerializeField]
    List<GameObject> m_troopCount = new List<GameObject>();
    [SerializeField]
    int m_requisitionPoints = 5; 
    [SerializeField]
    int m_requisitionTime = 5;

    float m_time = 3;

    [SerializeField]
    Image m_overlay;
    [SerializeField]
    Image m_prefabImage;

    [SerializeField]
    GameObject m_unitProductionUI;
    int m_unitsQueued = 0;

    [SerializeField]
    GameObject m_unitsBeingBuiltText;

    bool m_coroutineInUse = false;
    [SerializeField]
    float m_nextSpawn;

    [SerializeField]
    GameObject m_opponent;
    protected virtual void Start()
    {
        StartCoroutine(GenerateReserves());

        if (!m_opponent.GetComponent<RTSGameController>().m_AI)
        {
            m_unitProductionUI.SetActive(false);
        }

    }

    // Update is called once per frame
    //void Update()
    //{
    //    if (m_troopCount.Count < m_maxUnits && Time.time > m_nextSpawn && m_reserves != 0)
    //    {
    //        Vector3 spawn = new Vector3(Random.Range(-45, 45), m_spawnPoint.position.y + Random.Range(-1, 1), 0);
    //        m_nextSpawn = Time.time + m_spawnTime;
    //        Quaternion rotation = Quaternion.Euler(0, 0, m_rotation);
    //        GameObject newUnit = Instantiate(m_unit, spawn, rotation);
    //        m_troopCount.Add(newUnit);
    //        newUnit.GetComponent<UnitRTS>().SetupTeam(tag);
    //        m_reserves--;
    //    }
    //}

    private void Update()
    {
        if (m_opponent.GetComponent<RTSGameController>().m_AI)
        {
            if (m_troopCount.Count < m_maxUnits && Time.time > m_nextSpawn && m_reserves != 0)
            {
                Vector3 spawn = new Vector3(m_spawnPoint.position.x + Random.Range(-1, 1), m_spawnPoint.position.y + Random.Range(-45, 45), 0);
                m_nextSpawn = Time.time + m_spawnTime;
                Quaternion rotation = Quaternion.Euler(0, 0, m_rotation);
                GameObject newUnit = Instantiate(m_unit, spawn, rotation);
                newUnit.GetComponent<UnitRTS>().SetupTeam(tag);
                m_troopCount.Add(newUnit);
                m_reserves--;
                m_unitsQueued--;
            }
        }
        else
        {
            if (m_unitsQueued > 1)
            {
                m_unitsBeingBuiltText.GetComponent<TextMeshProUGUI>().text = "x" + m_unitsQueued;
            }
            else
            {
                m_unitsBeingBuiltText.GetComponent<TextMeshProUGUI>().text = "";
            }
        }

        for (int i = 0; i < m_troopCount.Count; i++)
        {
            if (m_troopCount[i] == null)
            {
                m_troopCount.Remove(m_troopCount[i]);
            }
        }
    }

    IEnumerator GenerateReserves()
    {
        while(true)
        {
            m_reserves += m_requisitionPoints;

            if (m_reserves > 1000)
            {
                m_reserves = 1000;
            }

            yield return new WaitForSeconds(m_requisitionTime);
        }
    }

    public void SpawnUnit()
    {
        if (m_troopCount.Count < m_maxUnits && m_reserves != 0)
        {
            if (m_unitsQueued < 9)
            {
                m_unitsQueued++;
            }

            if (!m_coroutineInUse)
            {
                m_unitProductionUI.SetActive(true);
                StartCoroutine(CreatingUnit());
            }

        }

    }


    IEnumerator CreatingUnit()
    {
        m_overlay.fillAmount = 1;

        Texture2D td = AssetPreview.GetAssetPreview(m_unit);

        Rect rect = new Rect(0, 0, td.width, td.height);

        m_prefabImage.sprite = Sprite.Create(td, rect, new Vector2(0, 0));
        m_coroutineInUse = true;

        while (m_unitsQueued > 0)
        {

            while (m_time > -0.1f)
            {
                m_overlay.fillAmount = m_time / m_spawnTime;
                m_time -= Time.deltaTime;
                yield return null;
            }

            //Creates a new unit
            Vector3 spawn = new Vector3(m_spawnPoint.position.x + Random.Range(-1, 1), m_spawnPoint.position.y + Random.Range(-45,45), 0);
            Quaternion rotation = Quaternion.Euler(0, 0, m_rotation);
            GameObject newUnit = Instantiate(m_unit, spawn, rotation);
            newUnit.GetComponent<UnitRTS>().SetupTeam(tag);
            m_troopCount.Add(newUnit);
            m_reserves--;
            m_unitsQueued--;
            SpawnUI();
            m_time = m_spawnTime;


        }
        m_coroutineInUse = false;
        m_unitProductionUI.SetActive(false);
    }

    void SpawnUI()
    {
        m_unitsBeingBuiltText.GetComponent<TextMeshProUGUI>().text = "x" + m_unitsQueued;
    }
}
