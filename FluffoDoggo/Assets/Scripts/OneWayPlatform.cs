using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneWayPlatform : MonoBehaviour {
    //NOTES
    //The trigger collider that this script references, needs to start under the platform, and finish before the top of the platform.

    BoxCollider2D[] colliders;
    BoxCollider2D platform;

	void Start ()
    {
        colliders = GetComponents<BoxCollider2D>(); //Grab the colliders. I's like to find a better way to do this, that doesn't grab the trigger collider too
        foreach(BoxCollider2D c in colliders)
        {
            if(!c.isTrigger)
            {
                platform = c;
                break;
            }
        }
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player") //Check that the player has entered the trigger
            Physics2D.IgnoreCollision(collision, platform); //If the collider is not the trigger collider, this collider will ignore collisions with the player
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
            Physics2D.IgnoreCollision(collision, platform, false); //un-ignoring collisions with the player
    }
}
