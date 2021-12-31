using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Utils;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerStats : MonoBehaviour
{
    [SerializeField]
    private float maxHealth;

    public float currentHealth;


    [SerializeField]
    private GameObject
        deathChunkParticle,
        deathBloodParticle;

    private LevelManager LM;

    public HealthBar healthBar;

    //For PickUp script

    public int cameraCount;
    public int keyCount;
    private GameMaster gm;

    private void Start()
    {
        cameraCount = CameraCount.scoreAmount;
        healthBar.GetComponent<HealthBar>();
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
        //LevelManager doesn't exist anymore
        //LM = GameObject.Find("LevelManager").GetComponent<LevelManager>();

        //For PickupScript
        gm = FindObjectOfType<GameMaster>();
        transform.position = gm.lastCheckPointPos;
    }

    private void Update()
    {
    }

    public void DecreaseHealth(float amount)
    {
        currentHealth -= amount;
        healthBar.SetHealth(currentHealth);


        if (currentHealth <= 0.0f)
        {
            //Die();
            Teleport();
        }
    }

    //old function to destroy the game Object and reload scene - not ideal as it gets rid of all the progress the player has achieved so far - all other gameObjects will be reset - cameras etc.. 
    // in order for this function to work - need to create a LevelManager with a respawn function
    private void Die()
    {

        LM.Respawn();
        Destroy(gameObject);
        Instantiate(deathChunkParticle, transform.position, deathChunkParticle.transform.rotation);
        Instantiate(deathBloodParticle, transform.position, deathBloodParticle.transform.rotation);

    }
    //Instead of destroying the gameObect on death - teleport to last checkpoint position and reset health to maxHealth so the player can restart from the last checkpoint 
    // - maybe add a function to reset BossHealth and reset Enemies (depends on the how difficult the game should get)
    private void Teleport()
    {
        try
        {
            transform.position = gm.lastCheckPointPos;
            currentHealth = maxHealth;
            Instantiate(deathChunkParticle, transform.position, deathChunkParticle.transform.rotation);
            Instantiate(deathBloodParticle, transform.position, deathBloodParticle.transform.rotation);
        }
        catch
        {
            Debug.LogError("couldn' find a position for lastCheckPointPos");
        }
        
    }

    //Platform Movement Script - so that the character can ride on the platform
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag.Equals("Platform"))
        {
            this.transform.parent = collision.transform;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag.Equals("Platform"))
        {
            this.transform.parent = null;
        }
    }
    public bool PickUpItem(GameObject obj)
    {
        switch (obj.tag)
        {
            case Constants.TAG_KEY:
                keyCount++;
                return true;
            case Constants.TAG_CAMERA:
                cameraCount++;
                CameraCount.scoreAmount++;
                return true;
            default:
                Debug.LogWarning($"WARNING: No handler implemented for tag {obj.tag}.");
                return false;
        }


    }
}

