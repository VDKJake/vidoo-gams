using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnbouncyPlatform : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && collision.gameObject.transform.position.y > gameObject.transform.position.y) //If the player enters the trigger and is above the platform
        {
            PlayerMovement player = collision.GetComponent<PlayerMovement>();
            player.ToggleMaterial(); //Disable the material
            player.Stop(); //Stop player's momentum
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player") //If player leaves trigger
        {
            PlayerMovement player = collision.GetComponent<PlayerMovement>();
            player.ToggleMaterial(); //Enable Material
        }
    }
}
