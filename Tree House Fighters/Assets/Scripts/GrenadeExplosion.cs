﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeExplosion : MonoBehaviour 
{
//------------------------------------------------------------------------CONSTANTS:

	private const string LOG_TAG = "ChangeMe";
	public bool VERBOSE = false;

//---------------------------------------------------------------------------FIELDS:
	public float Timer =0.25f;
//---------------------------------------------------------------------MONO METHODS:

	void Start() 
	{
		Invoke("BlowUp", Timer);
	}
		
	void OnTriggerEnter(Collider col)
	{
		if(col.tag == "Player")
		{
			PlayerMovement pm = col.GetComponent<PlayerMovement>();
			pm.HealthBar.value -= pm.ShootThreeDamage;
		}
		if(col.tag == "Building")
		{
			BuildingHP BHP = col.GetComponent<BuildingHP>();

			BHP.HP -= 20;
		}
	}

//--------------------------------------------------------------------------METHODS:

//--------------------------------------------------------------------------HELPERS:
	private void BlowUp()
	{
		Destroy(gameObject, Timer);
	}
}