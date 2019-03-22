using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractObjective : Objective
{
    public float holdTime;
    public bool preservesProgess = true;
    float currentTime;

    public override void Enter() { }

    public override void Exit()
    {
        if (!preservesProgess)
        {
            currentTime = 0;
        }
    }

    public override void In()
    {
        if (Input.GetKey(KeyCode.A))
        {
            currentTime += Time.deltaTime;
            if (currentTime >= holdTime)
            {
                Complete();
            }
        }
    }
}
