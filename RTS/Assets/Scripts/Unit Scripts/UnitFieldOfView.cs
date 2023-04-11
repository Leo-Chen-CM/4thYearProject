using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

/// <summary>
/// Field of view of a single unit
/// </summary>
public class UnitFieldOfView : MonoBehaviour
{
    public float m_radius;
    public float m_engagementRanged;

    [Range(1, 360)] public float m_angle = 45f;

    public LayerMask m_targetLayer;
    public LayerMask m_obstructionLayer;
    public LayerMask m_obstructionOnlyLayer;

    [HideInInspector]
    public List<Transform> visibleTargets = new List<Transform>();

    public float m_meshResolution;
    public int edgeResolveIterations;
    public float edgeDstThreshold;

    public float maskCutawayDst = .1f;

    public MeshFilter viewMeshFilter;
    Mesh viewMesh;
    public bool m_enemySpotted { get; private set; }

    public Transform m_target;

    private UnitShooting m_unitShooting;

    List<Collider2D> m_targetsInView = new List<Collider2D>();

    // Start is called before the first frame update
    void Start()
    {
        viewMesh = new Mesh();
        viewMesh.name = "View Mesh";
        viewMeshFilter.mesh = viewMesh;

        m_unitShooting = GetComponent<UnitShooting>();
        StartCoroutine("FindTargetsWithDelay", .1f);
    }

    bool FindVisibleTargets()
    {
        Collider2D[] targetsInViewRadius = Physics2D.OverlapCircleAll(new Vector2(transform.position.x, transform.position.y), m_radius, m_targetLayer);
        m_targetsInView.Clear();

        //m_targetsInView.AddRange(Physics2D.OverlapCircleAll(new Vector2(transform.position.x, transform.position.y), m_radius, m_targetLayer));

        foreach (Collider2D collider2D in targetsInViewRadius)
        {
            if (collider2D.gameObject.tag != gameObject.tag)
            {
                m_targetsInView.Add(collider2D);
            }
        }


        if (m_targetsInView.Count > 0)
        {
            int length = m_targetsInView.Count;

            for (int i = 0; i < length; i++)
            {
                if (m_targetsInView[i].gameObject != gameObject)
                {
                    Transform target = m_targetsInView[i].transform;
                    Vector3 dirToTarget = (target.position - transform.position).normalized;
                    float dstToNewTarget = Vector3.Distance(transform.position, target.position);

                    if (Physics2D.Raycast(transform.position, dirToTarget, dstToNewTarget, m_obstructionLayer) && dstToNewTarget < m_radius)
                    {
                        if (m_target != null)
                        {
                            float currentDstToTarget = Vector3.Distance(transform.position, m_target.position);

                            if (dstToNewTarget < currentDstToTarget)
                            {
                                m_target = m_targetsInView[i].transform;
                                m_enemySpotted = true;
                            }
                        }
                        else
                        {
                            m_target = m_targetsInView[i].transform;
                            m_enemySpotted = true;  
                        }

                    }
                    else
                    {
                        m_enemySpotted = false;
                        m_target = null;
                    }
                }


            }
        }
        else
        {
            m_enemySpotted = false;
            m_target = null;
        }
        return m_enemySpotted;
     
    }


    /// <summary>
    /// Debug Field of view
    /// </summary>
    private void OnDrawGizmos()
    {
        if (m_target != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, m_target.position);
        }

    }

    private void LateUpdate()
    {
        DrawFieldOfView();
        if (m_target != null)
        {
            transform.up = Vector3.Lerp(transform.up, (m_target.position - transform.position), 1);
            m_unitShooting.Shoot();
        }
    }

    IEnumerator FindTargetsWithDelay(float delay)
    {
        while (true)
        {
            yield return new WaitForSeconds(delay);
            FindVisibleTargets();
        }
    }

    /// <summary>
    /// Actual field of view
    /// </summary>
    private void DrawFieldOfView()
    {
        int stepCount = Mathf.RoundToInt(m_angle * m_meshResolution);
        float stepAngleSize = m_angle / stepCount;

        for (int i = 0; i <= stepCount; i++)
        {
            float angle = transform.eulerAngles.z - m_angle / 2 + stepAngleSize * i;
            Debug.DrawLine(transform.position, transform.position + DirFromAngle(angle, true) * m_radius, Color.red);
        }

        List<Vector3> viewPoints = new List<Vector3>();
        ViewCastInfo oldViewCast = new ViewCastInfo();
        for (int i = 0; i <= stepCount; i++)
        {
            float angle = transform.eulerAngles.z - m_angle / 2 + stepAngleSize * i;
            ViewCastInfo newViewCast = ViewCast(angle);
            viewPoints.Add(newViewCast.point);
            if (i > 0)
            {
                bool edgeDstThresholdExceeded = Mathf.Abs(oldViewCast.dst - newViewCast.dst) > edgeDstThreshold;
                if (oldViewCast.hit != newViewCast.hit || (oldViewCast.hit && newViewCast.hit && edgeDstThresholdExceeded))
                {
                    EdgeInfo edge = FindEdge(oldViewCast, newViewCast);
                    if (edge.pointA != Vector3.zero)
                    {
                        viewPoints.Add(edge.pointA);
                    }
                    if (edge.pointB != Vector3.zero)
                    {
                        viewPoints.Add(edge.pointB);
                    }
                }
            }
            oldViewCast = newViewCast;
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

        //int combinedMask = m_targetLayer | m_obstructionLayer;

        RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, m_radius, m_obstructionLayer);

        if (hit)
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
        return new Vector3(Mathf.Sin(-angleInDegrees * Mathf.Deg2Rad), Mathf.Cos(-angleInDegrees * Mathf.Deg2Rad), 0);
    }
}


