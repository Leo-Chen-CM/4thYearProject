using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitFieldOfView : MonoBehaviour
{

    public float m_radius = 10f;

    [Range(1, 360)] public float m_angle = 45f;

    public LayerMask m_targetLayer;
    public LayerMask m_obstructionLayer;

    [HideInInspector]
    public List<Transform> visibleTargets = new List<Transform>();

    public GameObject m_entityRef;

    public float m_meshResolution;
    public int edgeResolveIterations;
    public float edgeDstThreshold;

    public float maskCutawayDst = .1f;

    public MeshFilter viewMeshFilter;
    Mesh viewMesh;
    public bool m_enemySpotted { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        viewMesh = new Mesh ();
		viewMesh.name = "View Mesh";
		viewMeshFilter.mesh = viewMesh;




        m_entityRef = GameObject.FindGameObjectWithTag("Player");
        //StartCoroutine(FOVCheck());
        //StartCoroutine("FindTargetsWithDelay", .2f);
    }
    
    private void LateUpdate()
    {
        DrawFieldOfView();
    }

    IEnumerator FindTargetsWithDelay(float delay)
    {
        while (true)
        {
            yield return new WaitForSeconds(delay);
            FindVisibleTargets();
        }
    }
    void FindVisibleTargets()
    {
        visibleTargets.Clear();
        Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, m_radius, m_targetLayer);

        for (int i = 0; i < targetsInViewRadius.Length; i++)
        {
            Transform target = targetsInViewRadius[i].transform;
            Vector3 dirToTarget = (target.position - transform.position).normalized;
            if (Vector3.Angle(transform.forward, dirToTarget) < m_angle / 2)
            {
                float dstToTarget = Vector3.Distance(transform.position, target.position);
                if (!Physics.Raycast(transform.position, dirToTarget, dstToTarget, m_obstructionLayer))
                {
                    visibleTargets.Add(target);
                }
            }
        }
    }

    private IEnumerator FOVCheck()
    {
        WaitForSeconds wait = new WaitForSeconds(0.2f);

        while (true)
        {
            yield return wait;
            FOV();
        }
    
    }

    private void FOV()
    {
        Collider2D[] rangeCheck = Physics2D.OverlapCircleAll(transform.position, m_radius, m_targetLayer);

        if (rangeCheck.Length > 0)
        {
            Transform target = rangeCheck[0].transform;
            Vector2 directionToTarget = (target.position - transform.position).normalized;

            if (Vector2.Angle(transform.up, directionToTarget) < m_angle / 2)
            {
                float distanceToTarget = Vector2.Distance(transform.position, target.position);

                if (!Physics2D.Raycast(transform.position, directionToTarget, distanceToTarget, m_obstructionLayer))
                {
                    m_enemySpotted = true;
                }
                else
                {
                    m_enemySpotted = false;
                }
            }
            else
            {
                m_enemySpotted = false;
            }
        }
        else if (m_enemySpotted)
        {
            m_enemySpotted = false;
        }

    }

    /// <summary>
    /// Debug Field of view
    /// </summary>
    private void OnDrawGizmos()
    {

        
        Gizmos.color = Color.white;
        UnityEditor.Handles.DrawWireDisc(transform.position, Vector3.forward, m_radius);

        Vector3 angle01 = DirectionFromAngle(-transform.eulerAngles.z, -m_angle / 2);
        Vector3 angle02 = DirectionFromAngle(-transform.eulerAngles.z, m_angle / 2);


        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, transform.position + angle01 * m_radius);
        Gizmos.DrawLine(transform.position, transform.position + angle02 * m_radius);

        if (m_enemySpotted)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, m_entityRef.transform.position);
        }

    }


    /// <summary>
    /// Actual field of view
    /// </summary>
    private void DrawFieldOfView()
    {
        int stepCount = Mathf.RoundToInt(m_angle * m_meshResolution);
        float stepAngleSize = m_angle / stepCount;

        //for (int i = 0; i <= stepCount; i++)
        //{
        //    float angle = transform.eulerAngles.z - m_angle / 2 + stepAngleSize * i;
        //    Debug.DrawLine(transform.position, transform.position + DirFromAngle(angle, true) * m_radius, Color.red);
        //}

        List<Vector3> viewPoints = new List<Vector3>();
        ViewCastInfo oldViewCast = new ViewCastInfo();
        for (int i = 0; i <= stepCount; i++)
        {
            float angle = transform.eulerAngles.z - m_angle / 2 + stepAngleSize * i;
            ViewCastInfo newViewCast = ViewCast(angle);
            viewPoints.Add(newViewCast.point);
            //if (i > 0)
            //{
            //    bool edgeDstThresholdExceeded = Mathf.Abs(oldViewCast.dst - newViewCast.dst) > edgeDstThreshold;
            //    if (oldViewCast.hit != newViewCast.hit || (oldViewCast.hit && newViewCast.hit && edgeDstThresholdExceeded))
            //    {
            //        EdgeInfo edge = FindEdge(oldViewCast, newViewCast);
            //        if (edge.pointA != Vector3.zero)
            //        {
            //            viewPoints.Add(edge.pointA);
            //        }
            //        if (edge.pointB != Vector3.zero)
            //        {
            //            viewPoints.Add(edge.pointB);
            //        }
            //    }
            //}
            //oldViewCast = newViewCast;
        }

        int vertexCount = viewPoints.Count + 1;
        Vector3[] vertices = new Vector3[vertexCount];
        int[] triangles = new int[(vertexCount - 2) * 3];

        vertices[0] = Vector3.zero;
        for (int i = 0; i < vertexCount - 1; i++)
        {
            vertices[i + 1] = transform.InverseTransformPoint(viewPoints[i]);

            if (i < vertexCount - 2)
            {
                triangles[i * 3] = 0;
                triangles[i * 3 + 1] = i + 1;
                triangles[i * 3 + 2] = i + 2;
            }
        }

        viewMesh.Clear();

        viewMesh.vertices = vertices;
        viewMesh.triangles = triangles;
        viewMesh.RecalculateNormals();
    }

    EdgeInfo FindEdge(ViewCastInfo minViewCast, ViewCastInfo maxViewCast)
    {
        float minAngle = minViewCast.angle;
        float maxAngle = maxViewCast.angle;
        Vector3 minPoint = Vector3.zero;
        Vector3 maxPoint = Vector3.zero;

        for (int i = 0; i < edgeResolveIterations; i++)
        {
            float angle = (minAngle + maxAngle) / 2;
            ViewCastInfo newViewCast = ViewCast(angle);

            bool edgeDstThresholdExceeded = Mathf.Abs(minViewCast.dst - newViewCast.dst) > edgeDstThreshold;
            if (newViewCast.hit == minViewCast.hit && !edgeDstThresholdExceeded)
            {
                minAngle = angle;
                minPoint = newViewCast.point;
            }
            else
            {
                maxAngle = angle;
                maxPoint = newViewCast.point;
            }
        }

        return new EdgeInfo(minPoint, maxPoint);
    }


    public struct ViewCastInfo
    {
        public bool hit;
        public Vector3 point;
        public float dst;
        public float angle;

        public ViewCastInfo(bool _hit, Vector3 _point, float _dst, float _angle)
        {
            hit = _hit;
            point = _point;
            dst = _dst;
            angle = _angle;
        }
    }
    public struct EdgeInfo
    {
        public Vector3 pointA;
        public Vector3 pointB;

        public EdgeInfo(Vector3 _pointA, Vector3 _pointB)
        {
            pointA = _pointA;
            pointB = _pointB;
        }
    }

    ViewCastInfo ViewCast(float globalAngle)
    {
        Vector3 dir = DirFromAngle(globalAngle, true);
        RaycastHit hit;

        if (Physics.Raycast(transform.position, dir, out hit, m_radius, m_obstructionLayer))
        {
            return new ViewCastInfo(true, hit.point, hit.distance, globalAngle);
        }
        else
        {
            return new ViewCastInfo(false, transform.position + dir * m_radius, m_radius, globalAngle);
        }

    }

    public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
    {
        if (!angleIsGlobal)
        {
            angleInDegrees += transform.eulerAngles.z;
        }
        return new Vector3(Mathf.Sin(-angleInDegrees * Mathf.Deg2Rad), Mathf.Cos(-angleInDegrees * Mathf.Deg2Rad),0 );
    }

    private Vector2 DirectionFromAngle(float t_eulerZ, float t_angleInDegrees)
    {
        t_angleInDegrees += t_eulerZ;

        return new Vector2(Mathf.Sin(t_angleInDegrees * Mathf.Deg2Rad), Mathf.Cos(t_angleInDegrees * Mathf.Deg2Rad));
    }
}


