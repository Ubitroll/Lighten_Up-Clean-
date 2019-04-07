using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

// This script checks if all objects have all references and are properly set on the scene (tags, objects assigned in inspector etc.). Attached to an empty game object "CheckSceneObject".
public class CheckScene : MonoBehaviour
{
    // Checks if all flamable objects are properly done
    public void CheckAllFlamableObject()
    {
        GameObject[] flamableObjects = GameObject.FindGameObjectsWithTag("Flamable");

        int itemScriptCount = 0;
        int fireEffectCount = 0;
        int steamEffectCount = 0;
        int meshRendererCount = 0;

        foreach (GameObject flamableObj in flamableObjects)
        {
            ItemScript itemScript = flamableObj.GetComponent<ItemScript>();

            // Checking if they have mesh renderers first as (itemScript == null) will skip current loop
            if (flamableObj.GetComponent<MeshRenderer>() == null)
            {
                meshRendererCount++;
                Debug.Log("The " + flamableObj.name + " object doesn't have mesh renderer.");
            }

            // If the script isn't attached to an object that is "Flamable"
            if (itemScript == null)
            {
                itemScriptCount++;
                Debug.Log("The " + flamableObj.name + " object has 'Flamable' tag but doesn't have ItemScript script attached to it.");
                continue;
            }
            // Checking if they have fire particle effect attached
            if (itemScript.fireEffect == null)
            {
                fireEffectCount++;
                Debug.Log("The " + flamableObj.name + " object doesn't have fire particle effect attached to it.");
            }
            // Checking if they have steam particle effect attached
            if (itemScript.steamEffect == null)
            {
                steamEffectCount++;
                Debug.Log("The " + flamableObj.name + " object doesn't have steam particle effect attached to it.");
            }
        }

        // Displaying errors only when they actually are errors
        if(itemScriptCount > 0)
            EditorUtility.DisplayDialog("Flamable object doesn't have ItemScript attached!", itemScriptCount + " objects have 'Flamable' tag but don't have ItemScript script attached to it. Check logs for more details.", "Ok");

        if(fireEffectCount > 0)
            EditorUtility.DisplayDialog("Flamable object doesn't have fire particle effect!", fireEffectCount + " objects don't have fire particle effect attached to it. Check logs for more details.", "Ok");

        if(steamEffectCount > 0)
            EditorUtility.DisplayDialog("Flamable object doesn't have steam particle effect!", steamEffectCount + " objects don't have steam particle effect attached to it. Check logs for more details.", "Ok");

        if(meshRendererCount > 0)
            EditorUtility.DisplayDialog("Flamable object doesn't have mesh renderer!", meshRendererCount + " objects don't have mesh renderers. Check logs for more details.", "Ok");
    }

    public void CheckAllObjects()
    {
        GameObject[] allObjects = UnityEngine.Object.FindObjectsOfType<GameObject>();

        int flamableCount = 0;
        foreach(GameObject gameObject in allObjects)
        {
            // Checking if object has ItemScript attached to it
            if (gameObject.GetComponent<ItemScript>() != null)
            {
                // Checking if the object that has ItemScript has "Flamable" tag asigned to it as well
                if(gameObject.tag != "Flamable")
                {
                    flamableCount++;
                    Debug.Log("The " + gameObject.name + " object has ItemScript attached to it but isn't tagged as 'Flamable'.");
                }
            }
        }

        if(flamableCount > 0)
            EditorUtility.DisplayDialog("Object with ItemScript missing 'Flamable' tag!", flamableCount + " objects have ItemScript attached to them but aren't tagget as 'Flamable'. Check logs for more details.", "Ok");
    }

    public void CheckCharacters()
    {
        GameObject human = GameObject.FindGameObjectWithTag("Human");

        // Checking if the tag (or human object) exists
        if (human == null)
        {
            EditorUtility.DisplayDialog("Human missing 'Human' tag!", "Human player object missing 'Human' tag.", "Ok");
            return;
        }
        else
        {
            ExtinguishObject extinguishObject = human.GetComponent<ExtinguishObject>();

            if (extinguishObject == null)
            {
                EditorUtility.DisplayDialog("ExtinguishScript not attached to Human!", "Human player object desn't have ExtinguishScript attached to it.", "Ok");
            }
            else
            {
                // Used elses so that only one error message is displayed (one error message will already get developers attention to this script)
                if(extinguishObject.waterBomb == null)
                    EditorUtility.DisplayDialog("Water bomb not attached to Human!", "Human player object desn't have water bomb attached to it.", "Ok");
                else
                if (extinguishObject.waterGun == null)
                    EditorUtility.DisplayDialog("Water gun not attached to Human!", "Human player object desn't have water gun attached to it.", "Ok");
                else
                if (extinguishObject.extinguishBar == null)
                    EditorUtility.DisplayDialog("Extinguish bar not attached to Human!", "Human player object desn't have extinguish bar (from Canvas) attached to it.", "Ok");
                else
                if (extinguishObject.humanCrosshair == null)
                    EditorUtility.DisplayDialog("Crosshair not attached to Human!", "Human player object desn't have crosshair (from Canvas) attached to it.", "Ok");
                else
                if (extinguishObject.humanUI == null)
                    EditorUtility.DisplayDialog("UI not attached to Human!", "Human player object desn't have UI text (from Canvas) attached to it.", "Ok");
                else
                if (extinguishObject.ammoText == null)
                    EditorUtility.DisplayDialog("Ammunition UI not attached to Human!", "Human player object desn't have ammunition text (from Canvas) attached to it.", "Ok");
                else
                if (extinguishObject.playerCamera == null)
                    EditorUtility.DisplayDialog("Camera not attached to Human!", "Human player object desn't have camera attached to it.", "Ok");
            }
        }

        // Checking the camera
        GameObject camera = GameObject.FindGameObjectWithTag("HumanCamera");
        if(camera == null)
            EditorUtility.DisplayDialog("Human's camera missing 'HumanCamera' tag!", "Human's camera doesn't have 'HumanCamera' tag.", "Ok");

        GameObject candle = GameObject.FindGameObjectWithTag("Candle");
        
        if(candle == null)
        {
            EditorUtility.DisplayDialog("Candle missing 'Candle' tag!", "Candle player object missing 'Candle' tag.", "Ok");
            return;
        }
        else
        {
            FireObjectScript fireObjectScript = candle.GetComponent<FireObjectScript>();

            if(fireObjectScript == null)
            {
                EditorUtility.DisplayDialog("FireObjectScript not attached to Candle!", "Candle player object desn't have FireObjectScript attached to it.", "Ok");
            }
            else
            {
                if(fireObjectScript.fireBar == null)
                    EditorUtility.DisplayDialog("Fire bar not attached to Candle!", "Candle player object desn't have fire bar (from Canvas) attached to it.", "Ok");
                else
                if(fireObjectScript.candleUI == null)
                    EditorUtility.DisplayDialog("UI not attached to Candle!", "Candle player object desn't have UI text (from Canvas) attached to it.", "Ok");
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        CheckAllObjects();
        CheckAllFlamableObject();
        CheckCharacters();
    }
}
