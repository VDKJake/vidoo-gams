using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FluffoPickup : MonoBehaviour
{
    private GameObject player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject == player)
        {
            player.GetComponent<PlayerExpand>().Expand();
            Destroy(this.gameObject);
        }
    }
}
