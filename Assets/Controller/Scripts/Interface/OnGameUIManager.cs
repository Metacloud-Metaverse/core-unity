using Network;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnGameUIManager : MonoBehaviour
{
    public static OnGameUIManager Instance;

    [Header("Root Object")]
    public GameObject mainMenuOnGame;

    [Header("General Rects")]
    public RectTransform topRect;
    public RectTransform bottomRect;

    [Header("Circular Menu Actions")]
    public CircularMenu actionsMenu;

    [Header("Settings")]
    public GameObject panelSettings;
    public MoveUIPanelTarget leftPanel;
    public MoveUIPanelTarget rightPanelAudio;
    public MoveUIPanelTarget rightPanelGraphics;
    public MoveUIPanelTarget rightPanelGeneral;

    [Header("Map")]
    public GameObject panelMap;
    public MapMenu map;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void SetActionsMenuActive(bool value)
    {
        actionsMenu.OpenPanel(!actionsMenu.rootPanel.activeInHierarchy);
    }
    public void ToggleMainMenuOnGame()
    {
        var value = !mainMenuOnGame.activeInHierarchy;
        mainMenuOnGame.gameObject.SetActive(value);
        if (value == true)
        {
            OnButtonOpenSettings();
            if (value == true)
            {
                Cursor.lockState = CursorLockMode.Confined;
                Cursor.visible = true;
            }
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
    public void OnButtonOpenSettings()
    {
        bottomRect.gameObject.SetActive(true);
        panelMap.SetActive(false);
        //  panelSettings.SetActive(true);
        leftPanel.Open();
        OnButton_InSettings_Audio();
    }
    public void OnButtonOpenMap()
    {
     //   mainMenuOnGame.SetActive(false);
        bottomRect.gameObject.SetActive(false);
        panelMap.SetActive(true);
       // leftPanel.Close();
        rightPanelAudio.Close();
     //   mainMenuOnGame.SetActive(true);
    }

    public void OnButton_InSettings_Audio()
    {
        rightPanelAudio.Open();
        rightPanelGraphics.Close();
        rightPanelGeneral.Close();
    }
    public void OnButton_InSettings_Graphics()
    {
        rightPanelAudio.Close();
        rightPanelGraphics.Open();
        rightPanelGeneral.Close();
    }
    public void OnButton_InSettings_General()
    {
        rightPanelAudio.Close();
        rightPanelGraphics.Close();
        rightPanelGeneral.Open();
    }
}
