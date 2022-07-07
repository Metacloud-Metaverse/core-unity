using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CircularMenu : MonoBehaviour
{
    public List<UIChangeTargetScaleOnMouseStay> buttons = new List<UIChangeTargetScaleOnMouseStay>();
    public float radius = 300;
    public float buttonSize = 1;

    private int prevIndex = 0;
    public float offset = 40;

    public GameObject rootPanel;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        DetectMousePosition();
    }
    [ContextMenu("GenerateMenu")]
    public void CreateCircle()
    {
        
        float radianSeparation = (Mathf.PI * 2) / buttons.Count;
        for (int i = 0; i < buttons.Count; i++)
        {
            float x = Mathf.Sin(radianSeparation * i) * radius;
            float y = Mathf.Cos(radianSeparation * i) * radius;
            SetButtonPos(i, x, y);
            SetImg(i, x, y);
        }
    }

    public void OpenPanel(bool value)
    {
        if (value == true)
        {
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        rootPanel.SetActive(value);
    }

    private void SetButtonPos(int i, float x, float y)
    {
        buttons[i].GetComponent<RectTransform>().anchoredPosition = new Vector3(x, y, 0);
    }

    private void SetImg(int i, float x, float y)
    {

        var rect = buttons[i].GetComponent<RectTransform>();
        rect.localScale = Vector3.one * buttonSize;
        rect.eulerAngles = Vector3.zero;
        Vector3 dir = rect.position - transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;  // Assuming you want degrees not radians?
        rect.RotateAround(transform.position, new Vector3(0, 0, 1), angle);
        rect.anchoredPosition = new Vector3(x, y, 0);
    }

    private void DetectMousePosition()
    {
        Vector3 dir = new Vector3(Screen.width/2,Screen.height/2,0)- Input.mousePosition;

        var rot = Quaternion.FromToRotation(Vector3.up, dir);
        var euler = rot.eulerAngles;

        var part = 360 / buttons.Count-1;
       int index = Mathf.RoundToInt(euler.z * 1 / part);
        if (index == buttons.Count)
            index = 0;
      //  var partTest = euler.z / 360;
       // int index = Mathf.RoundToInt((buttons.Count-1) * partTest);

        for (int i = 0; i < buttons.Count; i++)
        {
            if (i != index)
                buttons[i].SetTargetScaleToClose();
            else
                buttons[i].SetTargetScaleToOpen();
        }
    }
}
