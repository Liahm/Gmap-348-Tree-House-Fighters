using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerMovement : MonoBehaviour 
{
//------------------------------------------------------------------------CONSTANTS:

	private const string LOG_TAG = "PlayerMovement";
	public bool VERBOSE = false;
	
	public Axes.Action Horizontal, Vertical, ShootOne, ShootTwo, ShootThree, DodgeButton, ReloadButton;

//---------------------------------------------------------------------------FIELDS:
	
	public CharacterController CC;
	[Space(10)]
	public Camera Cam;
	public Slider HealthBar;
	public Text BulletCount, LeftArmHPRegening, RightArmHPRegening;
	public bool CanMove=true, Shooting = false, CanShoot=true;
	public float  LeftGunHP, RightGunHP, LeftGunRegenTimer, RightGunRegenTimer, DashSpeed, DashCD;
	[System.NonSerialized]
	public float shotsFired;

	public float
		Speed				=	1,
		ShootingPlayerSpeed	=	0.75f,
		ResetAfterShootTime	=	1,
		Gravity				=	9.81f,
		ReloadTimeOne		=	2,
		ShootOneFireRate	=	0.25f,
		ShootingSpread		=	0.01f,
		ShootOneDamage		=	1,
		ShootOneSpeed		=	10,
		MagazineSizeOne		=	100,
		ReloadTimeTwo		=	5,
		ShootTwoDamage		=	5,
		ShootTwoSpeed		=	100,
		ReloadTimeThree		=	5,
		ShootThreeDamage	=	5,
		ShootThreeSpeed		=	50;

	public Vector3
		playerMove			=	Vector3.zero,
		netForce			=	Vector3.zero,//velocity added for one frame only
		constVelocity		=	Vector3.zero,//constant velocity added every frame
		Center				=	Vector3.zero;
	
	public GameObject CannonSpawnPos,GrenadeSpawnPos;
	public GameObject[] FlackSpawnPos1,FlackSpawnPos2;
	[Space(10)]
	public GameObject Bullets;
	public GameObject Cannon, Grenades;
	public GameObject LeftGun, RightGun;
	private float resetTimer, timeStamp,timeStamp2,timeStamp3, leftGunInitialHP, rightGunInitialHP,
					leftGunTimer, rightGunTimer, dashCooldown, activationTime;
	private bool cannonChange = true, reloading = false, healingLeft=false, healingRight=false,
					dashing = false;
//---------------------------------------------------------------------MONO METHODS:

	void Start()
	{
		shotsFired = MagazineSizeOne;
		leftGunInitialHP = LeftGunHP;
		rightGunInitialHP = RightGunHP;
		activationTime = 0.25f;
	}
	void Update()
    {
		if(CanMove)
		{
			MovePlayer();	
		}
		if(shotsFired != 0 && !reloading)
		{
			BulletCount.text = shotsFired.ToString();
		}
		else if(reloading && Time.time >= timeStamp - 0.1f)
		{
			reloading = false;
		}

		if((Axes.toStr[ShootOne] == "ShootOne Controller"  && CanShoot
			|| Axes.toStr[ShootOne] == "ShootOne Controller2") && CanShoot)
		{
			if(Mathf.Round(Input.GetAxisRaw(Axes.toStr[ShootOne])) < 0)
			{
				Debug.Log("Shooting One");
				ShootFlaks();
			}
			else if(Time.time >= resetTimer && Shooting)
				Shooting = false;
		}
		//Allow all 3 to fire at the same time?
		if(Input.GetButton(Axes.toStr[ShootOne]) && CanShoot)
		{
			Debug.Log("Shooting One");
			ShootFlaks();
		}
		else if(Time.time >= resetTimer && Shooting)
			Shooting = false;

		if(Input.GetButtonDown(Axes.toStr[ShootTwo]) && CanShoot)
		{
			Debug.Log("Shooting Two");

			ShootMiddle();
		}
		else if(Time.time >= resetTimer && Shooting)
			Shooting = false;

		if(Input.GetButtonDown(Axes.toStr[ShootThree]) && CanShoot)
		{
			Debug.Log("Shooting Three");
			ShootGrenades();
		}
		else if(Time.time >= resetTimer && Shooting)
			Shooting = false;

		if(Input.GetButtonDown(Axes.toStr[ReloadButton]) && shotsFired != MagazineSizeOne)
		{
			timeStamp = Time.time + ReloadTimeOne;
			BulletCount.text = "Reloading";
			reloading = true;
			shotsFired = MagazineSizeOne;
		}
		CalculateArms();

    }

//--------------------------------------------------------------------------METHODS:
	public void AddForce(Vector3 f) 
	{
		netForce+=f;
	}
	public void AddVelocity(Vector3 v) 
	{
		constVelocity+=v;
	}
	
	public Vector3 velocity 
	{
		get 
		{
			if(CC) 
			{
				return CC.velocity;
			}
			else 
			{
				return playerMove+constVelocity+netForce*Time.deltaTime;
			}
		}
	}
	
//--------------------------------------------------------------------------HELPERS:
	private void CalculateArms()
	{
		if(LeftGunHP <=  0 && healingLeft)
		{
			LeftArmHPRegening.text = "Left Arm Repairing - " + (leftGunTimer- Time.time).ToString("F2");
			if(Time.time >= leftGunTimer)
			{
				LeftArmHPRegening.text = "Left Arm Fully Functional";
				LeftGunHP = leftGunInitialHP;

			}
		}
		if(RightGunHP <= 0 && healingRight)
		{
			RightArmHPRegening.text = "Right Arm Repairing - " +  (rightGunTimer- Time.time).ToString("F2");
			if(Time.time >= rightGunTimer)
			{
				RightArmHPRegening.text = "Right Arm Fully Functional";
				RightGunHP = rightGunInitialHP;
			}
		}

		if(RightGunHP <= 0 && !healingRight)
		{
			RightGun.SetActive(false);
			rightGunTimer = Time.time + RightGunRegenTimer;
			cannonChange = false;
			healingRight = true;
		}
		else if(RightGunHP >= 0)
		{
			healingRight = false;
			RightGun.SetActive(true);
		}	

		if(LeftGunHP <= 0 && !healingLeft)
		{
			LeftGun.SetActive(false);
			leftGunTimer = Time.time +LeftGunRegenTimer;
			cannonChange = true;
			healingLeft = true;
		}
		else if(LeftGunHP >= 0)
		{
			healingLeft = false;
			LeftGun.SetActive(true);
		}	
	}
	private void Die()
	{
		//Add a who win statement too
		netForce = Vector3.zero;
		constVelocity = Vector3.zero;
		playerMove = Vector3.zero;
	}


	private void MovePlayer()
	{
		Vector3 camForward = Vector3.Cross(Vector3.up, Cam.transform.right).normalized;
		Vector3 inputVector = new Vector3(Axes.GetAxis(Vertical), 0, Axes.GetAxis(Horizontal));
		Vector3 playerDirection = inputVector.x * camForward + inputVector.z * Cam.transform.right;
		playerDirection.y = 0f; //No way you are jumping, ever.

		if(Shooting)
		{
			playerMove = ShootingPlayerSpeed * playerDirection;
		}
		else
		{
			if(inputVector.magnitude > 1 && !Shooting)
			{
				float atan2 = Mathf.Atan2(playerDirection.x, playerDirection.z) * Mathf.Rad2Deg;
				transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0,(atan2), 0), 10 * Time.deltaTime);
			}

			playerMove = Speed * playerDirection;
		}

		constVelocity+=netForce * Time.deltaTime;

		playerMove+=constVelocity;
		if(Input.GetButton(Axes.toStr[DodgeButton]) && !dashing)
		{	
			CC.Move(playerMove*Time.deltaTime*DashSpeed);
			dashCooldown = Time.time + DashCD;
			if(Time.time >= activationTime)
			{
				dashing = true;
			}
		}
		if(dashing && Time.time >= dashCooldown)
		{
			dashing = false;
			activationTime = Time.time + DashCD;
		}
		
		CC.Move(playerMove*Time.deltaTime);
		constVelocity=Vector3.Lerp(constVelocity,Vector3.zero,.5f*Time.deltaTime);
		netForce=Gravity*Vector3.down;
	}

	private void ShootFlaks()
	{
		
		GameObject flak;
		var randomNumberX = Random.Range(-ShootingSpread, ShootingSpread);
     	var randomNumberY = Random.Range(-ShootingSpread, ShootingSpread);
     	var randomNumberZ = Random.Range(-ShootingSpread, ShootingSpread); 
		Quaternion camY = Quaternion.Euler(0, Cam.transform.rotation.eulerAngles.y,0);
		if(Time.time >= timeStamp)
		{
			Shooting = true;
			if(cannonChange && RightGunHP > 0)
			{
				foreach(GameObject flacks in FlackSpawnPos1)
				{
					flak = Instantiate(Bullets, flacks.transform);
					flak.transform.SetParent(null);
					flak.transform.Rotate(randomNumberX, randomNumberY, randomNumberZ);
					flak.GetComponent<Rigidbody>().velocity = (Cam.transform.forward * ShootOneSpeed);
				}
				transform.rotation = Quaternion.Lerp(transform.rotation, camY, Time.time * 0.1f);
				shotsFired--;
				if(LeftGunHP > 0)
					cannonChange = false;
			}
			else if (!cannonChange && LeftGunHP > 0)
			{
				foreach(GameObject flacks in FlackSpawnPos2)
				{
					flak = Instantiate(Bullets, flacks.transform);
					flak.transform.SetParent(null);
					flak.transform.Rotate(randomNumberX, randomNumberY, randomNumberZ);
					flak.GetComponent<Rigidbody>().velocity = (Cam.transform.forward * ShootOneSpeed);
				}
				transform.rotation = Quaternion.Lerp(transform.rotation, camY, Time.time * 0.1f);
				shotsFired--;
				if(RightGunHP > 0)
					cannonChange = true;
			}
			
			if(shotsFired == 0)
			{
				//Reload Sound applied here
				timeStamp = Time.time + ReloadTimeOne;
				BulletCount.text = "Reloading";
				reloading = true;
				shotsFired = MagazineSizeOne;
			}
			else
			{	
				timeStamp = Time.time + ShootOneFireRate;
			}
		}
		
		resetTimer = Time.time + ResetAfterShootTime;
	}

	private void ShootMiddle()
	{
		if(Time.time >= timeStamp2)
		{
			GameObject cannonBall = Instantiate(Cannon, CannonSpawnPos.transform);
			cannonBall.transform.SetParent(null);
			transform.rotation = Cam.transform.rotation;
			Shooting = true;
			cannonBall.GetComponent<Rigidbody>().velocity = (transform.forward * ShootTwoSpeed);
			timeStamp2 = Time.time + ReloadTimeTwo;
		}
		resetTimer = Time.time + ResetAfterShootTime;
	}

	private void ShootGrenades()
	{
		
		Quaternion camY = Quaternion.Euler(0, Cam.transform.rotation.eulerAngles.y,0);

		if(Time.time >= timeStamp3)
		{
			Shooting = true;
			GameObject Grenade = Instantiate(Grenades, GrenadeSpawnPos.transform);
			Grenade.transform.SetParent(null);
			transform.rotation = Quaternion.Lerp(transform.rotation, camY, Time.time * 0.1f);
			Grenade.GetComponent<Rigidbody>().velocity = (GrenadeSpawnPos.transform.forward * ShootThreeSpeed);
			timeStamp3 = Time.time + ReloadTimeThree;
			
		}
		
		resetTimer = Time.time + ResetAfterShootTime;
	}
}
