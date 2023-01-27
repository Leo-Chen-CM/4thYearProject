using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    Vector3 m_direction;
    Vector3 m_moveDir;

    [SerializeField]
    int m_speed = 10;
    // Update is called once per frame
    void Update()
    {
        m_direction = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0f);

        if (m_direction.magnitude >= 0.1f)
        {
            m_moveDir = Quaternion.Euler(0f, transform.eulerAngles.y, 0f) * m_direction;
            transform.position += (m_moveDir.normalized * m_speed * Time.deltaTime);
        }
    }
}
