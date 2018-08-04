using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeAttribute : MonoBehaviour 
{
//------------------------------------------------------------------------CONSTANTS:

	private const string LOG_TAG = "GrenadeAttribute";
	public bool VERBOSE = false;

//---------------------------------------------------------------------------FIELDS:
	public float ActivationTime = 2;
	public AudioClip GrenadeShootSound;
	public AudioClip GrenadeHitSound;
	public AudioSource Source;
	public GameObject GrenadeExplosion;
	
	public bool activated;
//---------------------------------------------------------------------MONO METHODS:

	void Start() 
	{
		if(GrenadeShootSound != null)
		{
			Source.clip = GrenadeShootSound;
			Source.Play();	
		}
	}
		
	void OnTriggerEnter(Collider col)
	{
		if(activated)
		{
			if(GrenadeHitSound != null)
			{
				Source.clip = GrenadeHitSound;
				Source.Play();	
			}
			if(GrenadeExplosion != null)
			{
				var explosion = Instantiate(GrenadeExplosion, transform);
				explosion.transform.parent = null;
			}
			Destroy(gameObject);
		}
		else
		{
			LateExplosion();
		}
	}
	void Update()
    {
		if(Time.time > ActivationTime)
			activated = true;
		else if (Time.time > 15f)
			Destroy(gameObject);
    }

//--------------------------------------------------------------------------METHODS:

	IEnumerator LateExplosion()
	{
		yield return new WaitForSeconds(ActivationTime);
		Instantiate(GrenadeExplosion, transform);
		Destroy(gameObject);
	}
//--------------------------------------------------------------------------HELPERS:
	
}