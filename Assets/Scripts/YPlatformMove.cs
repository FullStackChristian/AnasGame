using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YPlatformMove : MonoBehaviour
{
    float dirX, moveSpeed = 3f;
    bool moveUp = true;
    //specify here between which params the platform should move:
    public float top;
    public float bottom;


    // Update is called once per frame
    void Update()
    {


        if (transform.position.y > top)
        { moveUp = false; }
        if (transform.position.y < bottom)
        { moveUp = true; }

        if (moveUp)
        {
            transform.position = new Vector2(transform.position.x, transform.position.y + moveSpeed * Time.deltaTime);
        }
        else
        {
            transform.position = new Vector2(transform.position.x, transform.position.y - moveSpeed * Time.deltaTime);
        }
    }
}
