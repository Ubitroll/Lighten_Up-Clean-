using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MoveState { STANDING, WALKING, RUNNING, JUMPING }

public class Character : MonoBehaviour
{
    const int maxPlayers = 2; // Sets the maximum amount of controller spaces
    public static Character[] characters = new Character[maxPlayers]; // Each character is added to this array when enabled

    [HideInInspector]
    public Camera playerCamera;
    Rigidbody rb;
    Animation animate;
    Collider col;

    int controller; // Used to differentiate player input from each joystick

    public float walkSpeed; // The default walk speed of the player
    public float runSpeed; // The speed of the player when running
    public float jumpStrength; // The strength of the players jump
    public float x_sensitivity = 200; // The x-sensitivity of the camera
    public float y_sensitivity = 200; // The y-sensitivity of the camera
    public float verticalViewLimit = 80f; // The limit of camera movement in the x-axis

    MoveState moveState; // The current state of movement the character is in
    float feetDistance; // The distance from the center of the collider to the bottom

    private void Awake()
    {
        AddPlayer(this); // Adds the current character
    }

    private void Start()
    {
        if (GetComponentInChildren<Camera>() != null)
        {
            playerCamera = GetComponentInChildren<Camera>();
        }
        if (GetComponent<Rigidbody>() != null)
        {
            rb = GetComponent<Rigidbody>();
        }
        if (GetComponent<Animation>() != null)
        {
            animate = GetComponent<Animation>();
        }
        if (GetComponent<Collider>() != null)
        {
            col = GetComponent<Collider>();
            feetDistance = col.bounds.extents.y;
        }
    }

    // Called when a character is enabled to give it a controller number
    static void AddPlayer(Character newPlayer)
    {
        for (int i = 0; i < maxPlayers; i++)
        {
            if (characters[i] == null)
            {
                newPlayer.controller = i;
                characters[i] = newPlayer;
                return;
            }
        }
        Debug.LogWarning("New character: " + newPlayer.ToString() + " was not assigned a controller value.");
    }

    void Update()
    {
        // Movement actions
        if ((Input.GetAxis("Move_Sideways_J" + controller.ToString()) != 0) ||
            (Input.GetAxis("Move_Forwards_J" + controller.ToString()) != 0))
        {
            if (Input.GetButton("Sprint_J" + controller.ToString()))
            {
                Move(new Vector3(Input.GetAxis("Move_Sideways_J" + controller.ToString()), 0, Input.GetAxis("Move_Forwards_J" + controller.ToString())), runSpeed);
                moveState = MoveState.RUNNING;
            }
            else
            {
                Move(new Vector3(Input.GetAxis("Move_Sideways_J" + controller.ToString()), 0, Input.GetAxis("Move_Forwards_J" + controller.ToString())), walkSpeed);
                moveState = MoveState.WALKING;
            }
        }
        else
        {
            moveState = MoveState.STANDING;
        }
        
        // Jumping actions
        if (!IsGrounded())
        {
            moveState = MoveState.JUMPING;
        }

        if (Input.GetButtonDown("Jump_J" + controller.ToString()) && IsGrounded())
        {
            Jump(jumpStrength);
        }

        // Rotation actions
        if (Input.GetAxis("Look_Horizontal_J" + controller.ToString()) != 0)
        {
            Turn(Input.GetAxis("Look_Horizontal_J" + controller.ToString()));
        }
        if (Input.GetAxis("Look_Vertical_J" + controller.ToString()) != 0)
        {
            TiltHead(Input.GetAxis("Look_Vertical_J" + controller.ToString()));
        }

        // Movement animation
        switch (moveState)
        {
            case MoveState.STANDING:
                animate.Play("t-pose");
                break;
            case MoveState.WALKING:
                animate.Play("walk");
                break;
            case MoveState.RUNNING:
                animate.Play("run");
                break;
            default:
                break;           
        }
    }

    // Moves the character
    virtual public void Move(Vector3 direction, float speed)
    {
        transform.position += (transform.forward * direction.z + transform.right * direction.x) * speed * Time.deltaTime;
    }

    // Turns the character left and right
    void Turn(float rotateAmount)
    {
        transform.Rotate(Vector3.up * rotateAmount * x_sensitivity * Time.deltaTime);
    }

    // Tilts the camera up and down
    virtual public void TiltHead(float rotateAmount)
    {
        playerCamera.transform.Rotate(Vector3.right * rotateAmount * y_sensitivity * Time.deltaTime);
        RestrictVerticalRotation(playerCamera.transform, verticalViewLimit);
    }

    // Makes the player jump upwards
    void Jump(float strength)
    {
        rb.AddForce(Vector3.up * 100 * strength);
    }

    // Checks if the player is grounded before allowing a jump
    bool IsGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, feetDistance + 0.025f);
    }

    // Restricts camera movement
    public virtual void RestrictVerticalRotation(Transform camera, float limit)
    {
        float ySetting = camera.transform.localEulerAngles.y;
        if (camera.transform.localEulerAngles.x > limit && camera.transform.localEulerAngles.x < 360 - limit - 10) // Line is a bit hard to read for the sake of getting it to work
        {
            camera.transform.localEulerAngles = new Vector3(limit, ySetting, 0);
        }
        else
        {
            if (camera.transform.localEulerAngles.x < 360 - limit && camera.transform.localEulerAngles.x > limit + 10) // Line is a bit hard to read for the sake of getting it to work
            {
                camera.transform.localEulerAngles = new Vector3(360 - limit, ySetting, 0);
            }
        }
    }

    ~Character() // Removes the character to make way for a new one
    {
        characters[controller] = null;
    }
}
