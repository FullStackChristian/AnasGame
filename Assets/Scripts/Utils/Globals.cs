using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Globals : MonoBehaviour
{
    //static references
    
    //Scenes
    public static Globals controller;
    public static string Startscreen = "MainMenu";
    public static string MainMenu = "MainMenu";
    public static string Puzzle = "Puzzle";
    public static string Platforms = "Platform";

    //Tags
    public static string PuzzleBox = "PuzzleBox";

    //Data to persist across scenes
    public List<string> MapLocations = new List<string> { "MainMenu", "Platforms", "Puzzle" };
    public string currentLocationType;


    // Start is called before the first frame update
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        //Check if the control instance is null
        if (controller == null)
        {
            //This instance becomes the single instance available
            controller = this;
        }
        //Otherwise check if the control instance is not this one
        else if (controller != this)
        {
            //In case there is a different instance destroy this one.
            Destroy(gameObject);
        }

        currentLocationType = Startscreen;
    }
}
