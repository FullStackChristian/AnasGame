using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Utils;
using UnityEngine;
using UnityEngine.UI;

public class SignDisplayText : MonoBehaviour
{
    public GameObject canvas;
    public Text dialogText;
    public string dialog;

    public bool playerInRange;

    private void Update()
    {
        if (/*Input.GetKeyDown(KeyCode.Space)&&*/ playerInRange)
        {

            canvas.SetActive(true);
            dialogText.text = dialog;
            
            
           //if (dialogBox.activeInHierarchy)
           //{
           //    dialogBox.SetActive(false);
           //}
           //else 
           //{
           //    dialogBox.SetActive(true);
           //    dialogText.text = dialog;
           //}
        }
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(Constants.TAG_PLAYER))
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag(Constants.TAG_PLAYER))
        {
            playerInRange = false;
            canvas.SetActive(false);
        }
    }

}
