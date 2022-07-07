using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class JackpotSymbolUI : MonoBehaviour
{
    public JackpoColumn column;
    public RectTransform posUp;
    public RectTransform posDown;
    public float speed = 500;
    public float heightMove = 5;
    public float timeMove = 1;
    public bool isRollActive = true;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!isRollActive)
            {
                isRollActive = true;
                StartCoroutine(Move());
            }
            else
            {
                isRollActive = false;
                StartCoroutine(Stop());
            }
        }       
    }
    private IEnumerator Stop()
    {
        var listUI = column.symbolsUI.OrderByDescending(x => x.transform.position.y).ToList();

        if (transform.position.y <= column.lastPos.rect.yMin)
        {
            Debug.Log("ASD");
            transform.position = posUp.position;
            yield break;
        }


        var intdex =-1;
        for (int i = 0; i < listUI.Count; i++)
        {
            if (listUI[i] == this)
            {
                intdex = i;
                break;
            }
        }

        var targetPos = column.initPositions[intdex];
        while (transform.position != targetPos)
        {
            if(targetPos == column.initPositions[0])
            {
                transform.position = targetPos;
                Debug.Log("ASDASD : "+gameObject.name);
            }
            else
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);
            }
            yield return null;
        }
    }
    private IEnumerator Move()
    {
        while (isRollActive)
        {

            if (transform.position.y <= posDown.position.y)
            {
                transform.position = posUp.position;
            }

            var nextMovement = transform.position + Vector3.down* heightMove;

            transform.position = Vector3.Lerp(transform.position, nextMovement, speed * Time.deltaTime);



            yield return null;
        }
    }
}
