using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraCount : MonoBehaviour
{
    public static int scoreAmount;
    private Text scoreText;

    private void Start()
    {
        scoreText = GetComponent<Text>();
        scoreAmount = 0;
    }

    private void Update()
    {
        scoreText.text = "Cameras: " + scoreAmount;
    }
}
