using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStat : MonoBehaviour
{
	public float health = 100f;
	public float maxHealth = 100f;
	public bool[] weaponPos;
	public GameObject[] weapon;
	public int currentWeapon = 0;
	public Slider healthHUD;
	
	private float weapChoice;
	bool ok = true;
	
	public void TakeDamage(float damage)
	{
		health -= damage;
		
		if(health <= 0f)
		{
			Debug.Log("You are dead...");
		}
		else
		{
			Debug.Log(health);
		}
	}
	
	public void Update()
	{
		Hud();
		
		weapChoice = Input.GetAxis("Mouse ScrollWheel");
		if(weapChoice < 0)
		{
			ok = false;
			while(!ok)
			{
				weapon[currentWeapon].SetActive(false);
				if(currentWeapon != weapon.Length-1)
				{
					currentWeapon++;
					
				}
			else
				{
					currentWeapon = 0;
				}
			if(weaponPos[currentWeapon])
			{
				ok = true;
				//Debug.Log(ammo[weapon[currentWeapon].GetComponent<Gun>().ammoType] + weapon[currentWeapon].GetComponent<Gun>().ammoLeft);
			}
				
			}
			weapon[currentWeapon].SetActive(true);
			//anim.Play("Equip");
		}

		if(weapChoice > 0)
		{
			ok = false;
			while(!ok)
			{
				weapon[currentWeapon].SetActive(false);
				if(currentWeapon != 0)
				{
					currentWeapon--;
					
				}
			else
				{
					currentWeapon = weapon.Length-1;
				}
			if(weaponPos[currentWeapon])
			{
				ok = true;
			}
				
			}
			weapon[currentWeapon].SetActive(true);
		}
	}
	
	private void Hud()
	{
		healthHUD.value = health/maxHealth;
	}
}
