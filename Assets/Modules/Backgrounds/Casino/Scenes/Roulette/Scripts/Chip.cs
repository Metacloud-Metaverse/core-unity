using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Chip : MonoBehaviour,IPointerDownHandler
{
    [Header("Others")]
    public UnityEngine.UI.Image image;
    public Roulette roulette;
    [Header("Sounds")]
    public AudioClip clipOnBoardArrive;
    public AudioClip clipOnBoardLeave;
    public AudioClip clipOnBoardLeaveB;

    [Header("Effects Movement")]
    public iTween.EaseType easeMove;
    public iTween.EaseType easeSize;
    private float timeMove = 0.05f;
    private float timeSize = 0.2f;
    public Vector3 sizeTargetOnBoard = new Vector3(0.35f,0.35f,0.35f);
    public RouletteCommand command { get; private set; }

    private Transform _initRoot;
    public bool isOnBoard = false;
    private void Update()
    {
        checkearseCustom();
    }
    private void checkearseCustom()
    {
        if (isOnBoard)
        {
            image.raycastTarget = roulette.isManualErasing;
        }
    }
    public void SetChip(RouletteCommand command)
    {
        isOnBoard = true;
        this.command = command;
        gameObject.SetActive(true);
    }

    public void SetPositionToBoard()
    {
        iTween.Stop(gameObject);

        if (_initRoot == null)
            _initRoot = transform.parent;
        if(command == null)
        {
            return;
        }
        if(command.inputUI == null)
        {
            if (command == null)
            {
                return;
            }
        }
        transform.SetParent(command.inputUI.transform);
        var offset = Vector3.zero;
        int leftCount = 0;
        int rightCount = 0;
        int xChangeTimes = 0;
        var xOffset = 6;
        var yOffset = 3;
        int baseY = -10;
        bool doBaseY = true;
        for (int i = 0; i < command.inputUI.transform.childCount; i++)
        {
            offset.y += yOffset;
            if (i % 5 == 0)
            {
                if (i % 2 == 0)
                {
                    rightCount++;
                    xChangeTimes++;
                    offset.x = rightCount * xOffset;
                }
                else
                {
                    leftCount++;
                    xChangeTimes++;
                    offset.x = leftCount * -xOffset;
                }
                if (xChangeTimes > 2 && (xChangeTimes) % 2 == 0 && !doBaseY)
                {
                    baseY += 10;
                    doBaseY = true;
                    rightCount = 0;
                    leftCount = 0;
                    xChangeTimes = 0;
                    offset.x = 0;
                }
                else if (xChangeTimes % 2 != 0)
                {
                    doBaseY = false;
                }
                offset.y = baseY;
            }
        }
        var targetPosition = Vector3.zero + offset;
        var hashMovement = iTween.Hash("position", targetPosition, "time", timeMove, "easeType", easeMove, "isLocal", true);
        var hashSize = iTween.Hash("x", sizeTargetOnBoard.x, "y", sizeTargetOnBoard.y, "z", sizeTargetOnBoard.z, "time", timeSize, "easeType", easeSize, "isLocal", true);
        iTween.MoveTo(gameObject, hashMovement);
        iTween.ScaleTo(gameObject, hashSize);
        Invoke("OnFinishToBoard", timeMove);
    }

    public void BackToOriginalParent()
    {
        iTween.Stop(gameObject);
        isOnBoard = false;
        transform.SetParent(_initRoot);
        SoundControllerRoulette.Instance.PlayQueue(clipOnBoardLeave,0.01f);
        var hashMovement = iTween.Hash("position", Vector3.zero, "time", timeMove, "easeType", easeMove, "isLocal", true, "oncomplete", "OnFinishBack");
        var hashSize = iTween.Hash("x", 1, "y", 1, "z", 1, "time", timeSize, "easeType", easeSize, "isLocal", true);
        iTween.MoveTo(gameObject, hashMovement);
        iTween.ScaleTo(gameObject, hashSize);
    }

    private void OnFinishBack()
    {
        gameObject.SetActive(false);
    }
    
    private void OnFinishToBoard()
    {
        SoundControllerRoulette.Instance.PlayClip(clipOnBoardArrive);
    }

    public void Earse()
    {
        roulette.ManualUndo(command);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (roulette.isManualErasing)
        {
            Earse();
        }
    }
}
