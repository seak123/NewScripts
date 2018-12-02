using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class HightLightShake : MonoBehaviour {

    private float alpha=0;
    private float delta = 0;
    private Image image;

    private void Start()
    {
        image = GetComponent<Image>();
        Reset();
    }
    // Update is called once per frame
    void Update () {
        if(alpha<=0.3f){
            delta = 0.5f;
        }
        if(alpha>=0.8f){
            delta = -0.5f;
        }
        alpha = alpha + delta * Time.deltaTime;
        image.color = new Color(image.color.r, image.color.g, image.color.b, alpha);
	}

    public void Reset()
    {
        alpha = 0;
    }
}
