using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Booster : MonoBehaviour
{
	public Vector3 boost;
	public AudioSource sound;
	
	private Vector3 realBoost;
	
	void Start()
	{
		realBoost = boost.y*transform.forward+boost.x*transform.up+boost.z*-transform.right;
	}
    void OnTriggerEnter(Collider other){
	   if(other.transform.name == "Player")
	   {
		   other.GetComponent<PlayerController>().ApplyBoost(realBoost);
		   sound.Play();
	   }
	}
}
