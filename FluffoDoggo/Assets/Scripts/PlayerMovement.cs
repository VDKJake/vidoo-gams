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
    [SerializeField]
    private const float JUMPHEIGHT = 300f;
    private float moveSpeed;
    private bool onSlope = false;

    private Transform groundChecker;
    private bool isGrounded = false;
    private float checkerRaduis = 0.2f;
    private LayerMask groundMask;
    private CircleCollider2D circleCollider;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        groundChecker = GameObject.Find("GroundChecker").transform;
        groundMask = LayerMask.GetMask("Ground");
        circleCollider = GetComponent<CircleCollider2D>();
    }

    private void Update()
    {
        //if player is on the ground and presses the jump button -> jump
        if (Input.GetButtonDown("Jump") && IsGrounded())
            rb.AddForce(new Vector2(0, JUMPHEIGHT));

        //accelerate with input on the horizontal axis
        moveSpeed += Input.GetAxis("Horizontal") * Time.deltaTime;

        //if no input decrease speed depending on direction
        if (moveSpeed < 0 && Input.GetAxis("Horizontal") == 0 && rb.velocity.x != 0)
            moveSpeed += Time.deltaTime;
        else if (moveSpeed > 0 && Input.GetAxis("Horizontal") == 0 && rb.velocity.x != 0)
            moveSpeed -= Time.deltaTime;
        //apply velocity changes
        rb.velocity = new Vector2(moveSpeed, rb.velocity.y);
    }

    private void FixedUpdate()
    {
        isGrounded = Physics2D.OverlapCircle(groundChecker.position, checkerRaduis, groundMask);
    }

    private bool IsGrounded()
    {
        // Checks if the bottom of the circle is within a very short distance of something
        // It's kinda shitty but it works for now. Will probably need to be changed tho
        return Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y - (circleCollider.bounds.extents.y + 0.01f)), Vector2.down, 0.1f);
    }
}
