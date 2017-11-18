using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneWayPlatform : MonoBehaviour {
    //NOTES
    //The trigger collider that this script references, needs to start under the platform, and finish before the top of the platform.

    BoxCollider2D[] platform;

	// Use this for initialization
	void Start () {
        platform = GetComponentsInChildren<BoxCollider2D>(); //Grab the colliders. I's like to find a better way to do this, that doesn't grab the trigger collider too
        //This assumes that this is the parent object of each of the platform sprites
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player") //Check that the player has entered the trigger
        {
            foreach (BoxCollider2D b in platform)
            {
                if (b.isTrigger == false) Physics2D.IgnoreCollision(collision, b); //If the collider is not the trigger collider, this collider will ignore collisions with the player
                //Debug.Log("Collision OFF: " + b.gameObject.ToString());
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            foreach (BoxCollider2D b in platform)
            {
                if (b.isTrigger == false) Physics2D.IgnoreCollision(collision, b, false); //un-ignoring collisions with the player
                //Debug.Log("Collision ON: " + b.gameObject.ToString());
            }
        }
    }
}
