using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
	public float speed = 50f;
	public float distanceBeforeShowingUp = 10f;
	public float destroyDelay = 5f;
	public float gravity = 0.10f;
	public float damage = 10f;
	public LayerMask ignoreLayer;
	
	public GameObject sparkEffect;
	
	public MeshRenderer mr;
	
	private RaycastHit hit;
	private float fallingVelocity;
    // Start is called before the first frame update
    void Start()
    {
        mr.enabled = false;
		Destroy(gameObject,destroyDelay);
    }

    // Update is called once per frame
    void Update()
    {
		if(Physics.Raycast(transform.position, transform.forward * speed * Time.deltaTime + Vector3.up*fallingVelocity*Time.deltaTime,out hit, speed * Time.deltaTime + Mathf.Abs(fallingVelocity) * Time.deltaTime , ~ignoreLayer))
		{
		Damageable target = hit.transform.GetComponent<Damageable> ();//Les truc qui tombent comme des briques;
		PlayerStat targetP = hit.transform.GetComponent<PlayerStat> ();
		
			if (target != null)
				{
				target.TakeDamage(damage);//The meat is damaged !
				}
			if (targetP != null)
				{
				targetP.TakeDamage(damage);//The meat is damaged !
				}
			
			Instantiate(sparkEffect, hit.point + hit.normal * 0.01f, Quaternion.LookRotation(hit.normal));
			Destroy(gameObject);
			if (hit.rigidbody != null) 
				{
				hit.rigidbody.AddForce (-hit.normal * 1250f);
				}
		}
		else
		{
			
		}
		fallingVelocity -= gravity*Time.deltaTime;
        transform.position += transform.forward * speed * Time.deltaTime + Vector3.up*fallingVelocity*Time.deltaTime;
		 if(distanceBeforeShowingUp <= 0f)
		 {
			 mr.enabled = true;
		 }
		 else
		 {
			distanceBeforeShowingUp -= speed * Time.deltaTime;
		 }
		
		
    }
}
