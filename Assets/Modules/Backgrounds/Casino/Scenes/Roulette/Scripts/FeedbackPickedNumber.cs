using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class FeedbackPickedNumber
{
    [Header("Feedback On Number Pick On Table")]
    private Transform feedbackPickedNumberOriginalParent;
    public Transform feedbackPickedNumber;


    public void DisableEffect()
    {
        feedbackPickedNumber.SetParent(feedbackPickedNumberOriginalParent);
        feedbackPickedNumber.gameObject.SetActive(false);
    }

    public void SetFeedbackPickedNumberPosition(Transform root)
    {
        feedbackPickedNumber.gameObject.SetActive(true);
        feedbackPickedNumber.SetParent(root);
        feedbackPickedNumber.transform.localPosition = Vector3.zero;
    }
    public void SetFeedbackPickedNumberDisabled()
    {
        feedbackPickedNumber.SetParent(null);
        feedbackPickedNumber.gameObject.SetActive(false);
       // feedbackPickedNumber.transform.localPosition = Vector3.zero;
    }
    public void Init()
    {
        feedbackPickedNumberOriginalParent = feedbackPickedNumber.transform.parent;
    }

}
