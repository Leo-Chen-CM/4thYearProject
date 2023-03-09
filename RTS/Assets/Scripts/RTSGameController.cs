using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

enum Teams
{
    Team1,
    Team2
};

public class RTSGameController : MonoBehaviour
{
    [SerializeField]
    private Transform m_selectedAreaTransform;
    private Vector3 m_startPosition;
    private List<UnitRTS> m_selectedUnits;
    // Update is called once per frame

    [SerializeField]
    Teams m_team = Teams.Team1;
    private void Awake()
    {
        m_selectedUnits = new List<UnitRTS>();
        m_selectedAreaTransform.gameObject.SetActive(false);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            m_selectedAreaTransform.gameObject.SetActive(true);
            m_startPosition = Utility.ReturnMousePosition2D();
        }

        if (Input.GetMouseButton(0))
        {

            Vector3 currentMousePosition = Utility.ReturnMousePosition2D();
            Vector3 lowerLeft = new Vector3
                (
                    Mathf.Min(m_startPosition.x, currentMousePosition.x),
                    Mathf.Min(m_startPosition.y, currentMousePosition.y)        
                );

            Vector3 upperRight= new Vector3
                (
                    Mathf.Max(m_startPosition.x, currentMousePosition.x),
                    Mathf.Max(m_startPosition.y, currentMousePosition.y)        
                );

            m_selectedAreaTransform.position = lowerLeft;
            m_selectedAreaTransform.localScale = upperRight - lowerLeft;
        }


        if (Input.GetMouseButtonUp(0))
        {
            m_selectedAreaTransform.gameObject.SetActive(false);

            Collider2D[] collider2DArray = Physics2D.OverlapAreaAll(m_startPosition, Utility.ReturnMousePosition2D());


            foreach(UnitRTS unitRTS in m_selectedUnits)
            {
                unitRTS.SetSelectedVisible(false);
            }

            m_selectedUnits.Clear();
            foreach (Collider2D collider2D in collider2DArray)
            {
                UnitRTS unitRTS = collider2D.GetComponent<UnitRTS>();

                if (unitRTS != null && unitRTS.gameObject.tag == m_team.ToString()) 
                {
                    unitRTS.SetSelectedVisible(true);
                    m_selectedUnits.Add(unitRTS);
                }

            }
            Debug.Log(m_selectedUnits.Count);
        }


        if (Input.GetMouseButtonDown(1))
        {
            Vector3 moveToPosition = Utility.ReturnMousePosition2D();

            List<Vector3> targetPositionList = GetPositionListAround(moveToPosition, new float[] { 5f, 10f, 15f }, new int[] { 5, 10, 20 });


            int targetPositionListIndex = 0;

            foreach (UnitRTS unit in m_selectedUnits)
            {
                unit.m_agent.SetTargetPosition(targetPositionList[targetPositionListIndex]);
                targetPositionListIndex = (targetPositionListIndex + 1) % targetPositionList.Count;
            }
        }
    }

    private List<Vector3> GetPositionListAround(Vector3 t_startPosition, float[] t_ringDistanceArray, int[] t_ringPositionCountArray)
    {
        List<Vector3> positionList = new List<Vector3>();
        positionList.Add(t_startPosition);
        for (int i = 0; i < t_ringDistanceArray.Length; i++)
        {
            positionList.AddRange(GetPositionListAround(t_startPosition, t_ringDistanceArray[i], t_ringPositionCountArray[i]));
        }
        return positionList;
    }

    private List<Vector3> GetPositionListAround(Vector3 t_startPosition, float t_distance, int t_positionCount)
    {
        List<Vector3> positionList = new List<Vector3>();
        for (int i = 0; i < t_positionCount; i++)
        {
            float angle = i * (360f / t_positionCount);
            Vector3 dir = ApplyRotationToVector(new Vector3(1, 0), angle);
            Vector3 position = t_startPosition + dir * t_distance;
            positionList.Add(position);
        }
        return positionList;
    }


    private Vector3 ApplyRotationToVector(Vector3 t_vector3, float t_angle)
    {
        return Quaternion.Euler(0, 0, t_angle) * t_vector3;
    }
}
