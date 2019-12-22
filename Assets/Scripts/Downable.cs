using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Downable : MonoBehaviour
{
	public Rigidbody rb;
	public NavMeshAgent navAgent;
	public Damageable subject;
	public AudioSource deathSound;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(subject.isDead)
		{
			rb.isKinematic = false;
			if(navAgent != null)navAgent.enabled = false;
			//if(deathSound != null)deathSound.Play();
		}
    }
	
}
