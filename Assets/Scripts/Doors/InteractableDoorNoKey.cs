using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Utils;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts.Doors
{
    public class InteractableDoorNoKey : MonoBehaviour
    {

        public string sceneToLoad;
        //public string newLocationType;

        [HideInInspector]
        public int LocationIdx;
        [HideInInspector]
        public List<string> LstLocationtypes = new List<string>(new string[] { Globals.MainMenu, Globals.Platforms, Globals.Puzzle });

        //Interact with door vars
        public bool isInRange;
        public KeyCode interactKey;
        public UnityEvent interactAction;
        public Animator animator;
        public PlayerStats player;
        //Animator var isOpen
        public bool isOpenBool;

        //LevelManager to Load scenes
        public GameObject LevelManager;
        //icon displayer next to text
        public Sprite icon;
        //possible text entries
        public string sampleText, textNoKey, textWithKey;

        //chat bubble vars
        Transform chatBubbleTrnsfrm;
        ChatBubble chatBubble;
        //Transform plyrBackground;
        Transform plyrIcon;
        Transform plyrText;

        private void Start()
        {     
            animator = GetComponent<Animator>();
        }

        private void Update()
        {
            if (isInRange)
            {
                chatBubble.DisplayChatBubble(icon, sampleText);
                if (Input.GetKeyDown(interactKey))
                {
                    OpenDoor();
                    LevelManager.GetComponent<LevelManager>().LoadScene(sceneToLoad, LstLocationtypes[LocationIdx]);
                }
            }
            
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag(Constants.TAG_PLAYER))
            {
                isInRange = true;
                chatBubbleTrnsfrm = collision.transform.Find("ChatBubble");
                chatBubble = chatBubbleTrnsfrm.GetComponent<ChatBubble>();
                objectsEnabled(true);
                
                
                Debug.Log("Player is in Range");
            }
        }
        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag(Constants.TAG_PLAYER))
            {
                isInRange = false;
                Debug.Log("Player is not in Range");
                objectsEnabled(false);
            }
        }

        public void OpenDoor()
        {
            if (!isOpenBool)
            {
                isOpenBool = true;
                Debug.Log("Door is now open...");
                animator.SetBool("isOpen", isOpenBool);
            }
        }

        void objectsEnabled(bool enabled)
        {
           // plyrBackground = chatBubbleTrnsfrm.transform.Find("Background");
           // plyrBackground.gameObject.SetActive(enabled);

            plyrIcon = chatBubbleTrnsfrm.transform.Find("Icon");
            plyrIcon.gameObject.SetActive(enabled);
            plyrIcon.GetComponent<SpriteRenderer>().sprite = icon;

            plyrText = chatBubbleTrnsfrm.transform.Find("Text");
            plyrText.gameObject.SetActive(enabled);
        }

    }
}
