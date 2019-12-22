using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
	public float fireRate = 0.1f;
	public float maxAmmo = 40f;
	public float reloadDelay = 2f;
	public float reloadRate = 1f;
	public float dispersion = 1f;
	public GameObject bullet;
	
	public Transform Mag;
	public Vector3 positionMag;
	public Vector3 rotationMag;
	public Vector3 positionEmptyMag;
	public Vector3 rotationEmptyMag;
	public AudioSource gunShoot;
	public ParticleSystem gunFlash;
	public Material overheatingMaterial;
	public Gradient overHeatingColor;
	
	public Transform firePoint;
	public Animator anim;
	
	private float ammo;
	private float nextShoot = 0f;
	private float nextReload = 0f;
    // Start is called before the first frame update
    void Start()
    {
        ammo = maxAmmo;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButton("Fire1"))
		{
			if(nextShoot <= Time.time )
			{
				if(ammo >= 1f)
				{
					Fire();
					nextShoot = Time.time+fireRate;
					nextReload = Time.time+reloadDelay;
					ammo -= 1f;
					if(ammo < 1f)anim.Play("LastFire");
				}
			}
		}
		if(nextReload <= Time.time)
		{
			if(ammo < maxAmmo)
			{
				ammo += Time.deltaTime*reloadRate;
			}
			else
			{
				ammo = maxAmmo;
			}
			
		}
		if(Mag != null)
		{
			Mag.localRotation = Quaternion.Euler(Vector3.Lerp(rotationEmptyMag,rotationMag,ammo/maxAmmo));
			Mag.localPosition = Vector3.Lerp(positionEmptyMag,positionMag,ammo/maxAmmo);
		}
		if(overheatingMaterial != null)overheatingMaterial.SetColor("_EmissionColor",overHeatingColor.Evaluate(ammo/maxAmmo));
    }
	
	private void Fire()
	{
		anim.Play("Fire",0,0.0f);
		if(bullet != null)Instantiate(bullet,firePoint.position,Quaternion.Euler(new Vector3(Random.Range(-dispersion,dispersion),Random.Range(-dispersion,dispersion),Random.Range(-dispersion,dispersion))+firePoint.eulerAngles),null);
		if(gunShoot != null)gunShoot.Play();
		gunFlash.Play();
	}
}
