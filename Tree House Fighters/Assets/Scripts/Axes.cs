using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Axes : MonoBehaviour 
{
	public enum Action {
		MoveX, MoveY, Dodge,
		CamX, CamY,
		ShootOne, ShootTwo, ShootThree,

		MoveXController, MoveYController, DodgeController,
		CamXController, CamYController,
		ShootOneController, ShootTwoController, ShootThreeController,

		MoveX2, MoveY2, Dodge2,
		CamX2, CamY2,
		ShootOne2, ShootTwo2, ShootThree2,
	}

	public static Dictionary<Action, string> toStr = new Dictionary<Action, string>{
		{Action.MoveX,"Horizontal"},
		{Action.MoveY,"Vertical"},
		{Action.CamX,"Cam Horizontal"},
		{Action.CamY,"Cam Vertical"},
		{Action.ShootOne, "ShootOne"},
		{Action.ShootTwo, "ShootTwo"},
		{Action.ShootThree, "ShootThree"},

		{Action.MoveXController,"Horizontal Controller"},
		{Action.MoveYController,"Vertical Controller"},
		{Action.CamXController,"Cam Horizontal Controller"},
		{Action.CamYController,"Cam Vertical Controller"},
		{Action.ShootOneController, "ShootOne Controller"},
		{Action.ShootTwoController, "ShootTwo Controller"},
		{Action.ShootThreeController, "ShootThree Controller"},

		{Action.MoveX2,"Horizontal Controller2"},
		{Action.MoveY2,"Vertical Controller2"},
		{Action.CamX2,"Cam Horizontal Controller2"},
		{Action.CamY2,"Cam Vertical Controller2"},
		{Action.ShootOne2, "ShootOne Controller2"},
		{Action.ShootTwo2, "ShootTwo Controller2"},
		{Action.ShootThree2, "ShootThree Controller2"},
	};

	public static float GetAxis(Action a) {
		return Input.GetAxis(toStr[a]);
	}
}
