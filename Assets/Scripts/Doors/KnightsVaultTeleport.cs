using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightsVaultTeleport : MonoBehaviour
{

    public GameObject player;
    public GameObject knightsVaultTeleportInside;
    public bool isInRange;

    private void Update()
    {
        if (isInRange)
        {
            if (Input.GetKeyDown(KeyCode.Backspace))
            {
                player.transform.position = knightsVaultTeleportInside.transform.position;
            }
        }
        else
        {
            Debug.Log("Player not in Range");
        }
        
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("PlayerHasEnteredKnightsVaultTrigger");
        isInRange = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Debug.Log("PlayerHasLeftKnightsVaultTrigger");
        isInRange = false;
    }
}
