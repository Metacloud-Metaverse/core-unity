using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class PlayerControllerPhysics 
{
   /* [Header("Gravity")]
    public bool personalizedGravity = true;
    public float gravityScale = 2;
    private float gravityStopOnAngle = 1;

    [Header("Solpe")]
    public float maxSlopeOnVerticalAngle = 45;
    public float currentAngle { get; private set; }
    public bool isOnDenySlope { get; private set; }
    public bool IsGrounded { get; private set; }
    private bool prevGrounded = false;
    public delegate void PlayerEvent();
    [Header("isGrounded")]
    public PlayerEvent OnGroundHit;
    public float groundRadius = 0.2f;

    private float prevGravity;
    private float prevVelocityYRB;
    
        
    private ThirdPersonController _controller;
    private Transform transform;

    public void Init(ThirdPersonController controller)
    {
        _controller = controller;
        transform = _controller.transform;
                prevGravity = _controller.physic.gravityScale;
        _controller.StartCoroutine(Update());
    }

    IEnumerator Update()
    {
        while (true)
        {
            AddGravity();
            SlideOnVerticals();
            CheckGrounded();
            CheckStopVelocityOnMinorAngles();
            yield return null;
        }
    }
    private void CheckStopVelocityOnMinorAngles()
    {
        if (_controller.input.axis == Vector2.zero && currentAngle >= 30 && currentAngle <= maxSlopeOnVerticalAngle)
        {
            _controller.components.rb.velocity = Vector3.zero;
            gravityStopOnAngle = 0;
        }
        else
            gravityStopOnAngle = 1;
    }
    private void AddGravity()
    {
        if (!personalizedGravity) return;
          _controller.components.rb.AddForce(Physics.gravity * (gravityScale), ForceMode.Acceleration);
    }

    private void SlideOnVerticals()
    {

        float capsuleHeight = Mathf.Max(_controller.components.coll.radius * 2f, _controller.components.coll.height);
        Vector3 capsuleBottom = transform.TransformPoint(_controller.components.coll.center - Vector3.up * capsuleHeight / 2f);
        float radius = transform.TransformVector(_controller.components.coll.radius, 0f, 0f).magnitude;

        Ray ray = new Ray(capsuleBottom + transform.up * .01f, -transform.up);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, radius * 5f))
        {
            currentAngle= Vector3.Angle(hit.normal, transform.up);
            if (currentAngle > maxSlopeOnVerticalAngle)
            {
                Debug.Log("Slide");
                isOnDenySlope = false;
                var normal = hit.normal;
                var groundParallel = Vector3.Cross(_controller.transform.up, normal);
                var slopeParallel = Vector3.Cross(groundParallel, normal);
                _controller.components.rb.position += slopeParallel.normalized / 7;
          //      prevGravity = _controller.physic.gravityScale;
          //      _controller.physic.gravityScale = 6;
            }
            else
            {
                if (_controller.physic.gravityScale != prevGravity)
                {
             ///       _controller.physic.gravityScale = prevGravity;
                }
            }
        }

    }

    private void CheckGrounded()
    {
        var hits = Physics.OverlapSphere(transform.position, groundRadius).Where(x => x.GetComponent<ThirdPersonController>() == null).ToList();

        if (hits.Count >= 1)
            IsGrounded = true;
        else
            IsGrounded = false;

        //IsGrounded = Physics.OverlapSphere(transform.position, groundRadius);

        if (prevGrounded != IsGrounded)
        {
            if (OnGroundHit != null && prevGrounded == false)
            {
                OnGroundHit.Invoke();
            }
            prevGrounded = IsGrounded;
        }
        //check cases where velocityOfRigibody get stuck on Y when player jump, and the player already is on ground
        //this happends when try jump on a top collider and stop inmediatly the jump process and dnot have time to 
        //check the jumps bools
        if (IsGrounded && _controller.jump.isJumping)
        {
            if (_controller.components.rb.velocity.y != prevVelocityYRB)
            {
                prevVelocityYRB = _controller.components.rb.velocity.y;
            }     
        }
    }*/

}
