using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Objective
{
    public string description;
    public int repeatCount = 1;
    public Vector3 triggerPosition;
    public float triggerRadius;
    public SphereCollider trigger;
    public string tag = "Human";

    GameObject triggerObject;

    public Objective()
    {
        triggerObject = new GameObject("Task Trigger");
        triggerObject.GetComponent<Transform>().hideFlags = HideFlags.HideInInspector;
        trigger = triggerObject.AddComponent<SphereCollider>();
        triggerObject.AddComponent<TaskTrigger>().objective = this;
        trigger.isTrigger = true;
    }

    public abstract void Enter();
    public abstract void Exit();
    public abstract void In();

    public void UpdateObject()
    {
        triggerObject.transform.position = triggerPosition;
        trigger.radius = triggerRadius;
        triggerObject.name = "Task Trigger (" + description + ")";
    }

    public void DestroyTrigger()
    {
        Object.DestroyImmediate(triggerObject);
    }

    public void Complete()
    {
        Debug.Log(description + " objective complete!");
    }
}
