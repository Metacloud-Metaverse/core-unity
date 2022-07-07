using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerControllerActionMovement
{

    /*
    public float runningSpeed = 4f;
    public float walkingSpeed = 1.8f;
    public float turnSmoothTime = 0.1f;

    //Slope
    [Header("Slope")]
    public LayerMask layersCheckSlope;
    public float slopeRadious = 1;
    public float maxHeightSlope = 1;
    public Vector3 offset;
    public float forceSlope = 1;




    public float currentSpeed { get; private set; }


    private Vector3 _inputDirection;
    private Vector3 _finalDirection;
    private float turnSmoothVelocity;
    private float _currentSpeed;


    private ThirdPersonController _controller;
    private Transform transform;
    public void Init(ThirdPersonController controller)
    {
        _controller = controller;
        transform = _controller.transform;
        _controller.StartCoroutine(Update());
    }
    private IEnumerator Update()
    {
        while (true)
        {
            Move();
            SetWalkingMode();
        AplyMovement();
            yield return null;
        }
    }
    private float _speed;
    private float _targetRotation;
    public float _rotationVelocity;
    private float _verticalVelocity;
    [Range(0.0f, 0.3f)]
    public float RotationSmoothTime = 0.12f;
    public float SpeedChangeRate = 10.0f;

    private void Move()
    {
        _inputDirection = new Vector3(_controller.input.axis.x, 0, _controller.input.axis.y);
        //TODO : RAY CAST FOWARD TO STOP INPUT WHEN HAVE COLLISION IN FRONT (EXAMPLE WALL)

        if (_inputDirection.magnitude > 0)
        {
            SlopeHelper();

            var targetAngle = Mathf.Atan2(_inputDirection.x, _inputDirection.z) * Mathf.Rad2Deg + _controller.components.cam.eulerAngles.y;
            var angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0, angle, 0);

            _finalDirection = Quaternion.Euler(0, targetAngle, 0) * Vector3.forward * _currentSpeed;
        }
    }

    private void SetWalkingMode()
    {
        if (_controller.input.sprint)
        {
            _currentSpeed = walkingSpeed;
            _controller._animationManager.walkingMode = true;
        }
        else
        {
            _currentSpeed = runningSpeed;
            _controller._animationManager.walkingMode = false;
        }
    }
    */
    private void SlopeHelper()
    {
      /*  if (_controller.jump.isJumping)
        {
            return;
        }
        //  var pos = transform.position + (transform.forward + offset);
        var pos = transform.position + (transform.forward * offset.x) + (transform.up * offset.y);
        var hits = Physics.OverlapSphere(pos, slopeRadious, layersCheckSlope.value);
        if (hits.Length > 0)
        {
            var height = hits[0].bounds.size.y;
            //IF Hit height is more than maxHeight Slope then help!
            if (height <= maxHeightSlope)
            {
                transform.position += Vector3.up * forceSlope * Time.deltaTime;
            }
        }*/
    }
    private void AplyMovement()
    {
       /* var x = _finalDirection.x * _inputDirection.magnitude;
        var y = _controller.components.rb.velocity.y;
        var z = _finalDirection.z * _inputDirection.magnitude;
        _controller.components.rb.velocity = new Vector3(x, y, z);
        if (_controller.components.rb.velocity.magnitude < 0.01f && _controller.jump.isJumping == false )
        {
            Debug.Log("Force RB Velocity");
            _controller._animationManager.velocity = Vector3.zero;
        }
        else
        {
            _controller._animationManager.velocity = _controller.components.rb.velocity;
        }*/
    }
}
