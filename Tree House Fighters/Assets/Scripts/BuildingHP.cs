using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingHP : MonoBehaviour 
{
//------------------------------------------------------------------------CONSTANTS:

	private const string LOG_TAG = "BuildingHP";
	public bool VERBOSE = false;

//---------------------------------------------------------------------------FIELDS:
	public float HP;
//---------------------------------------------------------------------MONO METHODS:

	void Start() 
	{

	}
		
	void Update()
    {
		if(HP <= 0)
		{
			Destroy(gameObject);
		}
    }

//--------------------------------------------------------------------------METHODS:

//--------------------------------------------------------------------------HELPERS:
	
}