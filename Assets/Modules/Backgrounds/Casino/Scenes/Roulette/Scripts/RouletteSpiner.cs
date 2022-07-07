using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
//TODO
/*
 * HACER UNA LISTA CON RANGOS DE SLOW DOWN
 * HACER QUE EL SONIDO DE LA PELOTITA PICANDO QUEDE MAS SUAVIZADO... como nose.
 * 
 * 
 * */
public class RouletteSpiner : MonoBehaviour
{
    public AudioSource aSource;
    public AudioClip wheelClip;

    [Header("Ball")]
    public BallRoulette ball;

    public RouletteSpinerRotationHandler rotationHandler;
    public bool isActive;
  
    [Header("Components")]
    [SerializeField] private Image img;
    [SerializeField] private GameObject imgMetaCloudIcon;
    [SerializeField] private Image imgBaseTextColorFeedback;
    [SerializeField] private TextMeshProUGUI textNumber;
    [SerializeField] private Transform targetRouletteOnSpin;
    [SerializeField] private Transform targetTableOnSpin;
    [SerializeField] private GameObject table;
    [SerializeField] private GameObject roulette;

    [SerializeField] private Transform targetRouletteOnSleep;
    [SerializeField] private Transform targetTableOnSleep;

    private int currentNumber;
    public float timeAnim = 0.3f;
    public iTween.EaseType easeAnim = iTween.EaseType.linear;
    public iTween.EaseType easeAnimSize = iTween.EaseType.linear;

    private Dictionary<int, int> circleRouletteValues;

    public Roulette rouletteApp;
    public Color colorOff;
    public Color colorOn;

    public int resultNumber;
    private float timeToChangeColorBeforeDelayCounter;

    private float _targetSpeed = 0;

    public RouletteSpiner(bool isActive)
    {
        this.isActive = isActive;
    }

    private void Awake()
    {
        ColorsDict.Init();
        CreateCircleRouletteDict();
    }
    private void CreateCircleRouletteDict()
    {

        circleRouletteValues = new Dictionary<int, int>()
        {
        {0,0 },
        {1,26},
        {2,3},
        {3,35},
        {4,12},
        {5,28},
        {6,7},
        {7,29},
        {8,18},
        {9,22},
        {10,9},
        {11,31},
        {12,14},
        {13,20},
        {14,1},
        {15,33},
        {16,19},
        {17,24},
        {18,5},
        {19,10},
        {20,32},
        {21,8},
        {22,30},
        {23,11},
        {24,36},
        {25,13},
        {26,27},
        {27,6},
        {28,34},
        {29,17},
        {30,25},
        {31,2},
        {32,21},
        {33,4},
        {34,19},
        {35,15},
        {36,32},
    };

    }

    public void ApplyRotation(int resultNumber)
    {
        this.resultNumber = resultNumber;
        var numberPosition = 0;
        foreach (var item in circleRouletteValues)
        {
            if (item.Value == resultNumber)
                numberPosition = item.Key;
        }
        img.enabled = false;
        isActive = true;
        MoveTableAndSpinerToSpinPosition();
        rotationHandler.RotateSpinner(this, OnRotationEnd, numberPosition);
        aSource.PlayOneShot(wheelClip);
        rouletteApp.isManualErasing = false;
        imgMetaCloudIcon.SetActive(false);
        timeToChangeColorBeforeDelayCounter = 0;
    }

    private void OnRotationEnd()
    {
       rouletteApp.audioResult.AsampleAudio(resultNumber, ColorsDict.GetStringColor(currentNumber),
            rouletteApp.almosOneChipWon);
      //  SoundControllerRoulette.Instance.PlayClip();
        aSource.Stop();
        img.enabled = true;
        ball.MoveBallToCenter();
        MoveTableAndSpinerToInitPosition();
        rouletteApp.OnGetNumberFromSpinner(currentNumber);
        StartCoroutine(EffectLightAndReset());
    }
    
    private void MoveTableAndSpinerToSpinPosition()
    {
        var sizeHash = iTween.Hash("x", 1.07f, "y", 1.07f, "easeType", easeAnimSize, "time", timeAnim, "isLocal", true);
        var spinTargetRouletteHash = iTween.Hash("position", targetRouletteOnSpin.position, "time", timeAnim, "easeType", easeAnim, "isLocal", false);
        var spinTargetTableHash = iTween.Hash("position", targetTableOnSpin.position, "time", timeAnim, "easeType", easeAnim, "isLocal", false);
        ApplyAnimEffect(sizeHash, spinTargetRouletteHash, spinTargetTableHash);
    }
    private void MoveTableAndSpinerToInitPosition()
    {
        var sizeHashOriginal = iTween.Hash("delay",1,"x", 1f, "y", 1f, "easeType", easeAnimSize, "time", timeAnim / 2, "isLocal", true);
        var spinTargetRouletteHashInit = iTween.Hash("delay",1,"position", targetRouletteOnSleep.position, "time", timeAnim / 2, "easeType", easeAnim, "isLocal", false);
        var spinTargetTableHashInit = iTween.Hash("delay",1,"position", targetTableOnSleep.position, "time", timeAnim / 2, "easeType", easeAnim, "isLocal", false);
        ApplyAnimEffect(sizeHashOriginal, spinTargetRouletteHashInit, spinTargetTableHashInit);
    }
    
    private void Update()
    {
        HandleCurrentNumber();
    }
    private void HandleCurrentNumber()
    {
        var rotateAngle = transform.localEulerAngles.z;
        // Convert negative angle to positive angle
        rotateAngle = rotateAngle < 0 ? rotateAngle + 360 : rotateAngle;

        var numberFloat = rotateAngle / rotationHandler.anglePerNumber;
        var numberInt = Mathf.RoundToInt(numberFloat);
        if (numberInt == 37)
        {
            numberInt = 0;
        }
        currentNumber = circleRouletteValues[numberInt];
        HandleTextCurrentNumber();
    }


  

    private void HandleTextCurrentNumber()
    {
        imgBaseTextColorFeedback.color = ColorsDict.GetColor(currentNumber);
        textNumber.text = currentNumber.ToString();
        
    }
    

    private void ApplyAnimEffect(Hashtable sizeHash, Hashtable spinTargetRoulette, Hashtable spinTargetTable)
    {
        iTween.MoveTo(roulette, spinTargetRoulette);
        iTween.ScaleTo(roulette, sizeHash);
        iTween.MoveTo(table, spinTargetTable);
    }

    private IEnumerator EffectLightAndReset()
    {
        for (int i = 0; i < 5; i++)
        {
            img.color = colorOff;
            yield return Yielders.Seconds(0.2f);
            img.color = colorOn;
            yield return Yielders.Seconds(0.2f);
        }
        yield return Yielders.Seconds(1.5f);
        img.enabled = false;
        imgMetaCloudIcon.SetActive(true); 
        transform.eulerAngles = Vector3.zero;
        textNumber.text = "";
        currentNumber = 0;
        ball.ResetPosition();
    }

}
