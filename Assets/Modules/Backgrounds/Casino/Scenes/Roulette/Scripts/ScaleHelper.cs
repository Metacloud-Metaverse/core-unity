using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleHelper : MonoBehaviour
{
    // Start is called before the first frame update
    public float h;
    public float w;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        int refHieght = 1080;
        int refWidght = 1980;
        //1 --- 1080
        //? ---currentScren
        h = Screen.height;
        w = Screen.width;
       // var hF = h * 1 / refHieght;
       // var wF = w * 1 / refWidght;
        GetComponent<RectTransform>().sizeDelta = new Vector2(w,h);
    }
}
