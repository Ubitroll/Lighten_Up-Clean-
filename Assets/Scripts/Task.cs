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

    [Min(0)]
    public float triggerRadius = 20;
    public float holdTime = 10;
    public string taskPrompt;
    float progress = 0;
    bool complete = false;

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
    }

    [MenuItem("GameObject/Create Task", priority = 0)]
    static Task CreateTask()
    {
        GameObject task = new GameObject("New Task");
        Selection.activeObject = task;
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
                if (Input.GetButton("C1X") || Input.GetKey(KeyCode.T))
                {
                    Debug.Log("Completing task...");
                    progress += Time.deltaTime;
                    if (progress >= holdTime)
                    {
                        complete = true;
                        AudioManager.PlaySound("TaskComplete", transform.position);
                    }
                    progressBar.text = "Progress: " + ((int)(progress / holdTime * 100)).ToString() + "%";
                }
                else
                {
                    progressBar.text = "Hold X to " + taskPrompt;
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
}
