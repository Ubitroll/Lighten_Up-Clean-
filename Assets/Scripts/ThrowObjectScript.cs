using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// This script should be attached to objects that are throwable. Enables the object to be thrown. When being carried
// and collides with another object (wall or any different collider) it will drop from the player's hands.
public class ThrowObjectScript : MonoBehaviour
{
    bool eraseUI = false; // used to erase the UI text once (other scripts are using the UI too)
    public float throwForce = 3500.0f;
    public bool ableToPickObject = false; // when the player is near the object he is able to pick it up
    public bool objectCarried = false; // checks if the player is currently carrying object
    public bool objectCollided = false; // checks if the carried object has touched a wall

    private Transform objectHolder; // empty game object
    private Transform player;
    private Transform playerCam;
    private Text humanUI; // used to notify the player he can pick the object up and how to operate with it

    void Start()
    {
        objectHolder = GameObject.Find("ObjectHolder").transform;
        player = GameObject.Find("Human").transform;
        playerCam = GameObject.FindGameObjectWithTag("HumanCamera").transform;
        humanUI = player.gameObject.GetComponent<ExtinguishObject>().humanUI;
    }

    // Update is called once per frame
    void Update()
    {
        float dist = Vector3.Distance(gameObject.transform.position, player.transform.position);

        //Debug.Log("Distance to object " + transform.name + " is " + dist);

        ExtinguishObject extObjScript = player.gameObject.GetComponent<ExtinguishObject>();

        // if the player is close enough to the object
        if(dist <= 26.0f && extObjScript.currentWeapon == 0)
        {
            ableToPickObject = true;
            eraseUI = false;

            humanUI.text = "Press B button to pick up " + gameObject.name + " object.";
        }
        else
        {
            // erasing the UI text
            if (!(eraseUI))
                humanUI.text = "";

            eraseUI = true;
            ableToPickObject = false;
        }

        // if able to pick the object up and user presses B button
        if(ableToPickObject && Input.GetButtonDown("C1B"))
        {
            objectCarried = true;
        }

        // while the object is being carried
        if(objectCarried)
        {
            transform.position = objectHolder.transform.position;

            humanUI.text = "Press B button to throw the object or Y to leave it.";

            // if object collides with something it drops out of player's hands
            if(objectCollided)
            {
                objectCarried = false;
                objectCollided = false;
                humanUI.text = "";
            }

            // when the player presses X button
            if(Input.GetButtonDown("C1B"))
            {
                transform.parent = null;
                objectCarried = false;
                // adding force to the object
                GetComponent<Rigidbody>().AddForce(playerCam.forward * throwForce);
                humanUI.text = "";
            }

            // droping the object from hands
            if(Input.GetButtonDown("C1Y"))
            {
                transform.parent = null;
                objectCarried = false;
                humanUI.text = "";
            }
        }
    }
    
    private void OnCollisionEnter(Collision collision)
    {
        if(objectCarried)
        {
            //Debug.Log("I " + gameObject.name + " collided with " + collision.gameObject.name);
            // in case the collision occurs with the human
            if (collision.collider.tag != "Human")
            {
                objectCollided = true;
            }
        }
    }
}
