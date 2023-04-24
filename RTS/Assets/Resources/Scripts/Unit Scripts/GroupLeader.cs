using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

/// <summary>
/// Adapted from EezehDev Coordinated Formations:
/// https://github.com/EezehDev/AI-Formations
/// </summary>
public class GroupLeader : MonoBehaviour
{
    public List<Transform> m_positions = new List<Transform>();
    // Group info
    [Header("Group Info")]
    public int groupID = -1;
    public List<BaseUnit> units = new List<BaseUnit>();
    public float m_spacing = 2;

    [SerializeField] private float m_StopDistance = 1;
    private Vector3 m_Target;
    private Vector3 m_TargetDirection;

    private float m_MaxSpeed = 5f;
    private float m_Speed = 0f;

    [SerializeField] private GameObject m_FormationPointPrefab;
    // Start is called before the first frame update
    void Start()
    {
        m_Target = transform.position;
        m_TargetDirection = transform.position;
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
            // If we are further away than stop distance
            float targetDistance = Vector3.Distance(transform.position, m_Target);
        if (targetDistance > m_StopDistance)
        {
            Vector3 v = new Vector3(transform.position.x, transform.position.y, 0);
            m_TargetDirection = (m_Target - v).normalized;
            Debug.Log("Target Direction: " + m_TargetDirection);
            m_Speed = m_MaxSpeed;
        }
        else
        {
            // Reset speed and angular velocity
            m_Speed = 0f;
        }
        // If we have a speed
        if (m_Speed != 0f)
        {

            float rotation = Mathf.Atan2(m_TargetDirection.y, m_TargetDirection.x) * Mathf.Rad2Deg;
            rotation -= 90;
            //m_Rigidbody2D.angularVelocity = rotation * -m_AngularSpeed;
            transform.rotation = Quaternion.Euler(0, 0, rotation);
            transform.position += (m_Speed * m_TargetDirection * Time.fixedDeltaTime);
        }

        RemoveExcessTransformations();
        MoveUnits();

        if (units.Count == 0)
        {
            Destroy(gameObject);
        }


        gameObject.GetComponent<SpriteRenderer>().enabled = GameManager.instance.m_debugMode;

        for (int i = 0; i < m_positions.Count; i++)
        {
            m_positions[i].GetComponent<SpriteRenderer>().enabled = GameManager.instance.m_debugMode;
        }
        
    }

    // Removes unecessary transformation points
    private void RemoveExcessTransformations()
    {
        foreach (BaseUnit unit in units)
        {
            if (unit == null)
            {
                units.Remove(unit);
            }
        }

        // If we have more transforms than needed, remove last ones
        if (m_positions.Count > units.Count)
        {
            for (int index = m_positions.Count; index < m_positions.Count; index++)
            {
                if (units[index] == null)
                {
                    units.RemoveAt(index);
                    Destroy(m_positions[index].gameObject);
                    m_positions.RemoveAt(index);
                }

            }
        }
    }
    private void MoveUnits()
    {
        Debug.Log("Unit count :" + units.Count);
        for (int index = 0; index < units.Count; index++)
        {
            units[index].SetTargetPosition(m_positions[index].position);
        }
    }
    // Sets a new leader location
    public void SetLocation(Vector3 location)
    {
        transform.position = location;
        m_Target = location;
    }
    // Set a new target
    public void SetTarget(Vector3 target)
    {
        m_Target = target;
        Debug.Log("Target set to :" + m_Target);
    }
    public void SetFormation(Formations t_formation)
    {
        switch (t_formation)
        {
            case Formations.Line:
                SetLinePosition();
                break;

            case Formations.Box:
                break;

            case Formations.Cheveron:
                break;

            default:
                break;
        }
    }

    protected void SetLinePosition()
    {
        // Store unit count
        int amountUnits = units.Count;

        // Multiple rows
        int minimumRowSize = 9;
        int maximumRows = 3;

        // Calculate rows
        int rows = 1;
        if (amountUnits > minimumRowSize)
            rows = ((amountUnits - 1) / minimumRowSize) + 1;

        if (rows > maximumRows)
            rows = maximumRows;

        // Define units per row
        int unitsPerRow = ((amountUnits - 1) / rows) + 1;

        // Set start position
        Vector2 bottomLeft = new Vector2(-m_spacing * (unitsPerRow / 2f) + (m_spacing / 2f), m_spacing * (rows / 2f) - (m_spacing / 2f));

        Vector3 currentPosition = Vector3.zero;
        currentPosition.x = bottomLeft.x;
        currentPosition.y = bottomLeft.y;

        // Loop to create formation
        int currentRow = 1;
        for (int index = 0; index < amountUnits; index++)
        {
            // Check if last row has a different size than other rows
            if ((rows > 1) && (currentRow == rows) && (index % unitsPerRow == 0))
            {
                int unitsLastRow = amountUnits - (unitsPerRow * (rows - 1));

                int unitDifference = unitsPerRow - unitsLastRow;
                if (unitDifference > 0)
                {
                    currentPosition.x += m_spacing * (unitDifference / 2f);
                }
            }

            // If we have transform already, set new position
            if (m_positions.Count > index)
            {
                m_positions[index].position= currentPosition + transform.position;
            }
            else
            {
                GameObject gameObject = Instantiate(m_FormationPointPrefab, transform);


                gameObject.transform.localPosition = currentPosition;
                // Set a position parented to the leader, with a relative position
                m_positions.Add(gameObject.transform);
            }

            if (((index + 1) % unitsPerRow) == 0)
            {
                // Update Y position and reset X
                currentPosition.y -= m_spacing;
                currentPosition.x = bottomLeft.x;
                currentRow++;
            }
            else
            {
                // Update X position
                currentPosition.x += m_spacing;
            }
        }
    }
}
