using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

// This script should be attached to an empty game object. Controls the navigation throughout all menu screens.
public class MainMenuScript : MonoBehaviour
{
    public string sceneName;
    public string newSceneName;

    // Variables for scene MainMenu
    public GameObject startButton;
    public GameObject optionsButton;
    public GameObject quitButton;
    public Dictionary<GameObject, bool> buttonsDict;

    public string aButton = "C1A";
    public string bButton = "C1B";
    public string moveX = "C1moveX";

    public bool moveLeft = false;
    public bool moveRight = false;

    private bool ableToSwitch = true;
    private bool startCoroutineOnce = false;
    // end of variables for scene MainMenu

    // Variables for scene StartScene
    public GameObject controller1;
    public GameObject controller2;

    public string moveXC2 = "C2moveX";
    public bool moveLeftC2 = false;
    public bool moveRightC2 = false;

    private bool ableToSwitchC2 = true;
    private bool startCoroutineOnceC2 = false;
    private float controllerStartPosX;
    // end of variables for scene StartScene

    private void Awake()
    {
        // Ensuring the script is not deleted while loading through menus
        DontDestroyOnLoad(this);

        // Destroying any copies of this object
        if (FindObjectsOfType(GetType()).Length > 1)
            Destroy(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        sceneName = SceneManager.GetActiveScene().name;
        GetSceneComponents();   
    }

    // Kind of Start() method but plays at the start of each scene
    void GetSceneComponents()
    {
        if (sceneName == "MainMenu")
        {
            startButton = GameObject.Find("StartButton").gameObject;
            optionsButton = GameObject.Find("OptionsButton").gameObject;
            quitButton = GameObject.Find("QuitButton").gameObject;

            // Calculating how big each panel should be
            int screenWidth = Screen.width;
            int panelWidth = Screen.width / 3 - 15;

            // Resizing the buttons (panels) and placing them so that they fill the whole screen
            RectTransform startRect = startButton.GetComponent<Image>().rectTransform;
            startRect.sizeDelta = new Vector2(panelWidth, startRect.sizeDelta.y);
            startRect.position = new Vector2(panelWidth / 2 + 10, startRect.position.y);

            RectTransform optionsRect = optionsButton.GetComponent<Image>().rectTransform;
            optionsRect.sizeDelta = new Vector2(panelWidth, optionsRect.sizeDelta.y);
            optionsRect.position = new Vector2(panelWidth * 1.5f + 20, optionsRect.position.y);

            RectTransform quitRect = quitButton.GetComponent<Image>().rectTransform;
            quitRect.sizeDelta = new Vector2(panelWidth, quitRect.sizeDelta.y);
            quitRect.position = new Vector2(panelWidth * 2.5f + 30, quitRect.position.y);

            // Storing the buttons in dictionary
            buttonsDict = new Dictionary<GameObject, bool>();
            buttonsDict.Add(startButton, true);
            buttonsDict.Add(optionsButton, false);
            buttonsDict.Add(quitButton, false);
        }

        if (sceneName == "StartScene")
        {
            controller1 = GameObject.Find("Controller1").gameObject;
            controller2 = GameObject.Find("Controller2").gameObject;

            controllerStartPosX = controller1.transform.position.x;
        }
    }

    IEnumerator DelaySwitch()
    {
        startCoroutineOnce = true;
        ableToSwitch = false;
        yield return new WaitForSeconds(0.3f);
        ableToSwitch = true;
        startCoroutineOnce = false;
    }

    IEnumerator DelaySwitchC2()
    {
        startCoroutineOnceC2 = true;
        ableToSwitchC2 = false;
        yield return new WaitForSeconds(0.3f);
        ableToSwitchC2 = true;
        startCoroutineOnceC2 = false;
    }

    // Update is called once per frame
    void Update()
    {
        newSceneName = SceneManager.GetActiveScene().name;

        // Checking if scene changed, if so, getting it's components
        if(newSceneName != sceneName)
        {
            sceneName = newSceneName;
            GetSceneComponents();
        }

        if (sceneName == "MainMenu")
        {
            moveRight = false;
            moveLeft = false;

            if (Input.GetAxis(moveX) < 0 && ableToSwitch)
            {
                moveLeft = true;
                moveRight = false;
                if (!(startCoroutineOnce))
                    StartCoroutine(DelaySwitch());
            }
            else
            if (Input.GetAxis(moveX) > 0 && ableToSwitch)
            {
                moveRight = true;
                moveLeft = false;
                if (!(startCoroutineOnce))
                    StartCoroutine(DelaySwitch());
            }

            foreach (GameObject key in buttonsDict.Keys)
            {
                if (buttonsDict[key])
                {
                    if (moveLeft)
                    {
                        // Checking if it's start button (upmost left)
                        if (key == startButton)
                        {
                            // don't do nothing
                            break;
                        }
                        else
                        {
                            // move left
                            if (key == optionsButton)
                            {
                                buttonsDict[optionsButton] = false;
                                buttonsDict[startButton] = true;
                                break;
                            }
                            else
                                if (key == quitButton)
                            {
                                buttonsDict[quitButton] = false;
                                buttonsDict[optionsButton] = true;
                                break;
                            }
                        }
                    } // end of if(moveLeft)
                    else
                        if (moveRight)
                    {
                        if (key == quitButton)
                        {
                            break;
                        }
                        else
                        {
                            if (key == optionsButton)
                            {
                                buttonsDict[optionsButton] = false;
                                buttonsDict[quitButton] = true;
                                break;
                            }
                            else
                                if (key == startButton)
                            {
                                buttonsDict[startButton] = false;
                                buttonsDict[optionsButton] = true;
                                break;
                            }
                        }
                    } // end of if(moveRight)
                }// end of if(buttonsDict[key])
            }// end of foreach

            // Displaying the button and highlighting the right one
            foreach (GameObject key in buttonsDict.Keys)
            {
                if (buttonsDict[key])
                {
                    key.GetComponent<Image>().color = new Color32(0, 0, 0, 140);
                }
                else
                {
                    key.GetComponent<Image>().color = new Color32(255, 255, 255, 140);
                }
            }


            if (Input.GetButton(aButton))
            {
                // Check which button was highlighted when the user pressed A
                foreach (GameObject key in buttonsDict.Keys)
                {
                    if (buttonsDict[key])
                    {
                        if (key == startButton)
                            SceneManager.LoadScene("StartScene");

                        if (key == optionsButton)
                            SceneManager.LoadScene("Options");

                        if (key == quitButton)
                            Application.Quit();
                    }
                }
            }
        } // end of if sceneName = MainMenu

        if (sceneName == "StartScene")
        {
            moveRight = false;
            moveLeft = false;
            moveLeftC2 = false;
            moveRightC2 = false;

            // Checking input for player1
            if (Input.GetAxis(moveX) < 0 && ableToSwitch)
            {
                moveLeft = true;
                moveRight = false;
                if (!(startCoroutineOnce))
                    StartCoroutine(DelaySwitch());
            }
            else
            if (Input.GetAxis(moveX) > 0 && ableToSwitch)
            {
                moveRight = true;
                moveLeft = false;
                if (!(startCoroutineOnce))
                    StartCoroutine(DelaySwitch());
            }

            // Checking input for player2
            if (Input.GetAxis(moveXC2) < 0 && ableToSwitchC2)
            {
                moveLeftC2 = true;
                moveRightC2 = false;
                if (!(startCoroutineOnceC2))
                    StartCoroutine(DelaySwitch());
            }
            else
            if (Input.GetAxis(moveXC2) > 0 && ableToSwitchC2)
            {
                moveRightC2 = true;
                moveLeftC2 = false;
                if (!(startCoroutineOnceC2))
                    StartCoroutine(DelaySwitch());
            }

            // Showing the controller for player 1
            RectTransform controller1Rect = controller1.GetComponent<RawImage>().rectTransform;
            // Moves the controller image
            if (moveLeft)
            {
                if (controllerStartPosX - 250 < controller1Rect.position.x)
                    controller1Rect.position = new Vector2(controller1Rect.position.x - 250, controller1Rect.position.y);
            }
            else
            if (moveRight)
            {
                if (controllerStartPosX + 250 > controller1Rect.position.x)
                    controller1Rect.position = new Vector2(controller1Rect.position.x + 250, controller1Rect.position.y);
            }

            // Showing both arrows
            if (controllerStartPosX == controller1Rect.position.x)
            {
                controller1.transform.GetChild(0).gameObject.SetActive(true);
                controller1.transform.GetChild(1).gameObject.SetActive(true);
            }
            else
            // Showing left arrow
            if (controllerStartPosX < controller1Rect.position.x)
            {
                controller1.transform.GetChild(0).gameObject.SetActive(false);
                controller1.transform.GetChild(1).gameObject.SetActive(true);
            }
            else
            // Showing right arrow
            if (controllerStartPosX > controller1Rect.position.x)
            {
                controller1.transform.GetChild(0).gameObject.SetActive(true);
                controller1.transform.GetChild(1).gameObject.SetActive(false);
            }

            // Showing the controller for player2
            RectTransform controller2Rect = controller2.GetComponent<RawImage>().rectTransform;
            // Moves the controller image
            if (moveLeftC2)
            {
                if (controllerStartPosX - 250 < controller2Rect.position.x)
                    controller2Rect.position = new Vector2(controller2Rect.position.x - 250, controller2Rect.position.y);
            }
            else
            if (moveRightC2)
            {
                if (controllerStartPosX + 250 > controller2Rect.position.x)
                    controller2Rect.position = new Vector2(controller2Rect.position.x + 250, controller2Rect.position.y);
            }

            // Showing both arrows
            if (controllerStartPosX == controller2Rect.position.x)
            {
                controller2.transform.GetChild(0).gameObject.SetActive(true);
                controller2.transform.GetChild(1).gameObject.SetActive(true);
            }
            else
            // Showing left arrow
            if (controllerStartPosX < controller2Rect.position.x)
            {
                controller2.transform.GetChild(0).gameObject.SetActive(false);
                controller2.transform.GetChild(1).gameObject.SetActive(true);
            }
            else
            // Showing right arrow
            if (controllerStartPosX > controller2Rect.position.x)
            {
                controller2.transform.GetChild(0).gameObject.SetActive(true);
                controller2.transform.GetChild(1).gameObject.SetActive(false);
            }

            if (Input.GetButton(aButton))
            {
                // Checking if the controllers aren't left in the start position or are placed to control the same character
                if (controller1Rect.position.x == controllerStartPosX || controller2Rect.position.x == controllerStartPosX || controller1Rect.position.x == controller2Rect.position.x)
                {
                    Text error = GameObject.Find("ErrorMessage").GetComponent<Text>();
                    error.text = "Both players need to choose different characters.";
                }
                else
                    SceneManager.LoadScene("Prototype4");
            }

            if(Input.GetButton(bButton))
            {
                SceneManager.LoadScene("MainMenu");
            }
        } // end of if sceneName = StartScene

        if(sceneName == "Options")
        {
            if (Input.GetButton(bButton))
            {
                SceneManager.LoadScene("MainMenu");
            }
        }
    }
}
