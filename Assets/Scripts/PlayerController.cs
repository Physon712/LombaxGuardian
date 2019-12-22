using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{

	public CharacterController characterController;
	public float gravity;
	public float lateralSpeed = 0.8f;
	public float speed = 1f;
    public float jumpForce = 2f;
	public float airAffinity = 0.5f;
	public float airSpeedMultiplier = 4f;
	
	
	public float jetpackForce = 8f;
	public float jetpackBoostForce = 0.35f;
	public float jetpackFuelCost = 0.4f;
	public float jetpackFuel = 5f;
	public float jetpackMaxFuel = 5f;
	public float jetpackRefuelRate = 1f;
	public float jetpackRefuelDelay = 1f;
	private float jetpackNextRefuel;
	private bool jetOn = false;
	public AudioSource jetpackSound;
	public AudioSource windSound;
	public AudioSource landSound;
	public AudioSource boostSound;
	public AudioSource stepSound;
	public float stepFrequency = 0.8f;
	
	public Animator anim;
	public AudioSource jumpSound;
	public Camera camera;
	public GameObject holster;
	public Slider jetpackFuelHUD;


	private float fallingVelocity;
	private Vector3 horizontalVelocity;
	private Vector3 wantedHorizontalVelocity;
	private float lateralInput;
	private float frontInput;
	private Vector3 dirInput;
	private float stepDelay;
	private bool wasGrounded = true;
	
	private float hbcycle = 0f;
	private int hbStep = 1;
	private Vector3 wantedHandBob;
	
	private float Velocity;
	

	void Start(){
		Cursor.lockState = CursorLockMode.Locked;
	}

    // Update is called once per frame
    void Update() {
		Velocity = Mathf.Clamp(horizontalVelocity.magnitude,0f,200f);
		camera.transform.Rotate(0f,0f,-Input.GetAxis("Horizontal")*3f);
		camera.fieldOfView = 80+ 10*(Velocity/48f);
		
        //PLAYER INPUT
        lateralInput = Input.GetAxisRaw("Horizontal");
        frontInput = Input.GetAxisRaw("Vertical");

        dirInput = new Vector3(frontInput,0f,lateralInput);
		dirInput.Normalize();
		wantedHorizontalVelocity = (dirInput.x*transform.forward*speed) + (dirInput.z*transform.right*speed);
		Debug.Log(wantedHorizontalVelocity.magnitude);
        if (characterController.isGrounded)
        {
			HandBobbing();
            fallingVelocity = 0f;
			horizontalVelocity = Vector3.Lerp(horizontalVelocity,wantedHorizontalVelocity,Time.deltaTime*20f);
			
        }
		else
		{
			if(Mathf.Abs(fallingVelocity) > 0.1f)holster.transform.localPosition = new Vector3(0f,Mathf.Clamp(fallingVelocity/-200f,-0.075f,0.075f),0f);
			fallingVelocity -= gravity * Time.deltaTime;
			horizontalVelocity = Vector3.Lerp(horizontalVelocity,wantedHorizontalVelocity*airSpeedMultiplier,Time.deltaTime*airAffinity);
			
		}
		
		if (Physics.Raycast (transform.position, -transform.up, 1.15f))//IsGrounded ?
		{
			if(!wasGrounded)landSound.Play();
			wasGrounded = true;
			if(jetpackFuel < jetpackMaxFuel)//FUEL RELOADING
			{
				jetpackFuel += jetpackRefuelRate*Time.deltaTime;
			}
			else
			{
				jetpackFuel = jetpackMaxFuel;
			}
			
		jetOn = false;
		jetpackSound.Stop();
		
		if (Input.GetButtonDown("Jump"))//JUMP AND JETPACKING
            {
                Jump();
            }
			if(Mathf.Abs(lateralInput)+Mathf.Abs(frontInput) > 0f)//IsMoving
			{
				stepDelay -=Velocity*Time.deltaTime; 
				if(stepDelay <= 0f)
				{
					stepSound.Play();
					stepDelay = stepFrequency;
				}
				
			} 
		}
		else
		{
			JetPacking();
			wasGrounded = false;
		}

		if(Input.GetButtonDown("Fire2") && jetpackFuel >= jetpackFuelCost)
		{
			Boost();
		}
		
        characterController.Move(horizontalVelocity*Time.deltaTime + transform.up*fallingVelocity*Time.deltaTime); //MOVING THE PLAYER ACCORDINGLY
		
		Hud();
		
		windSound.volume = -0.2f+(Mathf.Abs(horizontalVelocity.x)+Mathf.Abs(horizontalVelocity.z)+Mathf.Abs(fallingVelocity))/50f;
		
    
	 }
		
	
	
	private void Jump()
	{
        fallingVelocity = jumpForce;
		jumpSound.Play();
	}
	
	private void JetPacking()
	{
        
		if(Input.GetButtonDown("Jump") && jetpackFuel > 0f)
		{
			jetOn = true;
			jetpackSound.Play();
		}
		if(Input.GetButtonUp("Jump"))
		{
			jetOn = false;
			jetpackSound.Stop();
		}
		if(jetOn && jetpackFuel > 0f)
		{
			fallingVelocity += jetpackForce * Time.deltaTime;
			jetpackFuel -= Time.deltaTime;
			jetpackNextRefuel = Time.time+jetpackRefuelDelay;
		}
		else
		{
			jetpackSound.Stop();
			jetOn = false;
		}
		
		if(jetpackNextRefuel <= Time.time)
		{
			if(jetpackFuel < jetpackMaxFuel)
			{
				jetpackFuel += jetpackRefuelRate*Time.deltaTime;
			}
			else
			{
				jetpackFuel = jetpackMaxFuel;
			}
		}
		
	}
	
	private void Boost()
	{
		if(fallingVelocity < 0f)fallingVelocity = 0f;
		fallingVelocity += 10f;             //StartBoost
		horizontalVelocity += wantedHorizontalVelocity*jetpackBoostForce;
		jetpackFuel -= jetpackFuelCost;
		jetpackNextRefuel = Time.time+jetpackRefuelDelay;
		boostSound.Play();
	}
	
	public void ApplyBoost(Vector3 boost)
	{
		horizontalVelocity += new Vector3(boost.x,0f,boost.z);
		fallingVelocity = boost.y;
		characterController.Move(horizontalVelocity*Time.deltaTime + transform.up*fallingVelocity*Time.deltaTime);
	}
	
	private void Hud()
	{
		jetpackFuelHUD.value = jetpackFuel/jetpackMaxFuel;
	}
	
	private void HandBobbing()
	{
		hbcycle += Velocity/12f*Time.deltaTime*hbStep;
		if(Mathf.Abs(hbcycle) >= 0.1f)
		{
			hbcycle = 0.1f*Mathf.Sign(hbcycle);
			hbStep=hbStep*-1;
		}
		wantedHandBob = new Vector3(hbcycle*0.3f,Mathf.Cos(hbcycle*20f)*0.01f,0f);
		holster.transform.localPosition = wantedHandBob;
		
	}
	
}
