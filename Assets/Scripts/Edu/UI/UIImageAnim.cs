using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class UIImageAnim : MonoBehaviour
{
    [SerializeField] private Image img;

    public float alphaFrom = 0;
    public float alphaTo = 1;
    public float timeAlpha;
    public iTween.EaseType ease;

    private void Awake()
    {
        if (img == null)
            img = GetComponent<Image>();

        var hash = iTween.Hash("from", alphaFrom, "to", alphaTo, "time", timeAlpha, "onupdate", "UpdateImageAlpha","loopType",iTween.LoopType.pingPong);
        //make iTween call:
        iTween.ValueTo(gameObject, hash);
    }

#if UNITY_EDITOR
    [ContextMenu("GetImg")]
    private void GetImg()
    {
        img = GetComponent<Image>();
        UpdateImageAlpha(alphaFrom);
    }

#endif

    //since our ValueTo() iscalculating floats the "onupdate" callback will expect a float as well:
    private void UpdateImageAlpha(float newValue)
    {
        var color = img.color;
        color.a = newValue;
        img.color = color;
    }
}
