using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour 
{
//------------------------------------------------------------------------CONSTANTS:

	private const string LOG_TAG = "PowerUp";
	public bool VERBOSE = false;

	public enum PowerType
	{
		HealthPack, Teleporter, FasterAttack
	}
//---------------------------------------------------------------------------FIELDS:
	public GameObject TeleportLocation;
	public PowerType PowerUpType;
	public float HealthPackHeal, PowerUpTimer, NewShootOneRate;
	private float savedShootingRateVal;
//---------------------------------------------------------------------MONO METHODS:
	void OnTriggerEnter(Collider col)
	{
		if(col.tag == "Player")
		{
			PlayerMovement pm = col.GetComponent<PlayerMovement>();
			if(PowerUpType == PowerType.HealthPack)
			{
				pm.HealthBar.value += HealthPackHeal;
			}
			else if(PowerUpType == PowerType.Teleporter)
			{
				var camVal = col.transform.Find("Main Camera").GetComponent<CameraMovement>();
				pm.gameObject.transform.position = TeleportLocation.transform.position;
			}
			else if(PowerUpType == PowerType.FasterAttack)
			{
				savedShootingRateVal = pm.ShootOneFireRate;
				pm.ShootOneFireRate = NewShootOneRate;

				StartCoroutine(Reset(pm));
			}
		}
	}

//--------------------------------------------------------------------------METHODS:

	void Update()
	{
		transform.Rotate (new Vector3 (15, 30, 45) * Time.deltaTime);
	}
//--------------------------------------------------------------------------HELPERS:
	IEnumerator Reset(PlayerMovement pm)
	{
		yield return new WaitForSeconds(PowerUpTimer);
		pm.ShootOneFireRate = savedShootingRateVal;
		Destroy(gameObject);
	}
}