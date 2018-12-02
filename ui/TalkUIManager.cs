using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class TalkUIManager : MonoBehaviour {

    public GameObject character;
    public GameObject message;
    public Image image;
    public Text text;
    public Image chaImage;

    private bool startAnim;
    private float delta;

    private float shakeDelta;
    private int shakeNum;
    private int numDelta;
	// Use this for initialization
	void Start () {
      
	}

    public void StartAnim(){
        character.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        message.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        delta = -1;
        shakeDelta = 0;
        shakeNum = 0;
        numDelta = 0;
        startAnim = true;
        gameObject.SetActive(true);
    }
	
	// Update is called once per frame
	void Update () {
        if(startAnim){
            delta += Time.deltaTime;
            if (delta > 0 && delta < 0.4)
            {
                message.SetActive(true);
                character.SetActive(true);

                character.transform.DOScale(1f, 0.3f);
                message.transform.DOScale(1f, 0.3f);
            }
            if(delta>0.4f&&(delta-Time.deltaTime)<0.4f){
                character.transform.DOScale(Vector3.one * 0.9f, 0.1f).SetLoops(3, LoopType.Yoyo);
                message.transform.DOScale(Vector3.one * 0.9f, 0.1f).SetLoops(3, LoopType.Yoyo);
            }
           
            if(delta>=2.5&&delta<3.5){
                
                SetAlpha((3-delta)*2);
            }else if(delta>=4){
                gameObject.SetActive(false);
            }
        }

	}

    private void Shake(int num){
        shakeNum = num;
        shakeDelta += Time.deltaTime;
        character.transform.DOScale(1.1f, 0.05f);
        message.transform.DOScale(1.1f, 0.05f);
        if(shakeDelta>=0.06f){
            character.transform.DOScale(1f, 0.05f);
            message.transform.DOScale(1f, 0.05f);
        }
    }

    private void SetAlpha(float alpha){
        image.color = new Color(image.color.r, image.color.g, image.color.b, alpha);
        text.color = new Color(text.color.r, text.color.g, text.color.b, alpha);
        chaImage.color = new Color(chaImage.color.r, chaImage.color.g, chaImage.color.b, alpha);
    }
}
