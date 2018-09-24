using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

[RequireComponent(typeof(CharacterController))]
public class CharacterControllerBehavior : MonoBehaviour {

	private CharacterController _charController;

	
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
	
	void Update ()
	{
		
	}

	void HandleMovement()
	{
		
	}
}
