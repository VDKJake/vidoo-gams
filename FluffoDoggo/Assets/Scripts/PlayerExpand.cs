using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerExpand : MonoBehaviour
{
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
    private float previousTarget;
    private IEnumerator expand;
    private float maxFluffiness;

    [SerializeField]
    private Slider fluffSlider;
    [SerializeField]
    private Animator handleAnimator;

    // Use this for initialization
    void Start ()
    {
        circleCollider = GetComponent<CircleCollider2D>();
        baseRadius = circleCollider.bounds.extents.x;
        bg = GameObject.Find("TempBG");
        previousTarget = baseRadius;
        expand = ExpandOverTime(1.0f);

        // Max fluffiness level, only used for the fluffiness meter right now and is just equal to how big the doggo (circle collider) gets
        // when you get all the powerups in the current level (8). It takes away the base radius so the percent thing will work
        maxFluffiness = 1.46f - baseRadius;
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetKeyDown(KeyCode.Keypad0)) //This is just to test it out. When this is implemented properly, we will need to stop both being triggered in the same update frame.
            Expand();
        else if (Input.GetKeyDown(KeyCode.Keypad1))
            Contract();

        // Gets the percentage that the current radius of the circle is out of the max fluffiness
        // and sets the slider value and an animator value to that percentage. I'm kinda confused about it but it works
        // so I wont question it
        float percent = (circleCollider.radius - baseRadius) / maxFluffiness;
        fluffSlider.value = percent;
        handleAnimator.SetFloat("FluffValue", percent);
	}

    public void Expand()
    { //Make doggo bigger
        /*transform.position = new Vector2(transform.position.x, transform.position.y + 0.1F); //Change position to offset the expansion of the collider first
        circleCollider.radius += 0.1F; //Add 0.1 to the radius
        bg.transform.localScale = new Vector2(bg.transform.localScale.x + 0.2F, bg.transform.localScale.y + 0.2F); //Increase the background to compensate
        */

        // Sets the current target to whatever the previous target scale was + 0.1, starts ExpandOverTime and then sets the previous target
        // to whatever the current target is.
        // This is here so that if you're in the process of expanding and you get another pickup it wont 'cancel' the previous expand
        // and set the scale to like 0.36 or something
        float targetScale = (float)System.Math.Round(previousTarget + 0.1f, 2);
        StopCoroutine(expand);
        expand = ExpandOverTime(targetScale);
        StartCoroutine(expand);
        previousTarget = targetScale;
    }

    private IEnumerator ExpandOverTime(float targetScale)
    {
        // Does the same as expand but over time
        // Had to add the round thing because unity is a cunt and can go fuck itself
        do
        {
            transform.position = new Vector2(transform.position.x, (float)System.Math.Round(transform.position.y + 0.01F, 2));
            circleCollider.radius = (float)System.Math.Round(circleCollider.radius + 0.01f, 2);
            bg.transform.localScale = new Vector2((float)System.Math.Round(bg.transform.localScale.x + 0.02F, 2), (float)System.Math.Round(bg.transform.localScale.y + 0.02F, 2));
            yield return new WaitForSeconds(0.1f);
        } while (circleCollider.radius != targetScale);
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
