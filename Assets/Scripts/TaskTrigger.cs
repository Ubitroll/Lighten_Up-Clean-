using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskTrigger : MonoBehaviour
{
    public Objective objective;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == objective.tag)
        {
            objective.Enter();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == objective.tag)
        {
            objective.Exit();
        }
    }

    public void OnTriggerStay(Collider other)
    {
        if (other.tag == objective.tag)
        {
            objective.In();
        }
    }
}
