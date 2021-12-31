using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.SceneManagement;

public class CustomSceneManager : MonoBehaviour
{
    public static CustomSceneManager instance;

    public Transform respawnPoint;
    public GameObject playerPrefab;

    public CinemachineVirtualCameraBase cam;

    private int prevSceneLoad;

    private void Awake()  //make this instance available in other scripts
    {
        instance = this;
    }

    public void Respawn()
    {
        GameObject player = Instantiate(playerPrefab, respawnPoint.position, Quaternion.identity);
        cam.Follow = player.transform;
        cam.LookAt = player.transform;
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name, LoadSceneMode.Single);
    }

    public void ButtonClickCont() //when GameOver screen, restart previous scene on click of button continue
    {
        prevSceneLoad = SceneManager.GetActiveScene().buildIndex - 1;
        SceneManager.LoadScene(prevSceneLoad);
    }
}
