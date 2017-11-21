using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FluffoPickup : MonoBehaviour
{
    private GameObject player;
    private bool used;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        used = false;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (used == false)
        {
            if (collider.gameObject == player)
            {
                used = true;
                player.GetComponent<PlayerExpand>().Expand();
                Destroy(this.gameObject);
            }
        }
    }
}
