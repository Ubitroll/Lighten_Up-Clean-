using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CandlePlayer : Character
{
    // Provides temporary fix for the candle being oriented backwards
    override public void Move(Vector3 direction, float speed)
    {
        base.Move(-direction, speed);
    }

    // Seperate camera movement from first person camera
    public override void TiltHead(float rotateAmount)
    {
        playerCamera.transform.parent.Rotate(Vector3.right * rotateAmount * y_sensitivity * Time.deltaTime);
        RestrictVerticalRotation(playerCamera.transform.parent, verticalViewLimit);
    }
}
