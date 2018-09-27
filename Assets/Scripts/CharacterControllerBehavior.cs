using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

[RequireComponent(typeof(CharacterController))]
public class CharacterControllerBehavior : MonoBehaviour
{

    [SerializeField] private Transform _absoluteTransform;
    [SerializeField] private float _mass = 75.0f;
    [SerializeField] private float _acceleration = 3;
    [SerializeField] private float _dragGround = 5.0f;

    [SerializeField] private float _maxGraceTime = 2.0f;

    [SerializeField] private float _graceTimer = 0.0f;

    private bool _wasPreviousTickGrounded = false;

    private float _maxRunningSpeed = (30.0f * 1000.0f) / (60 * 60); //30 km / h

	private bool _HasJumped = false;

    private CharacterController _charController;

    private Vector3 _velocity = new Vector3();
    private Vector3 _input = new Vector3();

    private bool _jump = false;
    private float _jumpHeight = 5.0f;

    void Start()
    {
        _charController = GetComponent<CharacterController>();
        // 	_charController = (CharacterController)GetComponent(typeof(CharacterController));
        // 	_charController = (CharacterController)GetComponent("CharacterController");
        _absoluteTransform = Camera.main.transform;

#if DEBUG
        //Debug.Assert((_charController == null), "there is no character controller on the player");
        Assert.IsNotNull(_charController, "DEPENDENCY ERROR: chararacter controller is null in CharacterControllerBehavior");
#endif
    }

    void Update()
    {
        ApplyGraceCounter();
        CheckControls();
    }

    void FixedUpdate()
    {
        ApplyGround();
        ApplyGravity();
        DoMovement();
        ApplyJump();
        LimitMaximumRunningSpeed();
        ApplyMovement();
        ApplyGroundDrag();
        //apply jump drag
    }

    void ApplyGround()
    {
        if (_charController.isGrounded)
        {
			_HasJumped = false;
            //_velocity -= Vector3.Project(_velocity, Physics.gravity);
            _velocity += Physics.gravity * Time.fixedDeltaTime;
        }
    }
    void ApplyGravity()
    {
        if (!_charController.isGrounded) _velocity += Physics.gravity * Time.fixedDeltaTime;
    }
    void DoMovement()
    {
        if (_charController.isGrounded)
        {
            Vector3 xzAbsoluteForward = Vector3.Scale(_absoluteTransform.forward, new Vector3(1, 0, 1));

            Quaternion forwardRotation = Quaternion.LookRotation(xzAbsoluteForward);

            Vector3 relativeMovement = forwardRotation * _input;

            _velocity += relativeMovement * _mass * _acceleration * Time.deltaTime;
        }

    }

    void ApplyMovement()
    {
        Vector3 displacement = _velocity * Time.deltaTime;
        _charController.Move(displacement);
    }

    void ApplyGroundDrag()
    {
        if (_charController.isGrounded)
        {
            _velocity = _velocity * (1 - Time.deltaTime * _dragGround);
        }
    }

    void LimitMaximumRunningSpeed()
    {
        Vector3 vely = Vector3.Scale(_velocity, new Vector3(0, 1, 0));
        Vector3 velxz = new Vector3(_velocity.x, 0, _velocity.z);

        Vector3 clamped = Vector3.ClampMagnitude(velxz, _maxRunningSpeed);

        _velocity = clamped + vely;
    }

    void ApplyJump()
    {
        if (_jump && (_charController.isGrounded || _graceTimer > 0.0f))
        {
			_HasJumped = true;
            _velocity += -Physics.gravity.normalized * Mathf.Sqrt(2 * Physics.gravity.magnitude * _jumpHeight);
            _jump = false;
            _graceTimer = 0.0f;
        }
    }


    private void ApplyGraceCounter()
    {
        //update grace timer
        _graceTimer -= Time.deltaTime;

        //if player was previously grounded & is not grounded now -> activate grace timer
        if (!_charController.isGrounded && _wasPreviousTickGrounded && !_HasJumped)
        {
			_graceTimer = _maxGraceTime;
		}

        //set previous grounded state to this state
        _wasPreviousTickGrounded = _charController.isGrounded;
    }

    private void CheckControls()
    {
        _input.x = Input.GetAxis("Horizontal");

        _input.z = Input.GetAxis("Vertical");


        if (Input.GetKeyDown(KeyCode.Space) && (_charController.isGrounded || _graceTimer > 0.0f))
        {
			_jump = true;
            _velocity.y = 0;
        }

    }
}
