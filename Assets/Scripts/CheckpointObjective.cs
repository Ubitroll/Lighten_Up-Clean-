using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointObjective : Objective
{
    public override void Enter()
    {
        Complete();
    }

    public override void Exit() { }

    public override void In() { }
}
