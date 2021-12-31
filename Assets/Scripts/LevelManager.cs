using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cinemachine;

public class LevelManager : MonoBehaviour
{
    [SerializeField]
    private Transform respawnPoint;

    [SerializeField]
    private GameObject player;

    [SerializeField]
    private float respawnTime;

    private float respawnTimeStart;

    private bool respawn;

    private CinemachineVirtualCamera CVC;


    private void Update()
    {
        CheckRespawn();
    }

    void Start()
    {
        CVC = GameObject.Find("PlayerCamera").GetComponent<CinemachineVirtualCamera>();
    }

    public void LoadScene(string sceneToLoad, string newLocationType)
    {
        SceneManager.LoadScene(sceneToLoad, LoadSceneMode.Single);
        Globals.controller.currentLocationType = newLocationType;

    }

    public void Respawn()
    {
        respawnTimeStart = Time.time;
        respawn = true;
    }

    private void CheckRespawn()
    {
        if (Time.time >= respawnTimeStart + respawnTime && respawn)
        {
            var playerTemp = Instantiate(player, respawnPoint);
            CVC.m_Follow = playerTemp.transform;
            respawn = false;
        }
    }
}
