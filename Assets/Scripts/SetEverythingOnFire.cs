using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetEverythingOnFire : MonoBehaviour
{
    public bool setOnFire;

    private bool setOnFireOnce = false;

    public void SetEverythingToFire()
    {
        GameObject[] flamableObjects = GameObject.FindGameObjectsWithTag("Flamable");

        foreach (GameObject flamableObj in flamableObjects)
        {
            if(flamableObj.GetComponent<ItemScript>() != null)
                flamableObj.GetComponent<ItemScript>().onFire = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButton("C3B"))
        {
            setOnFire = true;
        }
        if (setOnFire)
        {
            if (!(setOnFireOnce))
                SetEverythingToFire();
        }


    }
}
