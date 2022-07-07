using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallRoulette : MonoBehaviour
{
    public Transform targetPosition;
    public AudioSource aSourceBall;
    public AudioClip ballClip;
    public float jumpTimes = 8;
    public float moveBallSpeed = 100;
    public float speedJumpTransition = 5;
    public Vector3 targetJumpScale = new Vector3(0.65f, 0.65f, 0.65f);

    private Vector3 _initSizeBall;
    private Vector3 _initPosition;

    private Coroutine _moveBallCoroutine;
    private Coroutine _jumpBallCoroutine;
    private void Awake()
    {
        _initSizeBall = transform.localScale;
        _initPosition = transform.localPosition;
    }

    public void ResetPosition()
    {
        transform.localPosition = _initPosition;
    }

    public void MoveBallToCenter()
    {
        if(_moveBallCoroutine != null)
            StopCoroutine(_moveBallCoroutine);
        if(_jumpBallCoroutine != null)
            StopCoroutine(_jumpBallCoroutine);
        StartCoroutine(MoveBall());
        StartCoroutine(JumpBall());
    }
    
    private IEnumerator MoveBall()
    {
        float magnitudeSin = 0.2f;
        while (Vector3.Distance(transform.localPosition, targetPosition.localPosition) > 0.1f)
        {
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, targetPosition.localPosition,
                moveBallSpeed * Time.deltaTime);
            yield return null;
        }
    }
    private IEnumerator JumpBall()
    {
        var fixedSpeed = speedJumpTransition;
        var fixedTargetSize = targetJumpScale;
        for (int i = 0; i < jumpTimes; i++)
        {
            aSourceBall.PlayOneShot(ballClip);
            //  aSourceBall.pitch -= 0.01f;
            while (transform.localScale != fixedTargetSize)
            {
                transform.localScale = Vector3.MoveTowards(transform.localScale, fixedTargetSize, fixedSpeed * Time.deltaTime);
                yield return null;
            }
            while (transform.localScale != _initSizeBall)
            {
                transform.localScale = Vector3.MoveTowards(transform.localScale, _initSizeBall, fixedSpeed * Time.deltaTime);
                yield return null;
            }

            if (fixedTargetSize.x < _initSizeBall.x)
            {
                fixedTargetSize = _initSizeBall;
            }
        }
    }
}
