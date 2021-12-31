using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XPlatformMove : MonoBehaviour
{
    float dirX, moveSpeed = 3f;
    bool moveRight = true;
    //specify here between which params the platform should move:
    public float positionRight;
    public float positionLeft;

   
    // Update is called once per frame
    void Update()
    {

        
        if (transform.position.x > positionRight)
        { moveRight = false; }
        if (transform.position.x < positionLeft)
        { moveRight = true; }

        if (moveRight)
        {
            transform.position = new Vector2(transform.position.x + moveSpeed * Time.deltaTime, transform.position.y);
        }
        else
        {
            transform.position = new Vector2(transform.position.x - moveSpeed * Time.deltaTime, transform.position.y);
        }
    }
}
