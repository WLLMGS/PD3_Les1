using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

[RequireComponent(typeof(CharacterController))]
public class CharacterControllerBehavior : MonoBehaviour {

	[SerializeField] private Transform _absoluteTransform;

	private CharacterController _charController;

	private Vector3 _velocity = new Vector3();
	private Vector3 _input = new Vector3();

	void Start ()
	 { 
		_charController = GetComponent<CharacterController>();
		// 	_charController = (CharacterController)GetComponent(typeof(CharacterController));
		// 	_charController = (CharacterController)GetComponent("CharacterController");

		#if DEBUG
		//Debug.Assert((_charController == null), "there is no character controller on the player");
		Assert.IsNotNull(_charController, "DEPENDENCY ERROR: chararacter controller is null in CharacterControllerBehavior");
		#endif
	 }
	
	void Update()
	{
		_input.x = Input.GetAxis("Horizontal");
		
		_input.z = Input.GetAxis("Vertical");
		
	}

	void FixedUpdate ()
	{
		ApplyGround();
		ApplyGravity();
		DoMovement();
	}

	void ApplyGround()
	{
		if(_charController.isGrounded)
		{
			_velocity -= Vector3.Project(_velocity, Physics.gravity);
		}
	}
	void ApplyGravity()
	{
		if(!_charController.isGrounded)	_velocity += Physics.gravity * Time.fixedDeltaTime;
	}
	void DoMovement()
	{
		Vector3 displacement = _velocity * Time.fixedDeltaTime;
		_charController.Move(displacement);
	}

	void ApplyMovement()
	{
		
	}

}
