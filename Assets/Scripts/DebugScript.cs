using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("C1moveX is " + Input.GetAxis("C1moveX"));
        Debug.Log("C1moveY is " + Input.GetAxis("C1moveY"));
        Debug.Log("C1horizontal is " + Input.GetAxis("C1horizontal"));
        Debug.Log("C1vertical is " + Input.GetAxis("C1vertical"));
    }
}
