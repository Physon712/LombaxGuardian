using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodeOnDeath : MonoBehaviour
{
	public float delay = 3f;
	public GameObject explosion;
	public Damageable subject;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(subject.isDead)
		{
			if(delay <= 0f)
			{
				Instantiate(explosion,transform.position,transform.rotation);
				Destroy(gameObject);
			}
			else
			{
				delay -= Time.deltaTime;
			}
		}
    }
}
