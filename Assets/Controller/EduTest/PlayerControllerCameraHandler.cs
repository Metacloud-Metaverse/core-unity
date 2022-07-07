using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerControllerCameraHandler 
{
    [Header("Cinemachine")]
    [Tooltip("The follow target set in the Cinemachine Virtual Camera that the camera will follow")]
    public GameObject CinemachineCameraTarget;
    [Tooltip("How far in degrees can you move the camera up")]
    public float TopClamp = 70.0f;
    [Tooltip("How far in degrees can you move the camera down")]
    public float BottomClamp = -30.0f;
    [Tooltip("Additional degress to override the camera. Useful for fine tuning camera position when locked")]
    public float CameraAngleOverride = 0.0f;
    [Tooltip("For locking the camera position on all axis")]

    public float sensibility = 0.8f;
    public float damping = 10;
    public bool LockCameraPosition = false;

    private const float _threshold = 0.01f;

    // cinemachine
    private float _cinemachineTargetYaw;
    private float _cinemachineTargetPitch;

    private float _targetX;
    private float _targetY;


    private ThirdPersonController _player;

    public void Init(ThirdPersonController player)
    {
        _player = player;
        _player.StartCoroutine(Update());
    }
    private IEnumerator Update()
    {
        while (true)
        {
            CameraRotation();
            yield return null;
        }
    }
    private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
    {
        if (lfAngle < -360f) lfAngle += 360f;
        if (lfAngle > 360f) lfAngle -= 360f;
        return Mathf.Clamp(lfAngle, lfMin, lfMax);
    }
    private void CameraRotation()
    {
        // if there is an input and camera position is not fixed
        if (_player.input.look.sqrMagnitude >= _threshold && !LockCameraPosition)
        {
            //Don't multiply mouse input by Time.deltaTime;

            _cinemachineTargetYaw += _player.input.look.x* sensibility;
            _cinemachineTargetPitch += _player.input.look.y* sensibility;
        }

        // clamp our rotations so our values are limited 360 degrees
        _cinemachineTargetYaw = ClampAngle(_cinemachineTargetYaw, float.MinValue, float.MaxValue);
        _cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, BottomClamp, TopClamp);

        // Cinemachine will follow this target

        var desiredRotQ = Quaternion.Euler(_cinemachineTargetPitch + CameraAngleOverride, _cinemachineTargetYaw, 0.0f);
        CinemachineCameraTarget.transform.rotation = desiredRotQ;
     //  CinemachineCameraTarget.transform.rotation = Quaternion.RotateTowards(CinemachineCameraTarget.transform.rotation, desiredRotQ, damping * Time.deltaTime);
        //     CinemachineCameraTarget.transform.rotation = Quaternion.Lerp(CinemachineCameraTarget.transform.rotation, desiredRotQ, Time.deltaTime * damping);
    }
}
