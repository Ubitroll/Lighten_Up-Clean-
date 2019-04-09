using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{

    public float moveToZ;
    public float cameraSpeed;
    private float timer;
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (timer > 3)
        {
            Vector3 target = new Vector3(transform.position.x, transform.position.y, moveToZ);
            Vector3 lerpedPos = Vector3.Lerp(transform.position, target, (1 - cameraSpeed) * cameraSpeed);
            transform.position = lerpedPos;
        }
    }
}
