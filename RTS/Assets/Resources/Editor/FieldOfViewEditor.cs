using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
[CustomEditor(typeof(UnitFieldOfView))]
public class FieldOfViewEditor : Editor
{

    void OnSceneGUI()
    {
        UnitFieldOfView fow = (UnitFieldOfView)target;
        Handles.color = Color.white;
        Handles.DrawWireDisc(fow.transform.position, Vector3.forward, fow.m_radius);
        Vector3 viewAngleA = fow.DirFromAngle(-fow.m_angle/ 2, false);
        Vector3 viewAngleB = fow.DirFromAngle(fow.m_angle / 2, false);

        Handles.DrawLine(fow.transform.position, fow.transform.position + viewAngleA * fow.m_radius);
        Handles.DrawLine(fow.transform.position, fow.transform.position + viewAngleB * fow.m_radius);

        Handles.color = Color.yellow;
        foreach (Transform visibleTarget in fow.visibleTargets)
        {
            Handles.DrawLine(fow.transform.position, visibleTarget.position);
        }
    }

}