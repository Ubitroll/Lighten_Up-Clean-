using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// This script should be attached to human character. It allows the player to choose weapons, use them, reload (Water-Gun)
// and to refill them. 
public class ExtinguishObject : MonoBehaviour
{
	public int currentWeapon = 0; // 0 = no weapon, 1 = "Water-Gun", 2 = "Water-Bomb", 3 = "Water-Mug"
	public List<GameObject> waterWeapons; // used array to make it easier in future to add more weapons.
	public GameObject waterGun;
	public GameObject waterBomb;
	float distanceToWater = Mathf.Infinity; // distance to water supply
	float distanceToFire = Mathf.Infinity; // distance to flamable object's point he is raycasting
	public GameObject extinguishBar; // extinguish bar displaying how much is left to put off the fire from the object
	public Image humanCrosshair; // human crosshair
	public float throwForce = 30.0f; // amount of force that player throws the Water-Bomb
	public bool raycastedFire = false; // needed to use this to make the extinguish bar properly work
	public Text humanUI; // UI text
	public Text ammoText; // UT ammunition text
    public GameObject playerCamera;

	private float amountFilled = 0.0f; // amount of bar to be filled (how much the human extinguished the object already)
	private Vector3 currentHumanPos;
	private bool nearWaterTrigger = false; // used on trigger enter and exit, if true the player can fill up his weapons with water
	private float waterAmount = 0.0f; // used to know how much water was poured over fired object
	private bool isAbleToThrow = true; // associated with throwing the water bomb
	private float timeFillingUp = 0.0f; // used for refilling the weapons with water

	private void FillUpWater(GameObject weapon)
	{
        // When the player presses X button
		if(Input.GetButton("C1A"))
		{
			Debug.Log ("Filling up current weapon with water");

			// if the weapon is water gun
			if (weapon == waterGun) 
			{
				WaterGunScript waterGunScript = waterGun.GetComponent<WaterGunScript> ();

				// assuming the water gun can have maximum of 30 ammo (10 in clip and 20 in reserve)
				int currentAmmo = waterGunScript.waterAmmoClip + waterGunScript.waterAmmoClip;

				if (currentAmmo < 30) 
				{
					// filling up
					timeFillingUp += Time.deltaTime;

					// giving 1 ammo each second
					if (timeFillingUp > 1) 
					{
						// firstly filling the clip
						if (waterGunScript.waterAmmoClip < 10) 
						{
							waterGunScript.waterAmmoClip++;
						} 
						else // then filling up the reserve ammo
							if(waterGunScript.waterAmmoAll < 20)
							{
								waterGunScript.waterAmmoAll++;
							}
						
						// resetting
						timeFillingUp = 0.0f;
					}
				}
				
			}

			if (weapon == waterBomb) 
			{
				WaterBombScript waterBombScript = waterBomb.GetComponent<WaterBombScript> ();
				
				// assuming the player can only have maximum of 10 bombs
				if (waterBombScript.numberOfBombs < 10) 
				{
					// filling up
					timeFillingUp += Time.deltaTime;

					// giving 1 bomb each 3 seconds
					if(timeFillingUp > 3)
					{
						waterBombScript.numberOfBombs++;

						// resetting
						timeFillingUp = 0.0f;
					}
				
				}
			}
		}
	}

	void OnTriggerEnter(Collider collider)
	{
		if (collider.gameObject.tag == "WaterSupply") 
		{
			nearWaterTrigger = true;
			humanUI.text = "Hold A to fill up!";
		}
        // Debug purposes
        // Debug.Log ("Human entered trigger " + collider.gameObject.name + " object.");
    }

    void OnTriggerExit(Collider collider)
	{
		if (collider.gameObject.tag == "WaterSupply")
		{
			nearWaterTrigger = false;
			humanUI.text = "";
		}
        // Debug purposes
		// Debug.Log ("Human exited trigger " + collider.gameObject.name + " object.");
	}
		
	IEnumerator DelayThrowing()
	{
		isAbleToThrow = false;
		yield return new WaitForSeconds (0.2f);
		isAbleToThrow = true;
	}

	void ThrowWaterBomb()
	{
		WaterBombScript waterBombScript = waterBomb.GetComponent<WaterBombScript> ();

		if (waterBombScript.numberOfBombs > 0) 
		{
            waterBombScript.numberOfBombs--;
            // bomb is instantiated a bit forward from the water-bomb the player is holding
            GameObject bomb = Instantiate (Resources.Load ("Prefabs/Water-Bomb"), waterBomb.transform.position + new Vector3(0f, 0, 1.5f) , Quaternion.identity) as GameObject;

            // getting human's camera and adding force from it's looking point
            GameObject camera = GameObject.FindGameObjectWithTag("HumanCamera").gameObject;
            bomb.GetComponent<Rigidbody> ().AddForce (camera.transform.forward * throwForce, ForceMode.Impulse);
		}
	}

	// Start is called before the first frame update
	void Start()
	{
		waterWeapons = new List<GameObject> ();
		// assigning gameobjects to waterWeapons array
		waterWeapons.Add(waterGun);
		waterWeapons.Add(waterBomb);

        currentWeapon = 0;
	}

    // Update is called once per frame
    void Update()
    {
		// if the player entered the trigger associated with water supply
		if (nearWaterTrigger) 
		{
			// if current weapon isn't no weapon
			if (currentWeapon != 0)
				FillUpWater (waterWeapons [currentWeapon - 1]);
		} 

        // getting the next weapon
        if(Input.GetButtonDown("C1RB") && currentWeapon < waterWeapons.Count)
        {
            ++currentWeapon;
            for (int i = 0; i < waterWeapons.Count; i++)
            {
                if (i == currentWeapon - 1)
                    waterWeapons[i].SetActive(true);
                else
                    waterWeapons[i].SetActive(false);
            }

            //Debug.Log("Selected " + waterWeapons[currentWeapon - 1]);
        }

        // getting the previous weapon
        if (Input.GetButtonDown("C1LB") && currentWeapon > 0)
        {
            --currentWeapon;

            if (currentWeapon == 0)
            {
                // disactivating all weapons
                for (int i = 0; i < waterWeapons.Count; i++)
                {
                    waterWeapons[i].SetActive(false);

                    ammoText.text = "";
                    //Debug.Log ("No weapons selected!");
                }
            }
            else
            {
                for (int i = 0; i < waterWeapons.Count; i++)
                {
                    if (i == currentWeapon - 1)
                        waterWeapons[i].SetActive(true);
                    else
                        waterWeapons[i].SetActive(false);
                }

                //Debug.Log("Selected " + waterWeapons[currentWeapon - 1]);
            } // end of else
        } // end of pressed C1LB

        //Debug.Log("Weapon: " + (currentWeapon));

		// throwing needs to be in this script rather than in the WaterBombScript
		if (waterWeapons[1].gameObject.activeSelf) 
		{
			if (Input.GetButtonDown("C1B") && isAbleToThrow) 
			{
				StartCoroutine (DelayThrowing ());
				ThrowWaterBomb ();
			}

		} // end of activeSelf

		// moving the crosshair to the center of his screen
		humanCrosshair.transform.position = new Vector3(Screen.width * 0.75f, Screen.height * 0.5f);
			
		currentHumanPos = this.transform.position;

		RaycastHit hit = new RaycastHit ();

		// raycast
		if (Physics.Raycast (playerCamera.transform.position, playerCamera.transform.TransformDirection (Vector3.forward), out hit, Mathf.Infinity)) 
		{
            if (hit.collider.gameObject.tag == "Flamable")
            {
                ItemScript itemScript = hit.collider.gameObject.GetComponent<ItemScript>();

                if (itemScript.onFire)
                {
                    raycastedFire = true;
                    // getting the % of water poured on the object and what's needed
                    amountFilled = itemScript.amountOfWater / itemScript.waterAmountNeeded;
                }
                else
                    raycastedFire = false;
            }
            else
                raycastedFire = false;
        }

        // UI elements showing up
        if (raycastedFire)
        {
            // to make sure the blue colour doesn't exeed the width of the extinguish bar
            if (amountFilled > 1)
                amountFilled = 1;

            Debug.Log("Raycasted fire");

            // Showing text and extinguish bar
            extinguishBar.SetActive(true);
            extinguishBar.transform.GetChild(0).gameObject.SetActive(true);
            extinguishBar.transform.GetChild(0).transform.localScale = new Vector3(amountFilled, 1, 1);
        }
        else
        {
            amountFilled = 0;
            extinguishBar.SetActive(false);
            extinguishBar.transform.GetChild(0).gameObject.SetActive(false);
        }
    }
}
