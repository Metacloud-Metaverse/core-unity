using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LoadingGuest : MonoBehaviour
{
    public TextMeshProUGUI loadingLabel;
    public string loadingMessage = "Loading...";
    public string loginSuccessful { get { return $"Hello, {Client.Instance.name}!"; } }
    private void Start()
    {
        loadingLabel.text = loadingMessage;
        Client.Instance.loginCallback = SetLoginSuccessfulMessage;
    }

    private void SetLoginSuccessfulMessage()
    {
        loadingLabel.text = loginSuccessful;
    }
}
