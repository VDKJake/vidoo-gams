using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    /*
     * TODO
     * add slope acceleration
     */

    private Rigidbody2D rb;
    private const float JUMPHEIGHT = 300f;
    public float moveSpeed;
    private bool movingLeft;

    private float groundDistance = 0.2f;
    private CircleCollider2D circleCollider;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        circleCollider = GetComponent<CircleCollider2D>();
    }

    private void Update()
    {
        //if player is on the ground and presses the jump button -> jump
        if (Input.GetButtonDown("Jump") && IsGrounded())
            rb.AddForce(new Vector2(0, JUMPHEIGHT));

        //accelerate with input on the horizontal axis
        if(IsGrounded())
            moveSpeed += Input.GetAxis("Horizontal") * Time.deltaTime;

        //if no input decrease speed depending on direction
        if (moveSpeed < 0 && Input.GetAxis("Horizontal") == 0 && rb.velocity.x != 0)
            moveSpeed += Time.deltaTime * 2;
        else if (moveSpeed > 0 && Input.GetAxis("Horizontal") == 0 && rb.velocity.x != 0)
            moveSpeed -= Time.deltaTime * 2;

        //ajust facing direction variable for wall bouncing
        if (movingLeft && moveSpeed > 0)
            movingLeft = !movingLeft;
        else if (!movingLeft && moveSpeed < 0)
            movingLeft = !movingLeft;

        //reverses the speed if doggo hits a wall
        if (CheckWall())
            moveSpeed = -moveSpeed;

        //get the normal.x of the ground doggo is on
        RaycastHit2D hit = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y - (circleCollider.bounds.extents.y + 0.01f)), Vector2.down, groundDistance);
        if (hit)
        {
            if (hit.collider.tag == "Ground")
            {
                //apply normal.x to moveSpeed to account for slope angle
                moveSpeed += hit.normal.x / 5;
            }
        }

        //apply velocity changes
        rb.velocity = new Vector2(moveSpeed, rb.velocity.y);
    }

    private bool IsGrounded()
    {
        // Checks if the bottom of the circle is within a very short distance of something
        // It's kinda shitty but it works for now. Will probably need to be changed tho
        return Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y - (circleCollider.bounds.extents.y + 0.01f)), Vector2.down, groundDistance);
    }

    private bool CheckWall()
    {
        if (movingLeft)
            return Physics2D.Raycast(new Vector2(transform.position.x - (circleCollider.bounds.extents.x + 0.01f), transform.position.y), Vector2.left, groundDistance);
        else
            return Physics2D.Raycast(new Vector2(transform.position.x + (circleCollider.bounds.extents.x + 0.01f), transform.position.y), Vector2.right, groundDistance);
    }
}
