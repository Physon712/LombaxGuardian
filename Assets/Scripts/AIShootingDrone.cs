using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIShootingDrone : MonoBehaviour
{
	public UnityEngine.AI.NavMeshAgent navAgent;
	public Damageable subject;
	public Transform firePoint;
	public float range = 100f;
	public GameObject bullet;
	public float fireRate = 1f;
	public float rafaleRate = 0.1f;
	public int bulletPerRafale = 3;
	public float dispersion = 1f;
	public float strafRadius = 5f;
	public float nextMoveDelayMax = 5f;
	public float nextMoveDelayMin = 3f;
	public Transform target;
	public LayerMask layerMask;
	public LayerMask enemyLayer;
	
	private float nextMove = 0f;
    private RaycastHit hit;
	private NavMeshHit nhit;
	private Vector3 direction;
	private float nextShoot;
	private Vector3 randomDirection;
	private int bulletShot = 0;
    void Start()
    {
        if(target == null)target = GameObject.Find("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
		
        if(!subject.isDead)
		{
			if(nextMove <= Time.time)
			{
				MakeAMove();
				nextMove = Time.time + Random.Range(nextMoveDelayMin,nextMoveDelayMax);
			}
			direction = target.position-transform.position;
			if (Physics.Raycast (transform.position, direction.normalized,out hit , range, layerMask))
			{
				if(hit.transform.gameObject.layer == 9f)
				{
					transform.LookAt(target);
					if(nextShoot <= Time.time)
					{
						Fire();
						bulletShot++;
						if(bulletShot % bulletPerRafale == 0)
						{
							nextShoot = Time.time + fireRate;
						}
						else
						{
							nextShoot = Time.time + rafaleRate;
						}
					}
				}
			}
		}
    }
	
	private void MakeAMove()
	{
		
		randomDirection = Random.insideUnitSphere * strafRadius;
		randomDirection += transform.position;
		if(NavMesh.SamplePosition(randomDirection, out nhit, strafRadius, 1))
		{
		navAgent.SetDestination(nhit.position);
		//navAgent.isStopped = false;
		}
		
	}
	
	private void Fire() {
		if(bullet != null)Instantiate(bullet,firePoint.position,Quaternion.Euler(new Vector3(Random.Range(-dispersion,dispersion),Random.Range(-dispersion,dispersion),Random.Range(-dispersion,dispersion))+firePoint.eulerAngles),null);
	}
}
