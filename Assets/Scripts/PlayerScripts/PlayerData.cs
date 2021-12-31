using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData 
{
    public float health;

    public float[] position;

    public int 
        cameraCount,
        keyCount;
    
    

    public PlayerData (PlayerStats player)
    {
        health = player.currentHealth;
        cameraCount = CameraCount.scoreAmount;

        position = new float[3];
        position[0] = player.transform.position.x;
        position[1] = player.transform.position.y;
        position[2] = player.transform.position.z;
    }
}
