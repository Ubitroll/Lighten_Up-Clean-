using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    // Variables for controlls
    // Set to automatically resort to controller 1
    // Booleans
    // Set to public incase there is an error and one is stuck on
    public bool leftStick = true;
    public bool rightStick = true;
    public bool aPressed, bPressed, xPressed, yPressed = false;
    public bool walking = false;
    public bool standing = false;
    public bool running = false;
    public bool firing = false;
    // Strings
    // Set to public so they can be assigned in the editor to the correct input
    public string moveX = "C1moveX";
    public string moveY = "C1moveY";
    public string horizontal = "C1horizontal";
    public string vertical = "C1vertical";
    public string aButton = "C1A";
    public string bButton = "C1B";
    public string xButton = "C1X";
    public string yButton = "C1Y";
    public float runSpeed = 10;
    public float sensitivityCamera = 3;
    public float jumpStrength = 3;
    public GameObject camera;
    public float cameraLower = 50f;
    public float cameraUpper = -30f;
    public float feetDistance;
    // Privates
    private Vector3 startPos;
    private Transform playerTransform;
    private Animation anim;
    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        playerTransform = transform;
        anim = gameObject.GetComponent<Animation>();

        if (GetComponent<Rigidbody>() != null)
        {
            rb = GetComponent<Rigidbody>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        // If not grounded
        if (IsGrounded() == false)
        {
            rb.AddForce(Vector3.down * 6);
        }
        
        
        // If the left stick is set to respond
        if (leftStick)
        {
            // Takes in the input direction
            Vector3 direction = Vector3.zero;
            direction.x = Input.GetAxis(moveX);
            direction.z = Input.GetAxis(moveY);

            // if the player is moving
            if (direction.x != 0 || direction.z != 0)
                walking = true;
            else
                walking = false;

            playerTransform.position = playerTransform.position + (playerTransform.forward * direction.z + playerTransform.right * direction.x) * (Time.deltaTime * runSpeed);
        }

        // If the right stick is set to respond
        if (rightStick)
        {
            playerTransform.Rotate(new Vector3(0, Input.GetAxis(horizontal), 0) * sensitivityCamera);
            camera.transform.Rotate(new Vector3(Input.GetAxis(vertical), 0, 0) * sensitivityCamera);

            if (camera.transform.eulerAngles.x <= cameraUpper)
            {
                camera.transform.Rotate(sensitivityCamera, 0, 0);
            }
            else if (camera.transform.eulerAngles.x >= cameraLower)
            {
                camera.transform.Rotate(-sensitivityCamera, 0, 0);
            }
           
        }
        
        // If a button is pressed
        if (Input.GetButton(aButton))
        {
            aPressed = true;

            Debug.Log("A button was pressed");
            if (IsGrounded())
            {
                rb.AddForce(Vector3.up * 100 * jumpStrength);
            }
        }
        else
        {
            aPressed = false;
        }

        // If b button is pressed
        if (Input.GetButton(bButton))
        {
            bPressed = true;
        }
        else
        {
            bPressed = false;
        }

        // If x button is pressed
        if (Input.GetButton(xButton))
        {
            xPressed = true;
        }
        else
        {
            xPressed = false;
        }

        // If y button is pressed
        if (Input.GetButton(yButton))
        {
            yPressed = true;
        }
        else
        {
            yPressed = false;
        }

        // if the player isn't walking or running
        if (!(walking) && !(running))
            standing = true;

        // switching boolean values
        if(walking)
        {
            standing = false;
            running = false;
        } else if (standing)
        {
            walking = false;
            running = false;
        } else if(running)
        {
            walking = false;
            standing = false;
        }

        // play the approporiate animation
        if(this.tag == "Candle" && firing)
        {
            anim.Play("fire");
        }
        else if (walking)
        {
            anim.Play("walk");
        }
        else if (standing)
        {
            anim.Play("t-pose");
        }
        else if (running)
        {
            anim.Play("run");
        }
    }

    bool IsGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, feetDistance + 0.025f);
    }
}
