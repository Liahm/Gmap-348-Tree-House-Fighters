﻿using System.Collections;
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
	public float HealthPackHeal, PowerUpTimer, NewShootOneRate, HealthPackRespawm;
	private float savedShootingRateVal, savedHealthPackRespawm;
	private bool respawning;
	private BoxCollider box;
	private MeshRenderer mesh;
	private GameObject healthChild;

	[System.NonSerialized]
	public Spawner spawn; 
//---------------------------------------------------------------------MONO METHODS:
	void OnTriggerEnter(Collider col)
	{
		if(col.tag == "Player")
		{
			PlayerMovement pm = col.GetComponent<PlayerMovement>();
			if(PowerUpType == PowerType.HealthPack)
			{
				pm.HealthBar.value += HealthPackHeal;
				respawning = true;
				savedHealthPackRespawm = Time.time + HealthPackHeal;
			}
			else if(PowerUpType == PowerType.Teleporter)
			{
				var camVal = col.transform.Find("Main Camera").GetComponent<CameraMovement>();
				pm.gameObject.transform.position = TeleportLocation.transform.position;
			}
			else if(PowerUpType == PowerType.FasterAttack)
			{
				if(spawn != null)
				{
					spawn.ObjectActive = false;
					spawn.respawntimer = Time.time + spawn.RespawnTime;
				}
				if(pm.ShootOneFireRate != savedShootingRateVal)
					savedShootingRateVal = pm.ShootOneFireRate;
				pm.ShootOneFireRate = NewShootOneRate;

				StartCoroutine(Reset(pm));
			}
		}
	}
	void Start()
	{
		box = gameObject.GetComponent<BoxCollider>();
		mesh = gameObject.GetComponent<MeshRenderer>();
		if(PowerUpType == PowerType.HealthPack)	
			healthChild = transform.Find("Health Group").gameObject;
	}
	void Update()
	{
		if(PowerUpType != PowerType.Teleporter)
			transform.Rotate (new Vector3 (0, 30, 0) * Time.deltaTime);

		if(respawning && PowerUpType != PowerType.HealthPack)
		{
			box.enabled = false;
			mesh.enabled = false;
			if(Time.time >= savedHealthPackRespawm)
			{
				box.enabled = true;
				mesh.enabled = true;
				respawning = false;
			}
		}
		else if (respawning && PowerUpType == PowerType.HealthPack)
		{
			box.enabled = false;
			healthChild.SetActive(false);
			if(Time.time >= savedHealthPackRespawm)
			{
				box.enabled = true;
				healthChild.SetActive(true);
				respawning = false;
			}
		}
	}
//--------------------------------------------------------------------------METHODS:

	
//--------------------------------------------------------------------------HELPERS:
	IEnumerator Reset(PlayerMovement pm)
	{
		gameObject.GetComponent<MeshRenderer>().enabled = false;
		gameObject.GetComponent<BoxCollider>().enabled = false;
		yield return new WaitForSeconds(PowerUpTimer);
		pm.ShootOneFireRate = savedShootingRateVal;
		Destroy(gameObject);
	}
}