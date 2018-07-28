using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeExplosion : MonoBehaviour 
{
//------------------------------------------------------------------------CONSTANTS:

	private const string LOG_TAG = "ChangeMe";
	public bool VERBOSE = false;

//---------------------------------------------------------------------------FIELDS:
	public float Timer;
//---------------------------------------------------------------------MONO METHODS:

	void Start() 
	{
		Invoke("BlowUp", Timer);
	}
		
	void OnTriggerEnter(Collider col)
	{
		if(col.tag == "Player")
		{
			//deal damage
		}
		
	}

//--------------------------------------------------------------------------METHODS:

//--------------------------------------------------------------------------HELPERS:
	private void BlowUp()
	{
		Destroy(gameObject, Timer);
	}
}