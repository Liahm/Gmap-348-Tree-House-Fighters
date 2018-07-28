using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotAttributes : MonoBehaviour 
{
//------------------------------------------------------------------------CONSTANTS:

	private const string LOG_TAG = "ShotAttributes";
	public bool VERBOSE = false;

//---------------------------------------------------------------------------FIELDS:
	public AudioClip FlakShootSound, CannonShootSound, GrenadeShootSound;
	public AudioClip FlakHitSound, CannonHitSound, GrenadeHitSound;
	public AudioSource Source;

	[Space(15)]
	public GameObject GrenadeExplosion;
//---------------------------------------------------------------------MONO METHODS:

	void OnTriggerEnter(Collider col)
	{
		
		if(gameObject.name == "Grenade")
		{
			if(GrenadeExplosion != null)
				Instantiate(GrenadeExplosion, transform);
		}
		else if(col.tag == "Player")
		{
			PlayerMovement pm = col.GetComponent<PlayerMovement>();
			Debug.Log(col.tag + " - " + gameObject.name);
			if(gameObject.tag == "Bullet")
				pm.HealthBar.value -= pm.ShootOneDamage;
			else if(gameObject.tag == "Cannon")
				pm.HealthBar.value -= pm.ShootTwoDamage;
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
		else if(gameObject.name == "Grenade" && GrenadeShootSound != null)
		{
			Source.clip = GrenadeShootSound;
			Source.Play();	
		}
	}

//--------------------------------------------------------------------------METHODS:

//--------------------------------------------------------------------------HELPERS:
	
}