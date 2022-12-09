using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;

public class TestAgent : Agent
{
    public Transform m_shootingPointDirection;
    public int minStepsBetweenShots = 50;
    public int damage = 1;
    public void Shoot(Vector3 t_direction)
    {
        int layerMask = 1 << LayerMask.NameToLayer("Enemy");

        if (Physics.Raycast(m_shootingPointDirection.position, t_direction, out var hit, 200f, layerMask))
        {
            //hit.transform.GetComponent<Enemy>.GetShot(damage);
        }
    }

    public void OnMouseEnter()
    {
        Shoot(Vector3.forward);
    }














}
