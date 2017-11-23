using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroopyPlatform : MonoBehaviour {

    //Notes + To do
    //The setting of the background stick assumes the platform is 2 units long
    //Change this to use variables we can set in inspector, for the distance and speed of movement.
    //Add a function to set the size of the stick in the background to match

    bool move; //true = descend, false = ascend
    public float limit; //How far downward the platform will sink. This should be a negative value
    public float speed; //How fast the platform will sink

	// Use this for initialization
	void Start () {
        move = false; //This should always be initialised to false

        GameObject s = transform.parent.transform.Find("Stick").gameObject;
        s.transform.localPosition = new Vector2(1, limit/2);
        s.transform.localScale = new Vector2(0.5f, -limit);
	}
	
	// Update is called once per frame
	void Update () {
        if (move)
        {
            if (transform.localPosition.y > limit) //If the platform is above the lower limit, translate it down
            {
                transform.localPosition -= transform.up * Time.deltaTime * speed; 
            }
        }
        else
        {
            if (transform.localPosition.y < 0) //If the platform is below the upper limit, translate it up
            {
                transform.localPosition += transform.up * Time.deltaTime * speed;
            }
        }
        //Debug.Log(gameObject.transform.localPosition.y);
        if (transform.localPosition.y > 0) transform.localPosition = new Vector2(0, 0); //To keep the platform position within the threshhold
        if (transform.localPosition.y < limit) transform.localPosition = new Vector2(0, limit);
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player") //If the player enters the trigger, begin to move the platform down
        {
            PlayerMovement player = collision.GetComponent<PlayerMovement>();
            player.ToggleMaterial();
            player.changeParent(gameObject); //Parent the player so they move smoothly with the platform
            move = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player") //If the players exits the trigger, begin to move the platform up
        {
            PlayerMovement player = collision.GetComponent<PlayerMovement>();
            player.ToggleMaterial();
            player.changeParent(gameObject); //Unparent the player
            move = false;
        }
    }
}
