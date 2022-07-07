using System.Linq;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerControllerThirdPerson : MonoBehaviour
{

	public PlayerControllerPhysicsHandler physics;
	public PlayerControllerInput input;
	public PlayerControllerJumpHandler jump;
	public PlayerControllerMovementHandler move;
	public PlayerControllerAnimationHandler animationHandler;
	public PlayerControllerCameraHandler cameraHandler;
	public PlayerComponents components;

	// player

	
	//vars for start menu

	private Animator _animator;

	private void Awake()
	{
	/*	input.Init(this);
		physics.Init(this);
		cameraHandler.Init(this);
		move.Init(this);
		jump.Init(this);*/
		//animationHandler.Init(this);
	}

	private void Start()
	{
	}

	private void Update()
	{
	}





	private void OnDrawGizmosSelected()
	{
		Color transparentGreen = new Color(0.0f, 1.0f, 0.0f, 0.35f);
		Color transparentRed = new Color(1.0f, 0.0f, 0.0f, 0.35f);

		if (physics.isGrounded) Gizmos.color = transparentGreen;
		else Gizmos.color = transparentRed;

		// when selected, draw a gizmo in the position of, and matching radius of, the grounded collider
		Gizmos.DrawSphere(new Vector3(transform.position.x, transform.position.y - physics.groundOffset, transform.position.z), physics.groundRadius);
	}
}
