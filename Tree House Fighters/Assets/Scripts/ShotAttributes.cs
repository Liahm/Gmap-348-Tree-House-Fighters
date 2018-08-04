using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotAttributes : MonoBehaviour 
{
//------------------------------------------------------------------------CONSTANTS:

	private const string LOG_TAG = "ShotAttributes";
	public bool VERBOSE = false;

//---------------------------------------------------------------------------FIELDS:
	public AudioClip FlakShootSound, CannonShootSound;
	public AudioClip FlakHitSound, CannonHitSound;
	public AudioSource Source;

//---------------------------------------------------------------------MONO METHODS:

	void OnTriggerEnter(Collider col)
	{
		
		if(col.tag == "Player")
		{
			PlayerMovement pm = col.GetComponent<PlayerMovement>();
			Debug.Log(col.tag + " - " + gameObject.name);
			if(gameObject.tag == "Bullet")
			{
				if(FlakHitSound != null)
				{
					Source.clip = FlakHitSound;
					Source.Play();		
				}
				pm.HealthBar.value -= pm.ShootOneDamage;
			}
			else if(gameObject.tag == "Cannon")
			{
				if(CannonHitSound != null)
				{
					Source.clip = CannonHitSound;
					Source.Play();	
				}
				pm.HealthBar.value -= pm.ShootTwoDamage;
			}
		}
		else if(col.tag == "Left Gun")
		{
			PlayerMovement pm = col.transform.parent.GetComponent<PlayerMovement>();
			if(gameObject.tag == "Bullet")
			{
				pm.LeftGunHP -= pm.ShootOneDamage;
			}
			else if(gameObject.tag == "Cannon")
			{
				pm.LeftGunHP -= pm.ShootTwoDamage;
			}
		}
		else if(col.tag == "Right Gun")
		{
			PlayerMovement pm = col.transform.parent.GetComponent<PlayerMovement>();
			if(gameObject.tag == "Bullet")
			{
				pm.RightGunHP -= pm.ShootOneDamage;
			}
			else if(gameObject.tag == "Cannon")
			{
				pm.RightGunHP -= pm.ShootTwoDamage;
			}
		}

		if(col.tag == "Building")
		{
			BuildingHP BHP = col.GetComponent<BuildingHP>();

			if(gameObject.tag == "Bullet")
			{
				BHP.HP -= 1;
			}
			else if(gameObject.tag == "Cannon")
			{
				BHP.HP -= 8;
			}
		}
		//Spawn dust animation
		//Move back to queue
		Destroy(gameObject);
	}

	void Start()
	{
		if(gameObject.name == "Bullet" && FlakShootSound != null)
		{
			Source.clip = FlakShootSound;
			Source.Play();		
		}
		else if(gameObject.name == "Cannon" && CannonShootSound != null)
		{
			Source.clip = CannonShootSound;
			Source.Play();	
		}
	}

//--------------------------------------------------------------------------METHODS:

//--------------------------------------------------------------------------HELPERS:
	
}