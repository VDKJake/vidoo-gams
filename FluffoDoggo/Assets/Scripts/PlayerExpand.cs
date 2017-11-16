using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerExpand : MonoBehaviour {

    //To-do / Temp Stuff
    //The expand and contract functions increase the size of a child object called "TempBG". This is just so we can see how big the collider is.
    //It will need to be changed later on when we add in all the fur and stuff.
    //We will need to add some function that checks if the doggo can fit wherever it is, before it expands.

    //Some other notes
    //The functions offset the position of the character, as the changes in radius go from the centre. Hopefully this will avoid weird collision things
    //I think so the doggo's size can interact with the speed and acceleration, we should use some sort of variable in both.

    private CircleCollider2D circleCollider;
    private GameObject bg;
    private float baseRadius;

	// Use this for initialization
	void Start ()
    {
        circleCollider = GetComponent<CircleCollider2D>();
        baseRadius = circleCollider.bounds.extents.x;
        bg = transform.Find("TempBG").gameObject;
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetKeyDown(KeyCode.Keypad0)) //This is just to test it out. When this is implemented properly, we will need to stop both being triggered in the same update frame.
        {
            Expand();
        }else if (Input.GetKeyDown(KeyCode.Keypad1))
        {
            Contract();
        }
	}

    public void Expand()
    { //Make doggo bigger
        transform.position = new Vector2(transform.position.x, transform.position.y + 0.1F); //Change position to offset the expansion of the collider first
        circleCollider.radius += 0.1F; //Add 0.1 to the radius
        bg.transform.localScale = new Vector2(bg.transform.localScale.x + 0.2F, bg.transform.localScale.y + 0.2F); //Increase the background to compensate
    }

    public void Contract()
    { //Make doggo smaller, reversing the steps in Expand
        if (circleCollider.radius > baseRadius) //So doggo can't shrink more than default
        {
            bg.transform.localScale = new Vector2(bg.transform.localScale.x - 0.2F, bg.transform.localScale.y - 0.2F); //Shrink Background
            circleCollider.radius -= 0.1F; //Take 0.1 off the radius
            transform.position = new Vector2(transform.position.x, transform.position.y - 0.1F); //Offset the position
        }
    }
}
