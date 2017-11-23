using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour {

    //Notes
    //The setting of the background stick assumes the platform is 2 units long
    //Change this to use variables we can set in inspector, for the distance and speed of movement.

    public bool direction; //true = up/down, false = left/right
    public float distance; //How far the platform will go
    public float speed; //How fast the platform moves

	// Use this for initialization
	void Start () {
        GameObject s = transform.parent.transform.Find("Stick").gameObject;
        if (direction)
        {
            s.transform.localPosition = new Vector2(1, distance/2);
            s.transform.localScale = new Vector2(0.5f, distance);
        }
        else
        {
            s.transform.localPosition = new Vector2(distance - 1, -0.05f);
            s.transform.localScale = new Vector2(distance, 0.5f);
        }
    }
	
	// Update is called once per frame
	void Update () {
        if (direction) transform.localPosition = new Vector2(0, Mathf.PingPong(speed * Time.time, distance));
        else transform.localPosition = new Vector2(Mathf.PingPong(speed * Time.time, distance), 0);
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player") //If the player enters the trigger, begin to move the platform down
        {
            PlayerMovement player = collision.GetComponent<PlayerMovement>();
            player.ToggleMaterial(); 
            player.changeParent(gameObject); //Parent the player to this platform, so they move smoothly with it
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player") //If the players exits the trigger, begin to move the platform up
        {
            PlayerMovement player = collision.GetComponent<PlayerMovement>();
            player.ToggleMaterial();
            player.changeParent(gameObject); //Unparent the player
        }
    }
}
