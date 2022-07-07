using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvatarLoader : MonoBehaviour
{
    public Avatar avatar;
    public float waitForSeconds = 0;

    IEnumerator Start()
    {
        yield return new WaitForSeconds(waitForSeconds);
        avatar.Load();
    }
}
