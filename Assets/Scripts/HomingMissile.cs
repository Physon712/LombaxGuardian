using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingMissile : MonoBehaviour
{
	public Rigidbody rb;
	public Transform target;
	
	public float speed = 75f;
	public float turningSpeed = 120f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rb.velocity = transform.up*speed*Time.fixedDeltaTime;
    }
}
