using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Utils;
using UnityEngine;
using UnityEngine.UI;

public class TimKnightsVaulText : MonoBehaviour
{
    public GameObject canvas;
    public GameObject KnightsVaultTrigger;
    public Text dialogText;
    public string 
        dialog1,
        dialogWithCamera;
   
    public bool playerInRange;

    private void Update()
    {
        if (/*Input.GetKeyDown(KeyCode.Space)&&*/ playerInRange)
        {
            if (CameraCount.scoreAmount.Equals(3))
            {
                canvas.SetActive(true);
                dialogText.text = dialogWithCamera;
                KnightsVaultTrigger.SetActive(true);
            }
            else 
            {
                canvas.SetActive(true);
                dialogText.text = dialog1;
            }

        }

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(Constants.TAG_PLAYER))
        {
            Debug.Log("Player in range");
            playerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag(Constants.TAG_PLAYER))
        {
            Debug.Log("Player not in range");
            playerInRange = false;
            canvas.SetActive(false);
            KnightsVaultTrigger.SetActive(false);
        }
    }

}
