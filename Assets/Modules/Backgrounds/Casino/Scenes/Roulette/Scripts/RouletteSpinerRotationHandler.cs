using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
[System.Serializable]
public class RouletteSpinerRotationHandler 
{
    public float maxSpeed = 500;
    public float minSpeed = 25;
    public AudioClip clipEndSpin;
    
    public float anglePerNumber = 9.72973f;
    private float _currnetSpeed;
    private int _resultNumber;
    private float _currentProgress;
    private RouletteSpiner _target;
    public void RotateSpinner(RouletteSpiner target, UnityAction callback, int resultNumber)
    {
        _target = target;
        _resultNumber = resultNumber;
        _currentProgress = 0;
        _target.StartCoroutine(RotateSpinnerCoroutine(callback));
    }
    private IEnumerator RotateSpinnerCoroutine(UnityAction callback)
    {
        int rounds = 2;
        var targetAngle = anglePerNumber * _resultNumber;
        var angle = 0f;
        var currentTotalAngleSum = 0f;
        var totalRoundAngle = rounds * 360f;
        totalRoundAngle += targetAngle;
        while (currentTotalAngleSum <= totalRoundAngle)
        {
            if (angle > 360)
            {
                angle -= 360;
                rounds--;
            }
            angle += _currnetSpeed * Time.deltaTime;
            currentTotalAngleSum += _currnetSpeed * Time.deltaTime;
            _currentProgress = currentTotalAngleSum * 1 / totalRoundAngle;
            _currnetSpeed = Mathf.Lerp(maxSpeed, minSpeed, _currentProgress);
            _target.transform.rotation = Quaternion.Euler(0, 0, angle);
            yield return null;

        }
        if(callback != null)
            callback.Invoke();
        SoundControllerRoulette.Instance.PlayClip(clipEndSpin);
    }
}
