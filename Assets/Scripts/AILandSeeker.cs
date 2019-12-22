using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AILandSeeker : MonoBehaviour
{
 public UnityEngine.AI.NavMeshAgent navAgent;
	public Damageable subject;
	public float range = 100f;
	public float strafRadius = 5f;
	public float nextMoveDelayMax = 5f;
	public float nextMoveDelayMin = 3f;
	public float attackRate = 0.5f;
	public float attackRadius = 0.3f;
	public float damage;
	public Transform target;
	public LayerMask layerMask;
	public LayerMask enemyLayer;
	
	private float nextMove = 0f;
    private RaycastHit hit;
	private UnityEngine.AI.NavMeshHit nhit;
	private Vector3 direction;
	private Vector3 randomDirection;
	private float attackDelay;
    void Start()
    {
        if(target == null)target = GameObject.Find("Player").transform;
		attackDelay = attackRate;
    }

    // Update is called once per frame
    void Update()
    {
		
        if(!subject.isDead)
		{
			
			direction = target.position-transform.position;
			if (Physics.Raycast (transform.position, direction.normalized,out hit , range, layerMask))
			{
				if(hit.transform.gameObject.layer == 9f)//SeekAndDestroy
				{
					if(Vector3.Distance(target.position,transform.position) <= attackRadius)
					{
						//DESTROY
						navAgent.isStopped = true;
						attackDelay-= Time.deltaTime;
						if(attackDelay <= 0f)
						{
							target.gameObject.GetComponent<PlayerStat>().TakeDamage(damage);
							attackDelay = attackRate;
						}
					}
					else
					{
						//SEEK
						navAgent.SetDestination(target.position);
					    navAgent.isStopped = false;
					}
					
				}
				else
				{
				if(nextMove <= Time.time)//Patroll instead
					{
					MakeAMove();
					nextMove = Time.time + Random.Range(nextMoveDelayMin,nextMoveDelayMax);
					}
				}
			}
		}
		
    }
	
	private void MakeAMove()
	{
		
		randomDirection = Random.insideUnitSphere * strafRadius;
		randomDirection += transform.position;
		if(UnityEngine.AI.NavMesh.SamplePosition(randomDirection, out nhit, strafRadius, 1))
		{
		navAgent.SetDestination(nhit.position);
		}
		
	}
	
}