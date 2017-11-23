using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    /*
     * TODO
     * in CheckWall() need to make sure object hit is a wall and not anything (ramp, enemy, pick up)
     * There is a bug with slopes. If you stop your movement on a slope, you will skip up a slope in the opposite direction to the direction you are holding
     */
    public Animator faceAnimator;
    public Animator feetAnimator;

    //serializefield keeps the variable private but displays it in the editor -> remove for release and change to a const with the final value
    [SerializeField]
    private  float JUMPHEIGHT; //was 450f, using size to alter this
    private Rigidbody2D rb;
    private float moveSpeed;
    private float minSpeed = 1.5f; //The minimum starting speed of the player. Should be positive
    private float slowFactor = 4f; //The factor at which the player can slow themselves down. Should be positive
    private float airSpeed = 0.75f; //The factor at which playes can move themselves in the air
    private bool movingLeft;
    private bool onSlope;
    private bool jumpInput;

    private float groundDistance = 0.2f;
    private CircleCollider2D circleCollider;
    private LayerMask mask;
    private PhysicsMaterial2D bouncyMat;
    private PhysicsMaterial2D noBouncyMat;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        circleCollider = GetComponent<CircleCollider2D>();
        mask = LayerMask.GetMask("Ground");
        jumpInput = false;
        bouncyMat = Resources.Load<PhysicsMaterial2D>("Materials/Bouncy"); //Used in the toggle material function
        noBouncyMat = Resources.Load<PhysicsMaterial2D>("Materials/No-Bouncy"); 
    }

    private void Update()
    {
        JUMPHEIGHT = 450F * (2 * GameObject.Find("TempBG").transform.localScale.x); //Using the scale to change the jump height. This is probably really wasteful. Maybe put it into the pickup code?

        //if player is on the ground and presses the jump button -> jump
        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            if ((rb.velocity.y >= 0))
            {
                rb.AddForce(new Vector2(0, JUMPHEIGHT));
            }
            else if ((rb.velocity.y < 0) && jumpInput == false)
            {
                jumpInput = true;
            }
        }

        //accelerate with input on the horizontal axis
        if (IsGrounded())
        {
            moveSpeed += (Input.GetAxis("Horizontal") * Time.deltaTime) * GameObject.Find("TempBG").transform.localScale.x;

            if ((movingLeft && Input.GetAxis("Horizontal") > 0) || (!movingLeft && Input.GetAxis("Horizontal") < 0)) //If axis points right and the player moves left OR If axis points left and the player moves right
            {
                moveSpeed += (slowFactor * (Input.GetAxis("Horizontal") * Time.deltaTime) * GameObject.Find("TempBG").transform.localScale.x); //Quickly slow the doggo down
            }

            if (movingLeft && Input.GetAxis("Horizontal") < 0) //If axis and player both point left
            {
                if (moveSpeed > -minSpeed) moveSpeed = -minSpeed; //Set minimum initial speed
            }
            else if (!movingLeft && Input.GetAxis("Horizontal") > 0) //If axis and player both point right
            {
                if (moveSpeed < minSpeed) moveSpeed = minSpeed; //Set minimum initial speed
            }
        }
        else
        {
            moveSpeed += (airSpeed * (Input.GetAxis("Horizontal") * Time.deltaTime) * GameObject.Find("TempBG").transform.localScale.x); //If the player is not grounded, apply half the movement. So players can control themselves mid-air
        }
            

        //if no input decrease speed depending on direction if not on a slope or in the air -> slope allows for additional acceleration from the slope still
        if (!onSlope && IsGrounded())
        {
            if (moveSpeed < 0 && Input.GetAxis("Horizontal") == 0 && rb.velocity.x != 0)
                moveSpeed += Time.deltaTime * 2;
            else if (moveSpeed > 0 && Input.GetAxis("Horizontal") == 0 && rb.velocity.x != 0)
                moveSpeed -= Time.deltaTime * 2;
        }

        //ajust facing direction variable for wall bouncing
        if (movingLeft && moveSpeed > 0)
            movingLeft = !movingLeft;
        else if (!movingLeft && moveSpeed < 0)
            movingLeft = !movingLeft;

        //reverses the speed if doggo hits a wall
        if (CheckWall())
            moveSpeed = -moveSpeed;

        //get the normal.x of the ground doggo is on
            RaycastHit2D hit = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y - (circleCollider.bounds.extents.y + 0.01f)), Vector2.down, groundDistance, mask);
            if (hit)
            {
                if (hit.collider.tag == "Ground" && hit.normal.x != 0)
                {
                    //if no input add the scale of doggo (not required if there is input because it is already added)
                    if (Input.GetAxis("Horizontal") == 0)
                        moveSpeed += (hit.normal.x / 20) * GameObject.Find("TempBG").transform.localScale.x;
                    else
                    {
                        //apply normal.x to moveSpeed to account for slope angle and set on slope
                        moveSpeed += (hit.normal.x / 20);
                        onSlope = true;
                    }
                }
                else
                    onSlope = false;
            }

        //apply velocity changes
        float y = Mathf.Clamp(rb.velocity.y, -10, 10);
        rb.velocity = new Vector2(moveSpeed, y);
        faceAnimator.SetFloat("Movespeed", rb.velocity.x);
        feetAnimator.SetFloat("Movespeed", rb.velocity.x);
    }

    private bool IsGrounded()
    {
        // Checks if the bottom of the circle is within a very short distance of something
        // It's kinda shitty but it works for now. Will probably need to be changed tho
        return Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y - (circleCollider.bounds.extents.y + 0.3f)), Vector2.down, groundDistance, mask);
    }

    private bool CheckWall()
    {
        RaycastHit2D hit;
        //checks if there is a wall depending on direction traveling -> shouldn't need a bottom collision check
        if (movingLeft)
        {
            hit = Physics2D.Raycast(new Vector2(transform.position.x - (circleCollider.bounds.extents.x + 0.01f), transform.position.y + (circleCollider.bounds.extents.y + 0.01f)), Vector2.left, groundDistance, mask); //top left
            if(hit && hit.collider.tag == "Ground")
                return true;

            hit = Physics2D.Raycast(new Vector2(transform.position.x - (circleCollider.bounds.extents.x + 0.01f), transform.position.y), Vector2.left, groundDistance, mask); //center left
            if (hit && hit.collider.tag == "Ground")
                return true;
        }
        else
        {
            hit = Physics2D.Raycast(new Vector2(transform.position.x + (circleCollider.bounds.extents.x + 0.01f), transform.position.y + (circleCollider.bounds.extents.y + 0.01f)), Vector2.right, groundDistance, mask); //top right
            if (hit && hit.collider.tag == "Ground")
                return true;

            hit = Physics2D.Raycast(new Vector2(transform.position.x + (circleCollider.bounds.extents.x + 0.01f), transform.position.y), Vector2.right, groundDistance, mask); //center right
            if (hit && hit.collider.tag == "Ground")
                return true;
        }
        //hit returns true if there is a hit on a wall
        return false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (jumpInput == true)
        {
            rb.AddForce(new Vector2(0, JUMPHEIGHT));
            jumpInput = false;
        }
    }

    public void Bounce() //I've added this as a public function, so the actual bouncy platforms can just tell it to bounce.
    {
        //Debug.Log("Bounce Called");
        if (rb.velocity.y != 0) //If the player is airborne, add a force equal to twice the jump height
        {
            rb.AddForce(new Vector2(0, JUMPHEIGHT * 2));
        }
        //else Debug.Log("Player Was Grounded");
    }

    public void Stop() //Public function for the same reason as bounce
    {
        //Debug.Log("Stop Called");
        if (rb.velocity.y != 0) //If the player is airborne, kill their momentum
        {
            moveSpeed = 0;
            rb.velocity = new Vector2(rb.velocity.x, 0);
        }
        //else Debug.Log("Player was Grounded");
    }

    public void ToggleMaterial() //Toggles the circle colliders material between the bouncy material and null
    {
        if (circleCollider.sharedMaterial != bouncyMat)
            circleCollider.sharedMaterial = bouncyMat;
        else circleCollider.sharedMaterial = noBouncyMat;
    }

    public void changeParent(GameObject obj) //Switches the parent of the player between null and a supplied game object
    {
        if (transform.parent == null)
            transform.parent = obj.transform;
        else transform.parent = null;
    }
}
