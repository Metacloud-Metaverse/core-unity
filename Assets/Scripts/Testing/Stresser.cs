using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;

public class Stresser : MonoBehaviour
{
    public GameObject prefab;
    public AvatarSystem characterCustomization;
    public int count;
    public int rowCount;
    public float distance;
    public float refreshTime;
    public float speed;
    public Toggle shadowsToggle;
    public bool isMotion = true;
    public bool useAvatarCustomization;
    
    private List<GameObject> _instances = new List<GameObject>();
    private List<Vector3> _destinies = new List<Vector3>();
    private List<Animator> _animators = new List<Animator>();

    private Vector3 _currentPosition;
    private bool _instantiationFinished;
    private float _currentTime;

    void Start()
    {
        InstantiatePrefabs();
        SetRandomDirections();
        SetRotations();
    }

    private void InstantiatePrefabs()
    {
        int i = 0;
        _currentPosition = Vector3.zero;
    
        while (i < count)
        {
            for (int j = 0; j < rowCount; j++)
            {
                var go = Instantiate(prefab);
                _instances.Add(go);
                _animators.Add(go.GetComponent<Animator>());
                go.transform.position = _currentPosition;
                _currentPosition = new Vector3(_currentPosition.x + distance,
                                               _currentPosition.y,
                                               _currentPosition.z);
                if (useAvatarCustomization)
                {
                    //characterCustomization.SetRandomAvatar(go);
                    characterCustomization.SetAvatar(go, 0, 0, 0, 0, 0, null, null, null, 0, 0, 0, 0, 1,0);
                }

                go.GetComponent<Animation>().Play("Walk");
                i++;
                if (i >= count) break;
            }
            _currentPosition = new Vector3(0,
                                           _currentPosition.y,
                                           _currentPosition.z + distance);
        }

        _instantiationFinished = true;

    }

    private void SetRandomDirections()
    {
        _destinies.Clear();

        for (int i = 0; i < count; i++)
        {
            float xRand = Random.Range(0, rowCount * distance);
            float zRand = Random.Range(0, count / rowCount * distance);
            _destinies.Add(new Vector3(xRand, 0, zRand));
        }

    }

    void Update()
    {
        if (!_instantiationFinished) return;
        if (!isMotion) return;

        for (int i = 0; i < _instances.Count; i++)
        {
            _instances[i].transform.position += _instances[i].transform.forward * speed * Time.deltaTime;
        }

        _currentTime += Time.deltaTime;
        if (_currentTime >= refreshTime)
        {
            _currentTime = 0;
            SetRandomDirections();
            SetRotations();
        }
    }

    private void SetRotations()
    {
        for (int i = 0; i < _instances.Count; i++)
        {
            Quaternion rotation = Quaternion.LookRotation(_destinies[i] - _instances[i].transform.position);
            _instances[i].transform.rotation = rotation;
        }
    }

    public void Refresh(int count, int column, float distance)
    {
        _instantiationFinished = false;

        for (int i = 0; i < _instances.Count; i++)
        {
            Destroy(_instances[i]);
        }

        _instances.Clear();
        _destinies.Clear();
        _animators.Clear();

        this.count = count;
        this.rowCount = column;
        this.distance = distance;

        InstantiatePrefabs();
        SetRandomDirections();
        SetRotations();
    }

    public void SetShadows()
    {
        var enabled = shadowsToggle.isOn;

        for (int i = 0; i < _instances.Count; i++)
        {
            var mrs = _instances[i].GetComponentsInChildren<SkinnedMeshRenderer>();
            foreach (var mr in mrs)
            {
                if(enabled)
                    mr.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
                else
                    mr.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;

            }
        }
    }

    public void SetMotion(bool enabled)
    {
        foreach (var animator in _animators)
        {
            animator.enabled = enabled;
        }
        isMotion = enabled;
    }

    public void SetQuality(int value)
    {
        QualitySettings.SetQualityLevel(value, true);
    }
}

  
