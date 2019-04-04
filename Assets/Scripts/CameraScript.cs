using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{

    public float moveToZ;
    public float cameraSpeed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 target = new Vector3(transform.position.x, transform.position.y, moveToZ);
        Vector3 lerpedPos = Vector3.Lerp(transform.position, target, cameraSpeed * cameraSpeed);
        transform.position = lerpedPos;
    }
}
