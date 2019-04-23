using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

[RequireComponent(typeof(SphereCollider))]
public class Task : MonoBehaviour
{
    SphereCollider trigger;
    static Text progressBar;

    public static List<Task> tasks = new List<Task>();

    public float triggerRadius = 20; // Radius of the task
    public float holdTime = 10; // Time to hold down the button
    public string taskPrompt; // Text when near a task. (Press x to ...)
    public Transform lookTarget; // Optional target that requires the player to look in a specific direction to complete the task
    public float lookAngle; // Maximum angle between where the player is looking and the target

    [HideInInspector]
    public bool complete = false;

    float progress = 0;

    private void Start()
    {
        trigger = GetComponent<SphereCollider>();
        trigger.isTrigger = true;
        trigger.radius = triggerRadius;
        tasks.Add(this);
        if (progressBar == null)
        {
            if (GameObject.FindGameObjectWithTag("TaskProgress"))
            {
                progressBar = GameObject.FindGameObjectWithTag("TaskProgress").GetComponent<Text>();
            }
            else
            {
                progressBar = new GameObject().AddComponent<Text>();
            }
        }

        if (taskPrompt == "")
        {
            Debug.LogWarning("Task requires a description to function.");
        }
    }

    //[MenuItem("GameObject/Create Task", priority = 0)]
    static Task CreateTask()
    {
        GameObject task = new GameObject("New Task");
        //Selection.activeObject = task;
        return task.AddComponent<Task>();
    }

    private void Reset()
    {
        trigger = GetComponent<SphereCollider>();
        trigger.hideFlags = HideFlags.NotEditable;
        trigger.radius = 0;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, triggerRadius);
        Gizmos.color = new Color(1, 0, 0, 0.1f);
        Gizmos.DrawSphere(transform.position, triggerRadius);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Human")
        {
            if (!complete)
            {
                if (LookingAtTarget(other.transform)) // Ensures the player is looking at the task object before allowing them to complete it
                {
                    if (Input.GetButton("C1X"))
                    {
                        Debug.Log("Completing task...");
                        progress += Time.deltaTime;
                        if (progress >= holdTime)
                        {
                            complete = true;
                            AudioManager.PlaySound("TaskComplete");
                        }
                        progressBar.text = "Progress: " + ((int)(progress / holdTime * 100)).ToString() + "%";
                    }
                    else
                    {
                        progressBar.text = "Hold X to " + (taskPrompt != "" ? taskPrompt : "do task"); // Show the task prompt on the UI, defaults to "do task" if it is empty.
                    }
                }
                else
                {
                    progressBar.text = "";
                }
            }
            else
            {
                progressBar.text = "Completed Task";
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Human")
        {
            progressBar.text = "";
        }
    }

    public static float CompletionAmount()
    {
        int tasksDone = 0;
        foreach (Task task in tasks)
        {
            if (task.complete)
            {
                tasksDone++;
            }
        }
        return (tasksDone / tasks.Count) * 100;
    }

    // Returns true if the player is looking in the direction of the target object
    bool LookingAtTarget(Transform lookDirection)
    { 
        if (lookTarget == null)
        {
            return true;
        }
        //Debug.Log(Vector3.Dot(lookDirection.forward.normalized, (lookTarget.position - lookDirection.position).normalized));
        if (Vector3.Dot(lookDirection.forward.normalized, (lookTarget.position - lookDirection.position).normalized) >= (((lookAngle / 180) - 1) * -1))
        {
            return true;
        }
        return false;
    }
}
