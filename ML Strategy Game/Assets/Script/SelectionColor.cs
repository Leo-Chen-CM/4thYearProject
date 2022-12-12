using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionColor : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Renderer>().material.color = Color.red;
    }

    // Update is called once per frame
    void onDestroy()
    {
        GetComponent<Renderer>().material.color = Color.white;
    }
}
