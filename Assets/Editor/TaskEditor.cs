using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class TaskEditor : EditorWindow
{
    Vector2 taskListScroll; //Scroll value for taskList section
    Task focusTask = null; //Task in focus

    int listIndex = 0; //Popup index for objective creation
    string[] objectiveOptions = new string[] { "Checkpoint", "Interaction" };

    [MenuItem("Window/Task Editor")]
    public static void ShowWindow()
    {
        GetWindow(typeof(TaskEditor));
    }

    void OnGUI()
    {
        //Creates seperate side by side sections
        EditorGUILayout.BeginHorizontal();

        //List of tasks and add/remove buttons
        EditorGUILayout.BeginVertical();

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Add Task"))
        {
            new Task("New Task");
        }
        EditorGUI.BeginDisabledGroup(focusTask == null); //Allows for task deletion only when a task is in focus
        if (GUILayout.Button("Remove Task"))
        {
            focusTask.Remove();
            focusTask = null;
        }
        EditorGUI.EndDisabledGroup();
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Space();
        
        taskListScroll = EditorGUILayout.BeginScrollView(taskListScroll);
        foreach (Task task in Task.taskList) //Shows each task in the taskList
        {
            //if (GUILayout.Button(task.name, GUILayout.MaxWidth(400))) //Button list
            if (EditorGUILayout.Foldout(false, task.name, true)) //Hierarchy like list
            {
                FocusOnTask(task); //Puts the selected task in focus
            }
        }
        
        EditorGUILayout.EndScrollView();
        EditorGUILayout.EndVertical();

        //Focused task editing section
        EditorGUILayout.BeginVertical(EditorStyles.helpBox, GUILayout.MinWidth(500));
        if (focusTask != null) //If there is a task in focus
        {
            EditorGUILayout.LabelField(focusTask.name, EditorStyles.boldLabel);
            EditorGUILayout.Space();
            focusTask.name = EditorGUILayout.TextField(focusTask.name);
            if (focusTask.name.Length == 0)
            {
                Debug.LogWarning("Task must have a name!");
                focusTask.name = "New Task";
            }
            focusTask.enabled = EditorGUILayout.Toggle("Enable Task", focusTask.enabled); //Allows for enabling/disabling the selected task
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Add Objective"))
            {
                focusTask.NewObjective(listIndex);
            }
            listIndex = EditorGUILayout.Popup(listIndex, objectiveOptions);
            EditorGUILayout.EndHorizontal();
            if (focusTask.objectives.Count > 0)
            {
                focusTask.objectiveScroll = EditorGUILayout.BeginScrollView(focusTask.objectiveScroll);
                foreach (Objective obj in focusTask.objectives)
                {
                    GUILayout.Space(10);
                    GUILayout.Label(obj.GetType().ToString(), EditorStyles.boldLabel);
                    EditorGUILayout.BeginHorizontal();
                    obj.description = EditorGUILayout.TextField("Description", obj.description);
                    if (GUILayout.Button("Remove Objective"))
                    {
                        focusTask.Remove(obj);
                        break;
                    }
                    EditorGUILayout.EndHorizontal();
                    obj.repeatCount = EditorGUILayout.IntField("Repeat Objective Amount", obj.repeatCount);
                    obj.trigger = (SphereCollider)EditorGUILayout.ObjectField("Trigger Collider", obj.trigger, typeof(SphereCollider), true);
                    EditorGUILayout.BeginHorizontal();
                    obj.triggerPosition = EditorGUILayout.Vector3Field("Trigger Position", obj.triggerPosition);
                    obj.triggerRadius = EditorGUILayout.FloatField("Trigger Radius", obj.triggerRadius);
                    EditorGUILayout.EndHorizontal();
                    obj.UpdateObject();
                    if (obj.GetType() == typeof(InteractObjective))
                    {
                        InteractObjective iObj = (InteractObjective)obj;
                        EditorGUILayout.BeginHorizontal();
                        iObj.holdTime = EditorGUILayout.FloatField("Button Down Time", iObj.holdTime);
                        iObj.preservesProgess = EditorGUILayout.Toggle("Preserves Progress", iObj.preservesProgess);
                        EditorGUILayout.EndHorizontal();
                    }
                }
                EditorGUILayout.EndScrollView();
            }
            else
            {
                EditorGUILayout.LabelField("Start by creating an objective.");
            }
        }
        else
        {
            EditorGUILayout.LabelField("Select a task to begin editing it.");
        }
        EditorGUILayout.EndVertical();
        EditorGUILayout.EndHorizontal();
    }

    //Puts a new task in focus
    void FocusOnTask(Task task)
    {
        focusTask = task;
    }
}