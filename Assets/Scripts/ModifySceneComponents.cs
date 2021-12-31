using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ModifySceneComponents : MonoBehaviour
{
    private string currentScene;
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        if (!string.IsNullOrEmpty(Globals.controller.currentLocationType))
            ModifyScene();

        currentScene = SceneManager.GetActiveScene().name;
    }

    private void FixedUpdate()
    {
        if (currentScene != SceneManager.GetActiveScene().name)
        {
            ModifyScene();
            currentScene = SceneManager.GetActiveScene().name;
        }

    }

    //Method to activate and deactive things based on the scene they are in
    public void ModifyScene()
    {
        //Initialise variables
        var plyr = GameObject.FindGameObjectWithTag("Player");
        var HlthBar = plyr.transform.Find("HealthBar"); 

        //Deactivate components based on the level design
        if (Globals.controller.currentLocationType == Globals.MainMenu)
        {
            plyr.GetComponentInChildren<LineRendererScript>().enabled = false;
            plyr.GetComponentInChildren<ChatBubble>().enabled = true;
           // plyr.GetComponentInChildren<HealthController>().enabled = false;
            HlthBar.gameObject.SetActive(false);
        }
        else if (Globals.controller.currentLocationType == Globals.Platforms)
        {
            plyr.GetComponentInChildren<LineRendererScript>().enabled = false;
            plyr.GetComponentInChildren<ChatBubble>().enabled = false;
            HlthBar.gameObject.SetActive(true);
        }
        else if (Globals.controller.currentLocationType == Globals.Puzzle)
        {
            plyr.GetComponentInChildren<LineRendererScript>().enabled = true;
            plyr.GetComponentInChildren<ChatBubble>().enabled = false;
            HlthBar.gameObject.SetActive(false);
        }
        else
            Debug.LogError("Level not specified");
    }
}
