using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Task
{
    public static List<Task> taskList = new List<Task>();

    public string name;
    public bool enabled;
    public List<Objective> objectives;
    public Vector2 objectiveScroll;

    public Task(string taskName)
    {
        name = taskName;
        taskList.Add(this);
        objectives = new List<Objective>();
    }

    public Objective NewObjective(int index)
    {
        Objective newObjective = null;
        switch (index)
        {
            case 0:
                newObjective = new CheckpointObjective();
                break;
            case 1:
                newObjective = new InteractObjective();
                break;
            default:
                Debug.LogWarning("Please choose an objective type from the dropdown menu.");
                break;
        }
        if (newObjective != null)
        {
            objectives.Add(newObjective);
        }
        return newObjective;
    }

    public void Remove()
    {
        foreach (Objective obj in objectives)
        {
            obj.DestroyTrigger();
        }
        taskList.Remove(this);
    }

    public void Remove(Objective objective)
    {
        objectives.Remove(objective);
        objective.DestroyTrigger();
    }

    public static string[] ToNameArray()
    {
        string[] stringArray = new string[taskList.Count];
        for (int i = 0; i < taskList.Count; i++)
        {
            stringArray[i] = taskList[i].name;
        }
        return stringArray;
    }
}
