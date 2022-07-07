using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIOnLoginController : MonoBehaviour
{
    [Header("Objetcs")]
    [SerializeField] private Scrollbar scrollBar;
    [SerializeField] private Button buttonAgree;
    [Header("Panels")]
    [SerializeField] private GameObject panelTerms;
    [SerializeField] private GameObject panelCustomize;


    [Header("Effects Settings")]
    public Vector3 initPanelScale = new Vector3(0.3f, 0.3f, 0.3f);
    public float time = 0.5f;
    public float sizeScale = 1.5f;
    public iTween.EaseType ease = iTween.EaseType.linear;
    private void Awake()
    {
        buttonAgree.interactable = false;
    }
    private void Update()
    {
        CheckTermsRead();
    }

    private void CheckTermsRead()
    {
        if (scrollBar.value <= 0 && !buttonAgree.interactable)
        {
            buttonAgree.interactable = true;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            OnButtonIAgreePress();
        }
    }

    public void OnButtonIAgreePress()
    {
        panelTerms.SetActive(false);


        panelCustomize.transform.localScale = initPanelScale;
        panelCustomize.SetActive(true);
        var hash = iTween.Hash("x",sizeScale,"y", sizeScale,"z", sizeScale, "time", time, "easeType", ease,"loop",iTween.LoopType.none);
        iTween.ScaleTo(panelCustomize, hash);
    }

    public void CustomizeAvatar()
    {
        FirstScreenManager.Instance.LoadScene(SceneIndex.CHARACTER_CUSTOMIZATION);
    }

    public void EnterWorld()
    {
        FirstScreenManager.Instance.LoadScene(SceneIndex.WORLD);
    }
}
