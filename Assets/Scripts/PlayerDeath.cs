using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Utils;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerDeath : MonoBehaviour
{

    private GameMaster gm;
    private GameObject player;

    private void Start()
    {
        gm = FindObjectOfType<GameMaster>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(Constants.TAG_PLAYER))
        {
            //Destroy(collision.gameObject);
            //SceneManager.LoadScene(SceneToLoad);
            player.transform.position = gm.lastCheckPointPos;
        }
    }
}
