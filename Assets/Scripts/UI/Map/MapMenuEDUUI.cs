using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine;

public class MapMenuEDUUI : MonoBehaviour
{
    public List<float> zoomLevels;
    private int currentZoom;
    private Vector2 mouseStartPos;
    private bool mouseMoved;

    public Image mapImage;
    public TextMeshProUGUI markerText;
 //   public CanvasScaler canvasScaler;
    private RectTransform rectTransform;
    public RectTransform buildingsRectTransform;
    public MapParcelInfo mapParcelInfo;

    private Vector2Int currentMouseCoordinates;

    public Image parcelMarker;

    private void Awake()
    {
     //   canvasScaler = GetComponent<CanvasScaler>();
        rectTransform = GetComponent<RectTransform>();
    }

    void Update()
    {
        //manual stretch of the map image
        var fixedH = Screen.height - OnGameUIManager.Instance.topRect.rect.height;

        mapImage.rectTransform.sizeDelta = new Vector2(Screen.width, fixedH);     
       // buildingsRectTransform.sizeDelta = new Vector2(Screen.width, fixedH);

        //percentage of the mouse position based on screen size
        markerText.transform.position = Input.mousePosition;
        var mouseScreenPercentage = new Vector3(Input.mousePosition.x / Screen.width, Input.mousePosition.y / fixedH);

        //find mouse position without offset
        var xPos = Mathf.FloorToInt(mouseScreenPercentage.x * (ParcelManager.Instance.Xmax / zoomLevels[currentZoom]));
        var yPos = Mathf.FloorToInt(mouseScreenPercentage.y * (ParcelManager.Instance.Ymax / zoomLevels[currentZoom]));

        //offset based in zoom
        var mapTotalWidth = (Screen.width + Screen.width * (zoomLevels[currentZoom] - 1));
        var mapPosx = mapImage.rectTransform.localPosition.x;
        var visibleMapWidth = mapImage.rectTransform.sizeDelta.x / 2;
        var mapOffsetx = mapTotalWidth / 2 - mapPosx - visibleMapWidth;
        xPos += Mathf.FloorToInt(mapOffsetx * (ParcelManager.Instance.Xmax / mapTotalWidth));

        var mapTotalHeight = (fixedH + fixedH * (zoomLevels[currentZoom] - 1));
        var mapPosy = mapImage.rectTransform.localPosition.y;
        var visibleMapHeight = mapImage.rectTransform.sizeDelta.y / 2;
        var mapOffset = mapTotalHeight / 2 - mapPosy - visibleMapHeight;
        yPos += Mathf.FloorToInt(mapOffset * (ParcelManager.Instance.Ymax / mapTotalHeight));

        //offset based on center of the map being 0,0
        xPos -= ParcelManager.Instance.Xmax / 2;
        yPos -= ParcelManager.Instance.Ymax / 2;
        currentMouseCoordinates = new Vector2Int(xPos, yPos);
        markerText.text = $"({currentMouseCoordinates.x}, {currentMouseCoordinates.y})";

        mapParcelInfo.ShowParcelInformation();


        MapControls();
    }

    private void MapControls()
    {
        //zooming
        var mouseDir = Input.mouseScrollDelta.y;
        if (mouseDir > 0 && currentZoom < zoomLevels.Count - 1)
        {
            currentZoom++;
            SetNewZoom();
        }
        else if (mouseDir < 0 && currentZoom > 0)
        {
            currentZoom--;
            SetNewZoom();
        }

        if (Input.GetMouseButtonUp(0) && mouseMoved == false)
        {
            mapParcelInfo.ToggleDisplay(currentMouseCoordinates);
        }
        //moving map
        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(2))
        {
            mouseStartPos = Input.mousePosition;
            mouseMoved = false;
        }
        if (Input.GetMouseButton(0) || Input.GetMouseButton(1) || Input.GetMouseButton(2))
        {
            var mouseDelta = new Vector2(Input.mousePosition.x - mouseStartPos.x, Input.mousePosition.y - mouseStartPos.y);
            var mapPos = mapImage.transform.localPosition;
            SetMapPosition(new Vector3(mapPos.x + mouseDelta.x, mapPos.y + mouseDelta.y));
            if (mouseStartPos.x != Input.mousePosition.x || mouseStartPos.y != Input.mousePosition.y)
            {
                mouseMoved = true;
            }
            mouseStartPos = Input.mousePosition;
        }
    }

    public void TeleportButton()
    {
        var size = ParcelManager.Instance.parcelSize;
        RequestWorldTeleportMessage.Send(new Vector3(currentMouseCoordinates.x * size, 0, currentMouseCoordinates.y * size));
    }

    private void SetNewZoom()
    {
        mapImage.transform.localScale = new Vector3(zoomLevels[currentZoom], zoomLevels[currentZoom], 1);
        parcelMarker.transform.localScale = new Vector3(zoomLevels[currentZoom], zoomLevels[currentZoom], 1);
        SetMapPosition(mapImage.transform.localPosition);
    }


    /// <summary>
    /// Moves the map, doesn't allow the map to go out of bounds
    /// </summary>
    private void SetMapPosition(Vector2 newPos)
    {
        var maxHeight = (zoomLevels[currentZoom] - 1) * rectTransform.position.x;
        var maxWidth = (zoomLevels[currentZoom] - 1) * rectTransform.position.y;
        if (currentZoom == 0)
        {
            maxHeight = 0;
            maxWidth = 0;
        }

        newPos.x = Mathf.Min(newPos.x, maxHeight);
        newPos.x = Mathf.Max(newPos.x, maxHeight * -1);

        newPos.y = Mathf.Min(newPos.y, maxWidth);
        newPos.y = Mathf.Max(newPos.y, maxWidth * -1);

        mapImage.transform.localPosition = newPos;
    }
}
