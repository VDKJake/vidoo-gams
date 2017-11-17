using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControls : MonoBehaviour
{
    private GameObject player;
    private Vector2 offset;
    private float smoothingTime = 0.3f;
    private Vector2 vel = Vector2.zero;

	private void Start ()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        offset = transform.position - player.transform.position;
	}
	
	private void Update ()
    {
        Vector2 target = new Vector2(player.transform.position.x , player.transform.position.y) + offset;
        transform.position = Vector2.SmoothDamp(transform.position, target, ref vel, smoothingTime, 10f, Time.deltaTime);
        transform.position = new Vector3(transform.position.x, transform.position.y, -10f);
	}
}
