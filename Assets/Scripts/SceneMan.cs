using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneMan : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadMainLevel()
    {
        Application.LoadLevel("Prototype2");
        Debug.Log("Loaded Main Level");
    }
}
