using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class PlayerControllerPhysicsHandler
{
    [Header("Body")]
    public float mass = 3;
    private Vector3 impactForce = Vector3.zero;
    [Header("Slides")]
    public float currentAngle = 0;
    public float currentHeight = 0;
    public float maxSlopeOnVerticalAngle = 45;
    public float ignoreHeightAngles = 2;
    [Range(1f,5f)] public float forceOnSlide = 2;
    private float _externalSlidingForce = 0;


    [Header("Ground Check Settings")]
    public float groundOffset = -0.14f;
	public float groundRadius = 0.5f;
    public LayerMask ignorePlayerLayer;


	public bool isGrounded { get; private set; }
    public bool isOnSlide { get; private set; }

	private ThirdPersonController _controller;
    
    
    private Transform _lasHitTransform;
    private Transform _currentHitTransform;

    public bool enablePlayerHandler { get; private set; }

    public physicsEvents OnGroundEnter;
    public delegate void physicsEvents();
    public void SetHandlerEnable(bool value)
    {
        enablePlayerHandler = value;
    }
    public void Init(ThirdPersonController player)
    {
        _externalSlidingForce = 0;
        _controller = player;
        _controller.StartCoroutine(Update());
    }
    public void AddForce(Vector3 dir, float force)
    {
        dir.Normalize();
        if (dir.y == 0)
        {
            dir.y = _controller.components.controller.velocity.y;
        }
        impactForce += dir.normalized * force / mass;
    }

     // ReSharper disable Unity.PerformanceAnalysis
     private IEnumerator Update()
    {
        while (true)
        {
            //loop infinite when this player handler is disabled
            while (enablePlayerHandler == false)
            {
                yield return null;
            }
            SlideOnVerticals();
            GroundedCheck();
            ForceCheck();

            yield return null;
        }
    }

    private void ForceCheck()
    {
        if (impactForce.magnitude > 0.2) _controller.components.controller.Move(impactForce * Time.deltaTime);
        impactForce = Vector3.Lerp(impactForce, Vector3.zero, 5 * Time.deltaTime);
    }
    public void SetOnGround()
    {
		isGrounded = true;
    }
    private void SlideOnVerticals()
    {

        float capsuleHeight = Mathf.Max(_controller.components.controller.radius, _controller.components.controller.height);
        Vector3 capsuleBottom = _controller.transform.TransformPoint(_controller.components.controller.center - Vector3.up * capsuleHeight / 2f);
        float radius = _controller.transform.TransformVector(_controller.components.controller.radius, 0f, 0f).magnitude;

        Ray ray = new Ray(capsuleBottom + _controller.transform.up * .01f, -_controller.transform.up);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, radius * 2f))
        {
            _currentHitTransform = hit.transform;
            
            if (_lasHitTransform == null || _currentHitTransform != _lasHitTransform)
            {
                _lasHitTransform = _currentHitTransform;
                currentAngle = Vector3.Angle(hit.normal, _controller.transform.up);
                currentHeight = hit.collider.bounds.size.y;
            }
            
           
#if UNITY_EDITOR
            Debug.DrawLine(hit.point, capsuleBottom + _controller.transform.forward,Color.blue);
            Debug.DrawLine(hit.point, hit.point + hit.normal, Color.blue);
#endif
            if (currentHeight >= ignoreHeightAngles)
            {
                if (currentAngle > maxSlopeOnVerticalAngle)
                {
                    isOnSlide = true;
                    var normal = hit.normal;
                    var groundParallel = Vector3.Cross(_controller.transform.up, normal);
                    var slopeParallel = Vector3.Cross(groundParallel, normal);

                    //_controller.components.controller.Move(Vector3.down * forceOnSlide * Time.deltaTime);
                    
                    _controller.move.SetSlideForce(slopeParallel.normalized * (forceOnSlide + _externalSlidingForce));
                }
                else
                {
                    _controller.move.SetSlideForce(Vector3.zero);
                    isOnSlide = false;
                }
            }
            else
            {
                _controller.move.SetSlideForce(Vector3.zero);
                isOnSlide = false;
            }

        }

    }

    private void GroundedCheck()
    {
        var spherePosition = _controller.transform.position; 
        spherePosition.y -= groundOffset;

        var hits = Physics.OverlapSphere(_controller.transform.position, groundRadius, ignorePlayerLayer.value);


        foreach (var item in hits)
        {
            if (item.isTrigger == false)
            {
                if (OnGroundEnter != null)
                {
                    OnGroundEnter.Invoke();
                }
                isGrounded = true;
                _controller.animationHandler.isGround = isGrounded;

                 return;
            }
        }
        isGrounded = false;
        _controller.animationHandler.isGround = isGrounded;
        //   isGrounded = hits.Length >= 1;

    }

    public void SetExternalSlidingForce(float force)
    {
        _externalSlidingForce = force;
    }
}
