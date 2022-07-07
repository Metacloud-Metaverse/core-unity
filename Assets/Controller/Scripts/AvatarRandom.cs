using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvatarRandom : MonoBehaviour
{
    private void Start()
    {
        AvatarSystem.Instance.SetRandomAvatar(gameObject, true);
    }
}
