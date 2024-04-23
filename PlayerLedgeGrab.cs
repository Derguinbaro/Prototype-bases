using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLedgeGrab : MonoBehaviour
{
    private bool greenBox;
    private bool redBox;
    public float redXOffset;
    public float redYOffset;
    public float redXSize;
    public float redYSize;
    public float greenXOffset;
    public float greenYOffset;
    public float greenXSize;
    public float greenYSize;
    private Rigidbody2D rb;
    private float startGrab;
    public LayerMask groundLayer;



    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        startGrab = rb.gravityScale;
    }


    void Update()
    {
        greenBox = Physics2D.OverlapBox (new Vector2 (transform.position.x + (greenXOffset * transform.localScale.x), transform.position.y + greenYOffset), new Vector2 (greenXSize, greenYSize), 0f, groundLayer);
        redBox = Physics2D.OverlapBox(new Vector2(transform.position.x + (redXOffset * transform.localScale.x), transform.position.y + redYOffset), new Vector2 (redXSize, greenYSize), 0f, groundLayer);

        if ( greenBox && !redBox && !playerVariables.isGrabbing && playerVariables.isJumping)
        {
            playerVariables.isGrabbing = true;
        }

        if (playerVariables.isGrabbing)
        {
            rb.velocity = new Vector2(0f, 0f);
            rb.gravityScale = 0f;
        }
    }

    public void ChangePos()
    {
        transform.position = new Vector2(transform.position.x + (0.5f * transform.localScale.x), transform.position.y + 0.4f);
        rb.gravityScale = startGrab;
        playerVariables.isGrabbing = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(new Vector2(transform.position.x + (redXOffset * transform.localScale.x), transform.position.y + redYOffset), new Vector2(redXSize, redYSize));
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(new Vector2(transform.position.x + (greenXOffset * transform.localScale.x), transform.position.y + greenYOffset), new Vector2(greenXSize, greenYSize));
    }
}
