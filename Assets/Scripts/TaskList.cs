using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TaskList : MonoBehaviour
{
    public float MinRange = 100f; // Range a task needs to be in to be considered nearby
    public Text closestUI; // Set to the UI element that displays the nearby task to the player
    public Text taskListUI; // Set to the UI element displaying the list of tasks still to be done

    Task[] tasks;
    Task closestTask = null;
    bool showList;

    private void Start()
    {
        tasks = Task.tasks.ToArray(); // Stores a copy of the tasks in the scene
        closestUI = closestUI ?? FindObjectOfType<Text>(); // Finds a Text object to use if not assigned
        taskListUI = taskListUI ?? FindObjectOfType<Text>(); // Finds a Text object to use if not assigned
    }
    
    private void Update()
    {
        closestTask = GetClosestTask();

        if (Input.GetButton("C1LB")) // || Input.GetKey(KeyCode.L)) // Displays the tasks still to be done when held
        {
            showList = true;
            taskListUI.transform.parent.gameObject.SetActive(true);
        }
        else
        {
            showList = false;
            taskListUI.transform.parent.gameObject.SetActive(false);
        }

        if (closestTask != null) // If there is a nearby task, display it in the UI
        {
            closestUI.text = "Nearby Task: " + Capitalise(closestTask.taskPrompt);// + " (" + minDistance.ToString() + "m)";
        }
        else
        {
            closestUI.text = "";
        }

        taskListUI.text = DrawList();
    }

    // Capitalises the first character of the given string
    string Capitalise(string str)
    {
        if (str == "")
        {
            return str;
        }
        return char.ToUpper(str[0]) + str.Substring(1);
    }

    // Returns the nearest task to the player
    Task GetClosestTask()
    {
        float minDistance = MinRange;
        Task currentClosest = null;
        foreach (Task task in tasks)
        {
            float distance = Vector3.Distance(gameObject.transform.position, task.transform.position);
            if (distance < minDistance && !task.complete)
            {
                minDistance = distance;
                currentClosest = task;
            }
        }
        return currentClosest;
    }

    // Returns the string of tasks to be displayed to the player
    string DrawList()
    {
        string taskList = "To do:\n\n";
        foreach (Task task in tasks)
        {
            if (!task.complete) // Currently hides completed tasks rather than strikethrough them
            {
                taskList += Capitalise(task.taskPrompt) + "\n";
            }
        }
        return taskList;
    }
}
