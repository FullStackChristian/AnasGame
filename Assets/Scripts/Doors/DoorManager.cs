using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class DoorManager : MonoBehaviour
{
    public string sceneToLoad;
    //public string newLocationType;

    [HideInInspector]
    public int LocationIdx;
    [HideInInspector]
    public List<string> LstLocationtypes = new List<string> (new string[] { Globals.MainMenu, Globals.Platforms, Globals.Puzzle });

    public GameObject LevelManager;
    public Sprite icon;
    public string sampleText, textNoKey, textWithKey;

    [SerializeField]
    PlayerStats player;

    //Seconds to wait timers
    public float secondsToWait = 3;

    //Control chat bubble
    
    Transform chatBubbleTrnsfrm;
    ChatBubble chatBubble;
    Transform plyrBackground;
    Transform plyrIcon;
    Transform plyrText;

    bool triggered = false;

    private IEnumerator OnTriggerEnter2D(Collider2D collision)
    {
        //display a text bubble to show what level they are enteringd
        chatBubbleTrnsfrm = collision.transform.Find("ChatBubble");
        chatBubble = chatBubbleTrnsfrm.GetComponent<ChatBubble>();

        //Display bubble
        chatBubble.DisplayChatBubble(icon, sampleText);

        //Set text to be active when in contact with door
        objectsEnabled(true);

        //display animation like a bar at the top counting down to middle e.g. -------------        ----------         ----- (Should be centered on top middle of screen I think, should be larger at ends and shorter in middle)


        //Wait for a few seconds so user can change their mind 
        triggered = true;
        
        if (triggered && player.keyCount > 0)
        {
            //Display bubble
            chatBubble.DisplayChatBubble(icon, textWithKey);
            yield return new WaitForSeconds(secondsToWait);
            //Load the desired Level
            
                LevelManager.GetComponent<LevelManager>().LoadScene(sceneToLoad, LstLocationtypes[LocationIdx]);
           
        }
        else if (triggered && player.keyCount == 0)
        {
            chatBubble.DisplayChatBubble(icon, textNoKey);
            
        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        //turn off when not in contact with door
        objectsEnabled(false);

        triggered = false;
    }

    void objectsEnabled(bool enabled)
    {
        plyrBackground = chatBubbleTrnsfrm.transform.Find("Background"); 
        plyrBackground.gameObject.SetActive(enabled);
        
        plyrIcon = chatBubbleTrnsfrm.transform.Find("Icon"); 
        plyrIcon.gameObject.SetActive(enabled);
        plyrIcon.GetComponent<SpriteRenderer>().sprite = icon;

        plyrText = chatBubbleTrnsfrm.transform.Find("Text");
        plyrText.gameObject.SetActive(enabled);
    }
    
}
