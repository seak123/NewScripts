using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tip : MonoBehaviour {

    public Text parentTxt;
    public Text content;
    public Image bound;

    public void InjectContent(string value){
        parentTxt.text = value;
        content.text = value;

    }

    private void Update()
    {
        float sizeFactor = (float)Screen.width / 750f;
        bound.gameObject.GetComponent<RectTransform>().sizeDelta = parentTxt.gameObject.GetComponent<RectTransform>().sizeDelta+new Vector2(30,30);
    }
}
