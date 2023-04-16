using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
	//startup
	//public Animator animator;
	public CharacterController characterController;
	public Camera playerCamera;
	public GameObject Weapon;
	Vector3 velocity;
	Rigidbody rb;
	PlayerInput playerInput;
	
	//movement
	public float speed;
	public float sprintBoost;
	private bool isCrouched = false;
	public float playerFOV;
	public float ADSFOV;

	//jumping
	public float groundDistance = 0.4f;
	public float jumpForce;
	public float gravity;
	public Transform groundCheck;
	public LayerMask groundMask;
	bool isGrounded;

	void Start()
	{
		playerInput = GetComponent<PlayerInput>();
		rb = GetComponent<Rigidbody>();
		playerCamera.fieldOfView = playerFOV;

		playerInput.SwitchCurrentControlScheme("KeyboardAndMouse", Keyboard.current, Mouse.current);
		Debug.Log(playerInput.currentControlScheme);
	}

	void Update()
    {
		isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

		if (isGrounded && velocity.y <0)
		{
			velocity.y = -2f;
		}

		float x = Input.GetAxis("Horizontal");
		float z = Input.GetAxis("Vertical");

		Vector3 move = transform.right * x + transform.forward * z;

		characterController.Move(move * speed * Time.deltaTime);

		if (Input.GetButtonDown("Jump") && isGrounded)
		{
			velocity.y = Mathf.Sqrt(jumpForce * -2 * gravity);
		}

		velocity.y += gravity * Time.deltaTime;

		characterController.Move(velocity * Time.deltaTime);

	}

	public void Crouch(InputAction.CallbackContext context)
	{
		if (context.performed)
		{
			if (isCrouched == false)
			{
				playerCamera.transform.position += Vector3.down;
				isCrouched = true;
			}
			else
			{
				playerCamera.transform.position += Vector3.up;
				isCrouched = false;
			}
		}
		
	}

	public void Fire(InputAction.CallbackContext context)
	{
		/*bool isPressed = false;
		if (context.performed)
		{
			isPressed = true;
		}
		while (isPressed == true)
		{
			Weapon.GetComponent<Gun>().Fire();
			isPressed = false;
			*//*if (context.canceled)
			{
				isPressed = false;
			}*//*
		}*/


		if (context.performed)
		{
			Weapon.GetComponent<Gun>().Fire();
		}
	}

	public void Sprint(InputAction.CallbackContext context)
	{
		//tap
		if (context.performed)
		{
			speed *= sprintBoost;
			Debug.Log(speed);
		}
		if (context.canceled)
		{
			speed /= sprintBoost;
			Debug.Log(speed);
		}
	}

	public void Aim(InputAction.CallbackContext context)
	{
		if (context.performed)
		{
			playerCamera.fieldOfView = ADSFOV;
			//Debug.Log(playerCamera.fieldOfView);
		}
		if (context.canceled)
		{
			playerCamera.fieldOfView = playerFOV;
			//Debug.Log(playerCamera.fieldOfView);
		}
	}
}
