using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerControllerJumpHandler
{
	[Space(10)]
	[Tooltip("The height the player can jump")]
	public float JumpHeight = 1.2f;
	[Tooltip("The character uses its own gravity value. The engine default is -9.81f")]
	public float Gravity = -15.0f;

	[Space(10)]
	[Tooltip("Time required to pass before being able to jump again. Set to 0f to instantly jump again")]
	public float JumpTimeout = 0.50f;
	[Tooltip("Time required to pass before entering the fall state. Useful for walking down stairs")]
	public float FallTimeout = 0.15f;


	// timeout deltatime
	private float _jumpTimeoutDelta;
	private float _fallTimeoutDelta;


	public float verticalVelocity { get; private set; }
	private float _terminalVelocity = 53.0f;


	public bool enablePlayerHandler { get; private set; }
	private ThirdPersonController _player;

	public void SetHandlerEnable(bool value)
	{
		enablePlayerHandler = value;
	}
	public void Init(ThirdPersonController player)
	{
		_player = player;
		_player.StartCoroutine(CustomUpdate());
		_jumpTimeoutDelta = JumpTimeout;
		_fallTimeoutDelta = FallTimeout;
	}

	private IEnumerator CustomUpdate()
	{
		while (true)
		{
			//loop infinite when this player handler is disabled
			while (enablePlayerHandler == false)
			{
				yield return null;
			}
			JumpAndGravity();
			yield return null;
		}
	}
	private void JumpAndGravity()
	{
		if (_player.physics.isGrounded)
		{
			// reset the fall timeout timer
			_fallTimeoutDelta = FallTimeout;


			// update animator if using character
			_player.animationHandler.SetJump(false);
			_player.animationHandler.SetFalling(false);

			// stop our velocity dropping infinitely when grounded
			if (verticalVelocity < 0.0f)
			{
				verticalVelocity = -2f;
			}

			// Jump
			if (_player.input.jump && _jumpTimeoutDelta <= 0.0f)
			{
				// the square root of H * -2 * G = how much velocity needed to reach desired height
				verticalVelocity = Mathf.Sqrt(JumpHeight * -2f * Gravity);

				// update animator if using character
				_player.animationHandler.SetJump(true);
			}

			// jump timeout
			if (_jumpTimeoutDelta >= 0.0f)
			{
				_jumpTimeoutDelta -= Time.deltaTime;
			}
		}
		else
		{
			// reset the jump timeout timer
			_jumpTimeoutDelta = JumpTimeout;

			// fall timeout
			if (_fallTimeoutDelta >= 0.0f)
			{
				_fallTimeoutDelta -= Time.deltaTime;
			}
			else
			{
				// update animator if using character
				_player.animationHandler.SetFalling(true);
			}

			// if we are not grounded, do not jump
			_player.input.jump = false;
		}

		// apply gravity over time if under terminal (multiply by delta time twice to linearly speed up over time)
		if (verticalVelocity < _terminalVelocity)
		{
			verticalVelocity += Gravity * Time.deltaTime;
		}
	}
}
