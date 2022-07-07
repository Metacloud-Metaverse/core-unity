using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RouletteUIMSGText : MonoBehaviour
{
    public List<InternalTextMSG> textsCounter = new List<InternalTextMSG>();
    public List<TMPro.TextMeshProUGUI> textsUI = new List<TMPro.TextMeshProUGUI>();


    public void Update()
    {
        CheckClean();
    }
    public void SetText(string text)
    {
        for (int i = 0; i < textsUI.Count; i++)
        {
            if (textsUI[i].gameObject.activeInHierarchy == false)
            {
                textsCounter.Add(new InternalTextMSG(i));
                textsUI[i].text = text;
                textsUI[i].gameObject.SetActive(true);
                break;
            }
        }
    }
    private void CheckClean()
    {
        for (int i = 0; i < textsCounter.Count; i++)
        {
            textsCounter[i].counter += Time.deltaTime;
            if (textsCounter[i].counter >= 3)
            {
                textsUI[textsCounter[i].indexTextUI].gameObject.SetActive(false);
                textsCounter.RemoveAt(i);
            }
        }
    }
    public class InternalTextMSG
    {
        public float counter;
        public int indexTextUI;

        public InternalTextMSG(int index)
        {
            counter = 0;
            indexTextUI = index;
        }
    }
}
