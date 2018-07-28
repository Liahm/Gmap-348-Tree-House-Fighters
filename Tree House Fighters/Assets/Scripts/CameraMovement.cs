using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
//------------------------------------------------------------------------CONSTANTS:

	private const string LOG_TAG = "CameraMovement";
	public bool VERBOSE = false;

	public Axes.Action XCamAxis, YCamAxis;
	const float p = Mathf.PI/180;
//---------------------------------------------------------------------------FIELDS:
	public PlayerMovement PM;
	public GameObject CameraFocus;
	public float
		CamDistance=5,
		CamSensitivity=200,
		CamMaxAngle=80,
		CamMinAngle=-70,
		CamDispFactor=1.2f,
		CamVertAngle=20,
		CamHorAngle=0;

	

//---------------------------------------------------------------------MONO METHODS:
	void LateUpdate()
	{
		CamHorAngle += CamSensitivity * Time.deltaTime * Input.GetAxis(Axes.toStr[XCamAxis]);

		CamVertAngle += CamSensitivity * Time.deltaTime * Input.GetAxis(Axes.toStr[YCamAxis]);
		CamVertAngle = Mathf.Clamp(CamVertAngle, CamMinAngle, CamMaxAngle);

		Vector3 pos = CamDistance * Vector3.forward;

		pos = Quaternion.AngleAxis(-CamVertAngle, Vector3.right) * pos;
		pos = Quaternion.AngleAxis(CamHorAngle, Vector3.up) * pos;

		//make sure camera stays in front of walls
		float dist = CamDistance;
		//RaycastHit[] hits = Physics.RaycastAll(cameraFocus.transform.position, pos, camDistance);
		RaycastHit[] hits = Physics.SphereCastAll(CameraFocus.transform.position, 0.5f , pos, CamDistance, -1);
		foreach (RaycastHit hit in hits)
		{
			if (hit.transform.tag == "Wall")
			{
				dist = Mathf.Min(dist, hit.distance);
			}
		}
				
		
		pos = pos.normalized * dist;
		float distanceRatio = (dist / CamDistance);
		Vector3 focusDisplacement = Vector3.up * distanceRatio;
		CameraFocus.transform.position = PM.transform.position + (Vector3.up * CamDispFactor) + focusDisplacement;

		transform.position = CameraFocus.transform.position+pos;
		

		transform.LookAt(CameraFocus.transform);

	}

//--------------------------------------------------------------------------METHODS:

//--------------------------------------------------------------------------HELPERS:
	
}