using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damageable : MonoBehaviour
{
	public Rigidbody rb;
	public UnityEngine.AI.NavMeshAgent navAgent;
	public float health = 30f;
	public bool isDead = false;
	public AudioSource deathSound;
	public GameObject brokeFlash;
	
	public void TakeDamage(float damage)
	{
		health -= damage;
		if(health <= 0f && isDead == false)
		{
			isDead = true;
			if(rb != null)rb.isKinematic = false;
			if(navAgent != null)navAgent.enabled = false;
			if(deathSound != null)deathSound.Play();
			if(brokeFlash != null)Instantiate(brokeFlash,transform.position,transform.rotation);
		}
	}
}
