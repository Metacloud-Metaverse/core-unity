using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MoveUIPanelTarget : MonoBehaviour
{
    public float time = 0.5f;
    public iTween.EaseType ease;
    public RectTransform openTargetRect;
    public RectTransform closeTargetRect;

    private Coroutine desactiveCoroutine;

    private bool isOpen;
    private void Awake()
    {
    }
    private void Update()
    {
    }

#if UNITY_EDITOR
    [ContextMenu("Move To Close")]
    public void MoveToClosePos()
    {
        transform.position = closeTargetRect.position;

    }
    [ContextMenu("Move To Open")]
    public void MoveToCloseOpen()
    {
        transform.position = openTargetRect.position;
    }
#endif
    public void Open()
    {
        if (!isOpen)
        {
            gameObject.SetActive(true);
            if (desactiveCoroutine != null)
                StopCoroutine(desactiveCoroutine);
            iTween.Stop(gameObject);
            var hash = iTween.Hash("position", openTargetRect.position, "time", time, "easeType", ease);
            iTween.MoveTo(gameObject, hash);
            isOpen = true;
        }
    }
    public void Close()
    {
        if (isOpen)
        {
            iTween.Stop(gameObject);
            var hash = iTween.Hash("position", closeTargetRect.position, "time", time, "easeType", ease);
            iTween.MoveTo(gameObject, hash);
            isOpen =false;
            if (desactiveCoroutine != null)
                StopCoroutine(desactiveCoroutine);
            desactiveCoroutine = StartCoroutine(DesactiveCoroutine());
        }
    }

    private IEnumerator DesactiveCoroutine()
    {
        yield return Yielders.Seconds(time);
        gameObject.SetActive(false);
    }
}
