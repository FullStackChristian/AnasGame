using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMaster : MonoBehaviour
{
    private static GameMaster instance;
    private PlayerStats player;
    public Vector2 lastCheckPointPos;
    //[SerializeField]
    //private AudioClip mainTheme;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>();
        lastCheckPointPos = player.transform.position;
        //AudioSource.PlayClipAtPoint(mainTheme, transform.position);
    }
    
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    //Trigger Save System
    public void SavePlayer()
    {
        SaveSystem.SavePlayer(player);
    }
    public void LoadPlayer()
    {
        PlayerData data = SaveSystem.LoadPlayer();

        player.currentHealth = data.health;
        CameraCount.scoreAmount = data.cameraCount;

        Vector3 position;
        position.x = data.position[0];
        position.y = data.position[1];
        position.z = data.position[2];
        player.transform.position = position;
    }

    public void LoadScene(string sceneToLoad, string newLocationType)
    {
        SceneManager.LoadScene(sceneToLoad, LoadSceneMode.Single);
        Globals.controller.currentLocationType = newLocationType;

    }
}
