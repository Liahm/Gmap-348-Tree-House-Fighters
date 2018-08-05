using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour 
{
//------------------------------------------------------------------------CONSTANTS:

	private const string LOG_TAG = "Spawner";
	public bool VERBOSE = false;

//---------------------------------------------------------------------------FIELDS:
	public GameObject SpawnObject;
	public float RespawnTime;

	[System.NonSerialized]
	public bool ObjectActive;
	[System.NonSerialized]
	public float respawntimer;
//---------------------------------------------------------------------MONO METHODS:

	void Start() 
	{
		respawntimer = RespawnTime;
	}
		
	void Update()
    {
		if(Time.time >= respawntimer && !ObjectActive)
		{
			var spawnedObject = Instantiate(SpawnObject, transform.position, SpawnObject.transform.rotation);
			spawnedObject.GetComponent<PowerUp>().spawn = gameObject.GetComponent<Spawner>();
			ObjectActive = true;
		}
    }

//--------------------------------------------------------------------------METHODS:

//--------------------------------------------------------------------------HELPERS:
	
}