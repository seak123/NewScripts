using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextCtrl : MonoBehaviour {

    public string text;

    private void Start()
    {
        string content = StrUtil.GetText(text);
        Text comp = gameObject.GetComponent<Text>();
        if (comp != null) comp.text = content;
    }
}
