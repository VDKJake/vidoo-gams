using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FluffoPickup : MonoBehaviour
{
    public GameObject player;

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject == player)
        {
            player.GetComponent<PlayerExpand>().Expand();
            Destroy(this.gameObject);
        }
    }
}
