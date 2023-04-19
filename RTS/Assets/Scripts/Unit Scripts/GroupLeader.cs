using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GroupLeader : MonoBehaviour
{
    // Group info
    [Header("Group Info")]
    public int groupID = -1;
    public List<BaseUnit> units = new List<BaseUnit>();
    public float unitWidth = 1;

    // Formations
    [Header("Formation")]
    [SerializeField] private GameObject m_FormationPointPrefab = null;
    private List<Vector3> m_FormationTransforms = new List<Vector3>();
    private bool m_Wheeling;

    // Navigation
    [Header("Navigation")]
    [SerializeField] private float m_SpeedAmplifier = 1.3f;
    [SerializeField] private float m_StopDistance = 0.2f;
    [SerializeField] private float m_MaxAngularSpeed = 60f;
    [SerializeField] private float m_MinAngularSpeed = 15f;
    private Rigidbody2D m_Rigidbody2D;
    private NavMeshPath m_Path;
    private Vector3 m_Target;
    private Vector3 m_TargetDirection;
    private float m_MaxSpeed = 0f;
    private float m_Speed = 0f;
    private float m_AngularSpeed = 0f;

    // Initialise path and target
    private void Start()
    {
        m_Wheeling = true;

        // Initialize navigation data
        m_Path = new NavMeshPath();
        m_Target = transform.position;
        m_TargetDirection = transform.position;
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        // If we are further away than stop distance
        float targetDistance = Vector3.Distance(transform.position, m_Target);
        if (targetDistance > m_StopDistance)
        {
            // Recalculate path
            NavMesh.CalculatePath(transform.position, m_Target, NavMesh.AllAreas, m_Path);

            // If we have a path
            if (m_Path.corners.Length > 1)
            {
                m_TargetDirection = (m_Path.corners[1] - transform.position).normalized;
                m_Speed = m_MaxSpeed;
            }
            else
            {
                Debug.Log("No Path Found");
            }
        }
        else
        {
            // Reset speed and angular velocity
            m_Speed = 0f;
            m_Rigidbody2D.angularVelocity = 0;
        }


        // If we have a speed
        if (m_Speed != 0f)
        {
            if (m_Wheeling)
            {
                float rotation = Vector3.Cross(m_TargetDirection, transform.forward).y;
                m_Rigidbody2D.angularVelocity = rotation * -m_AngularSpeed;

                // Update position using speed
                transform.position += (m_Speed * transform.up * Time.fixedDeltaTime);
            }
            else
            {
                if (m_Speed != 0f)
                {
                    // Update position using speed
                    transform.position += (m_Speed * m_TargetDirection * Time.fixedDeltaTime);
                }
            }
        }

        // Move the units
        //MoveUnits();
    }

    // -------------------------
    // LEADER AND GROUP COMMANDS
    // -------------------------

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

    // Move units towards formation position
    private void MoveUnits()
    {
        for (int index = 0; index < units.Count; index++)
        {
            units[index].SetTargetPosition(m_FormationTransforms[index]);
        }
    }

    public void SetFormationPositions(List<Vector3> t_positions)
    {
        m_FormationTransforms = t_positions;
    }

}
