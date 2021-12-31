using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using Assets.Scripts;
using UnityEngine.Tilemaps;

public class LineRendererScript : MonoBehaviour
{
    //Public
    public float maxRange;
    public float minRange;
    public LayerMask whatToHit;
    public LayerMask whatIsGround;
    public float MaxTimeBetweenPushes;
    public GameObject tilemap;
    public GameObject ana;
    public GameObject timPrefab;

    //Private
    private LineRenderer lineRenderer;
    
    private float TimeBetweenPushes;
    //public BoxCollider2D boxCollider;

    // Use this for initialization
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {

        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //mousePos.y = transform.position.y;//this ensures that the end of the line is at the level of the player.

        if (TimeBetweenPushes < 0) //then you can push
        {
            if (Input.GetMouseButton(0))
            {

                //Vector3 startPos = new Vector3 (transform.position.x * distanceAwayFromPlayer.x, transform.position.y * distanceAwayFromPlayer.y, transform.position.z);
                Vector3 startPos = transform.position;
                Vector3 endPos = new Vector3 (mousePos.x, mousePos.y);

                float dist = Vector3.Distance(startPos, endPos); 
                if (dist > maxRange || dist < minRange) //Could add code that checks for gaps here //Only render line if close enough to box and there is a space between 
                    return;

                lineRenderer.SetPosition(0, startPos);
                lineRenderer.SetPosition(1, endPos);
                //Debug.Log("Line Renderer positions : Start - " + startPos + " : End - " + endPos);

                lineRenderer.enabled = true;

                //for switching the way the ray faces
                /*if (endPos.x < 0)ad
                    endPos.x = endPos.x * -1;*/

                RaycastHit2D hit = FireLineCast(ana.transform.position, endPos);
                if (BoxHit(hit, ana.transform.position, endPos)) //if hit something and action has happened then add cooldown for animation
                    TimeBetweenPushes = MaxTimeBetweenPushes;

            }
        }
        else
        {
            TimeBetweenPushes -= Time.deltaTime;
        }

        if (Input.GetMouseButtonUp(0))
        {
            lineRenderer.enabled = false;
        
    }
    }

    private bool BoxHit(RaycastHit2D hit, Vector2 origin, Vector2 direction)
    {
        Vector2 forceDirection;
        bool canPush;

        if (hit.collider != null)
        {
            if (hit.collider.tag.Equals("BoxPuzzle"))
            {
                forceDirection = origin.x < direction.x ? Vector2.right : Vector2.left;

                if (CanBoxBePushed(hit.collider.gameObject, forceDirection)) // Make sure the box has enough room to be moved i.e. Can Tim's animation fit in the space between Ana and box and is there any gaps that Tim would fall down
                {
                    canPush = true;

                    Positions boxBounds = GetBoundsAndSize(hit.collider.gameObject);

                    //Give force to box
                    Debug.Log("Collided with box : " + hit.collider.gameObject.name);

                    //Check if there is any colliders in the way of pushing the box, if so then pull the box instead
                    if (SpaceBehindObject(boxBounds, forceDirection))
                        canPush = false;

                    if (canPush)
                    //Using add force
                        hit.collider.gameObject.GetComponent<Rigidbody2D>().AddForce(forceDirection * (hit.collider.gameObject.GetComponent<Rigidbody2D>().mass * 200f));
                    else
                        hit.collider.gameObject.GetComponent<Rigidbody2D>().AddForce(forceDirection * -1 * (hit.collider.gameObject.GetComponent<Rigidbody2D>().mass * 200f));
                    hit.collider.gameObject.GetComponent<AudioSource>().Play();
                    /*//Instantiate Tim on front of box
                    var timPositon = forceDirection == Vector2.right ? new Vector2(boxBounds.Left.x - boxBounds.Width / 2f, boxBounds.Left.y) : new Vector2(boxBounds.Right.x + boxBounds.Width / 2f, boxBounds.Right.y);
                    Instantiate(timPrefab, timPositon, Quaternion.identity);*/

                    //Create animation for Tim to be pushing the box 

                    return true;
                }
                

                //Could make it if there is not enough room to push then Tim could do a pull
            }
            else
            {
                Debug.Log("No function for : " + hit.collider.gameObject.name);
            }

            

        }
        return false;
    }

    

    void OnMouseDown()
    {
        Debug.Log("Mouse Clicked");
    }

    private RaycastHit2D FireLineCast(Vector2 origin, Vector2 direction)
    {
        Debug.DrawLine(origin, direction, Color.blue);
        RaycastHit2D hit = Physics2D.Linecast(origin, direction, whatToHit);

        return hit;
    }

    private bool CanBoxBePushed(GameObject gameObject, Vector2 forceDirection)
    {
        bool roomForTim = false;
        bool noGapsBetween = false;

        //Box's positions
        Positions boxBounds = GetBoundsAndSize(gameObject);

        //Ana's positions
        Positions AnaPositions = GetBoundsAndSize(ana);

        //Determine if there is enough room in front of Ana - should be 1 box width between Ana and the box at least for Tim to spawn
        var distBtwAnaAndObject = forceDirection == Vector2.right ? Mathf.Abs(AnaPositions.Right.x - boxBounds.Left.x) : Mathf.Abs(AnaPositions.Left.x - boxBounds.Right.x);
        if (distBtwAnaAndObject > boxBounds.Width)
        {
            Debug.Log("Room for Tim");
            roomForTim = true;
        }

        //Is there a gap 1 block onfront and 1 below Ana's position
        Vector2 checkArea = forceDirection == Vector2.right ? new Vector2(boxBounds.Left.x - boxBounds.Width, boxBounds.Left.y - boxBounds.Height) : new Vector2(boxBounds.Right.x + boxBounds.Width, boxBounds.Right.y - boxBounds.Height);
        Debug.Log("Checking area : " + checkArea);

        DebugDrawBox(checkArea, new Vector2 { x = boxBounds.Width / 3, y = boxBounds.Height / 3 }, 0f, Color.cyan, 5);
        Collider2D[] hits = Physics2D.OverlapBoxAll(checkArea, new Vector2 { x = boxBounds.Width / 3, y = boxBounds.Height / 3 }, 0f, whatIsGround);

        foreach (var hit in hits)
        {
            Debug.Log("Floor available for Tim");
            noGapsBetween = true;
        }

        if (hits.Length == 0)
            Debug.Log("No floor available for Tim");

        if (roomForTim && noGapsBetween)
            return true;

        return false;
    }

    private bool SpaceBehindObject(Positions boxBounds, Vector2 forceDirection)
    {
        Vector2 checkArea = forceDirection == Vector2.right ? new Vector2(boxBounds.Right.x, boxBounds.Right.y) : new Vector2(boxBounds.Left.x, boxBounds.Left.y);
        Debug.Log("Checking area : " + checkArea);

        DebugDrawBox(checkArea, new Vector2 { x = boxBounds.Width / 4, y = boxBounds.Height / 4 }, 0f, Color.green, 5);
        Collider2D[] hits = Physics2D.OverlapBoxAll(checkArea, new Vector2 { x = boxBounds.Width / 4, y = boxBounds.Height / 4 }, 0f, whatIsGround);

        foreach (var hit in hits)
        {
            Debug.Log("Space behind box to push");
            return true;
        }

        Debug.Log("Collider behind object");
        return false;
    }

    private Positions GetBoundsAndSize(GameObject gameObject)
    {
        //Get the size
        float width = gameObject.GetComponent<BoxCollider2D>().bounds.size.x;
        float height = gameObject.GetComponent<BoxCollider2D>().bounds.size.y;

        Positions positions = new Positions { Top = gameObject.transform.position, Bottom = gameObject.transform.position, Right = gameObject.transform.position, Left = gameObject.transform.position, Width = width, Height = height };

        //Convert to bounds
        positions.Top = new Vector3 (positions.Top.x, positions.Top.y + height / 2);         positions.Bottom = new Vector3 (positions.Bottom.x, positions.Bottom.y - height / 2);
        positions.Left = new Vector3 (positions.Left.x - width / 2, positions.Left.y);       positions.Right = new Vector3 (positions.Right.x + width / 2, positions.Right.y);

        return positions;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Mesh hit box");
    }

    void DebugDrawBox(Vector2 point, Vector2 size, float angle, Color color, float duration)
    {

        var orientation = Quaternion.Euler(0, 0, angle);

        // Basis vectors, half the size in each direction from the center.
        Vector2 right = orientation * Vector2.right * size.x / 2f;
        Vector2 up = orientation * Vector2.up * size.y / 2f;

        // Four box corners.
        var topLeft = point + up - right;
        var topRight = point + up + right;
        var bottomRight = point - up + right;
        var bottomLeft = point - up - right;

        // Now we've reduced the problem to drawing lines.
        Debug.DrawLine(topLeft, topRight, color, duration);
        Debug.DrawLine(topRight, bottomRight, color, duration);
        Debug.DrawLine(bottomRight, bottomLeft, color, duration);
        Debug.DrawLine(bottomLeft, topLeft, color, duration);
    }
}