using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIGeneralPopup : MonoBehaviour
{
    public static UIGeneralPopup instance;
    [SerializeField] private GameObject root;
    [SerializeField] private TextMeshProUGUI textTittle;
    [SerializeField] private TextMeshProUGUI textDescription;
    [SerializeField] private List<UnityEngine.UI.Button> buttons = new List<UnityEngine.UI.Button>();
    [SerializeField] private ItweenGenericUtil has;
   
    private Vector3 _initSize;
    private Color _initTittleColor;
    private Color _initDescripCOlor;

    private Queue<PopupStruct> _targetsQueuePopups = new Queue<PopupStruct>();
    private bool _checkNext;
    private void Awake()
    {
        instance = this;
        _checkNext = true;
        _initSize = transform.localScale;
        transform.localScale = Vector3.zero;
        _initTittleColor = textTittle.color;
        _initDescripCOlor = textDescription.color;
    }

    public void Set(string tittle, string descrpiton)
    {
        _targetsQueuePopups.Enqueue(new PopupStruct(tittle, descrpiton));
        Process();
    }

    private void Process()
    {
        if (_targetsQueuePopups.Count > 0 && _checkNext)
        {
            _checkNext = false;
            var currentProcess = _targetsQueuePopups.Dequeue();
            SetTittle(currentProcess.tittle);
            SetDescription(currentProcess.description);
            Open();
        }
    }
    #region Builder
    public UIGeneralPopup SetTittle(string text)
    {
        textTittle.text = text;
        textTittle.color = _initTittleColor;
        return this;
    }
    public UIGeneralPopup SetDescription(string text)
    {
        textDescription.text = text;
        textDescription.color = _initDescripCOlor;
        return this;
    }
    public UIGeneralPopup SetButton(string buttonText,UnityEngine.Events.UnityAction callback, bool close )
    {
        for (int i = 0; i < buttons.Count; i++)
        {
            if(buttons[i].gameObject.activeInHierarchy == false)
            {
                buttons[i].GetComponentInChildren<TextMeshProUGUI>().text = buttonText;
                buttons[i].onClick.RemoveAllListeners();
                buttons[i].onClick.AddListener(callback);
                if (close)
                    buttons[i].onClick.AddListener(Close);
                buttons[i].gameObject.SetActive(true);
                break;
            }
        }
        return this;
    }

    #endregion

    public UIGeneralPopup Open()
    {
        if (has)
            iTween.ScaleTo(gameObject, has.GetHas());
        root.SetActive(true);
        return this;
    }
  


    public void Close()
    {
        for (int i = 0; i < buttons.Count; i++)
        {
            if (buttons[i].gameObject.activeInHierarchy == true)
            {
                buttons[i].GetComponentInChildren<TextMeshProUGUI>().text = "";
                buttons[i].onClick.RemoveAllListeners();
                buttons[i].gameObject.SetActive(false);
            }
        }
        CloseITween();
        textTittle.text = "";
        textDescription.text = "";
        _checkNext = true;      
        Process();
    }


    private void CloseITween()
    {
        Hashtable hass = new Hashtable();
        hass.Add("easeType", has.ease);
        hass.Add("loopType", has.loop);
        hass.Add("islocal", has.isLocal);
        hass.Add("onCompleted", "onCompletedClosed");
        hass.Add("x", 0);
        hass.Add("y", 0);
        hass.Add("z", 0);
        hass.Add("time", has.time);
        iTween.ScaleTo(gameObject, hass);
    }
    private void onCompletedClosed()
    {
        root.SetActive(false);
    }
    struct PopupStruct
    {
        public string tittle;
        public string description;
        public PopupStruct(string tittle, string description)
        {
            this.tittle = tittle;
            this.description = description;
        }
    }
}
