using UnityEngine;
using UnityEngine.UI;
public class CameraCharacterCustomization : MonoBehaviour
{
    private Transform _startMarker;
    private Transform _endMarker;
    private float _startTime;
    private int _renderTextureDepth = 24;
    private int _antialias = 2;
    private float _journeyLength;
    private bool _isInterpolating;

    public float speed = 1.0f;

    public Transform initialPosition;
    public Transform watchHeadPosition;
    public Transform watchTorsoPosition;
    public Transform watchLegsPosition;
    public Transform watchFeetPosition;

    public Camera me;
    public RawImage rawImage;
    public RectTransform rt;

    void Start()
    {        
        var tex = new RenderTexture((int)rt.rect.width, (int)rt.rect.height, _renderTextureDepth, RenderTextureFormat.RGB111110Float);
        tex.antiAliasing = _antialias;
        me.targetTexture = tex;
        rawImage.texture = me.targetTexture;
    }


    void LateUpdate()
    {
        Interpolate();
    }


    private void Interpolate()
    {
        if (!_isInterpolating) return;

        float distCovered = (Time.time - _startTime) * speed;

        float fractionOfJourney = distCovered / _journeyLength;

        transform.position = Vector3.Lerp(_startMarker.position, _endMarker.position, fractionOfJourney);
        if (transform.position == _endMarker.position)
        {
            _isInterpolating = false;
        }
    }

    public void WatchHead()
    {
        InitializeInterpolation(watchHeadPosition);
    }

    public void WatchTorso()
    {
        InitializeInterpolation(watchTorsoPosition);
    }

    public void WatchLegs()
    {
        InitializeInterpolation(watchLegsPosition);
    }

    public void WatchFeet()
    {
        InitializeInterpolation(watchFeetPosition);
    }

    public void BackToInitialPosition()
    {
        if (transform.position == initialPosition.position) return;
        InitializeInterpolation(initialPosition);
    }

    private void InitializeInterpolation(Transform _endMarker)
    {
        if (_endMarker.position == transform.position) return;
        _startMarker = transform;
        this._endMarker = _endMarker;
        _startTime = Time.time;

        _journeyLength = Vector3.Distance(_startMarker.position, _endMarker.position);
        _isInterpolating = true;
    }

    
}
