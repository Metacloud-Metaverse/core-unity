using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[System.Serializable]
public class PlayerControllerMovementHandler 
{

	[Tooltip("Move speed of the character in m/s")]
	public float moveSpeed = 2.0f;
	[Tooltip("Sprint speed of the character in m/s")]
	public float sprintSpeed = 5.335f;
	[Tooltip("How fast the character turns to face movement direction")]
	[Range(0.0f, 0.3f)]
	public float rotationSmoothTime = 0.12f;
	[Tooltip("Acceleration and deceleration")]
	public float speedChangeRate = 10.0f;

	private float _speed;
	private float _animationBlend;
	private float _targetRotation = 0.0f;
	private float _rotationVelocity;
	private Vector3 _externalForce;
	private Vector3 _slideForce;
	public Vector3 targetVector;

	private bool _isStopped;

	private ThirdPersonController _player;

    public void Init(ThirdPersonController player)
    {
		_isStopped = false;
        _player = player;
        _player.StartCoroutine(Update());
    }
    private IEnumerator Update()
    {
        while (true)
        {
			Move();
            yield return null;
        }
    }

	public void StopForce(float time)
    {
		_player.StartCoroutine(StopProcces(time));
    }
	private IEnumerator StopProcces(float time)
    {
		_isStopped = true;
		_player.components.controller.Move(new Vector3(0, _player.components.controller.velocity.y, 0));
		//TODO PONER CON CALSE YIELDERS
		yield return new WaitForSeconds(time);
		_isStopped = false;
	}
	private void Move()
	{
		
		// set target speed based on move speed, sprint speed and if sprint is pressed
		float targetSpeed = !_player.input.sprint ? sprintSpeed : moveSpeed;

		// a simplistic acceleration and deceleration designed to be easy to remove, replace, or iterate upon

		// note: Vector2's == operator uses approximation so is not floating point error prone, and is cheaper than magnitude
		// if there is no input, set the target speed to 0
		if (_player.input.axis == Vector2.zero) targetSpeed = 0.0f;

		// a reference to the players current horizontal velocity
		var velocity = _player.components.controller.velocity;
		float currentHorizontalSpeed = new Vector3(velocity.x, 0.0f, velocity.z).magnitude;

		float speedOffset = 0.1f;

		float inputMagnitude = _player.input.axis.magnitude;

		// accelerate or decelerate to target speed
		if (currentHorizontalSpeed < targetSpeed - speedOffset || currentHorizontalSpeed > targetSpeed + speedOffset)
		{
			// creates curved result rather than a linear one giving a more organic speed change
			// note T in Lerp is clamped, so we don't need to clamp our speed
			_speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed * inputMagnitude, Time.deltaTime * speedChangeRate);

			// round speed to 3 decimal places
			_speed = Mathf.Round(_speed * 1000f) / 1000f;
		}
		else
		{
			_speed = targetSpeed;
		}
		_animationBlend = Mathf.Lerp(_animationBlend, targetSpeed, Time.deltaTime * speedChangeRate);

		// normalise input direction
		Vector3 inputDirection = new Vector3(_player.input.axis.x, 0.0f, _player.input.axis.y).normalized;
		
		//If player is onSlide Angle, divide de forward direction
		if (_player.physics.isOnSlide)
		{
			if (inputDirection.z > 0)
			{
				inputDirection.z = inputDirection.z / 2;
			}
		}
		


		// note: Vector2's != operator uses approximation so is not floating point error prone, and is cheaper than magnitude
		// if there is a move input rotate player when the player is moving
		if (inputDirection != Vector3.zero)
		{
			var inputOnJump = inputDirection;
            if (_player.animationHandler.isJumping)
            {
                
			}
			_targetRotation = Mathf.Atan2(inputOnJump.x, inputOnJump.z) * Mathf.Rad2Deg + _player.components.camera.transform.eulerAngles.y;
			float rotation = Mathf.SmoothDampAngle(_player.transform.eulerAngles.y, _targetRotation, ref _rotationVelocity, rotationSmoothTime);

			// rotate to face input direction relative to camera position
			_player.transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
		}


		
		Vector3 targetDirection = Quaternion.Euler(0.0f, _targetRotation, 0.0f) * Vector3.forward;

		//if stoped is active put direction to Zero
		if (_isStopped)
		{
			targetDirection = Vector3.zero;
		}

		targetVector = targetDirection.normalized * (_speed * Time.deltaTime) + new Vector3(0.0f, _player.jump.verticalVelocity, 0.0f) * Time.deltaTime;
	
		
		//if (targetVector.y < 0 && _player.physics.isGrounded && _externalForce != Vector3.zero) 
		//	targetVector.y = 0;
		
		//Add force external force movement previous MovePlayer with inputs
		if (_externalForce != Vector3.zero)
			_player.components.controller.Move(_externalForce * Time.deltaTime);
		if (_slideForce != Vector3.zero)
			_player.components.controller.Move((_slideForce) * Time.deltaTime);
		//Move Player
		_player.components.controller.Move(targetVector);
        

		

		//	_player.components.controller.Move(externalForce * Time.deltaTime);
		//	else 
		//	_player.components.rb.velocity =(targetDirection.normalized * (_speed * Time.deltaTime) + new Vector3(0.0f, _player.jump.verticalVelocity, 0.0f) * Time.deltaTime);

		// update animator if using character
		_player.animationHandler.SetMoveBlend(_animationBlend);
		_player.animationHandler.SetMoveSpeed(inputMagnitude);
	
	}

	public void SetSlideForce(Vector3 force)
	{
		_slideForce = force;
	}
	public void SetExternalForce(Vector3 force)
	{
		_externalForce = force;
	}

}
