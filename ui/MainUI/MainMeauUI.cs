using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public interface ISceneUI{
    void OnEnter();
}

public class MainMeauUI : MonoBehaviour,ISceneUI {

    public GameObject Back1;
    public GameObject Back2;
    public GameObject Back0;

    public Text text1;
    public Text text2;
    public Text text3;
    public Text text4;

    public GameObject Back;
    public GameObject[] Images;
    public Text GameName;

    private float animOffset;
    private Vector3 animOrinial;
    private float cloudOffset;
    private float animSpeed;

    private float screenOffset;
    private float cloudFactor;

    private float textalpha;
    private float alphaSpeed;

    private bool isInited = false;

    public void OnEnter()
    {
        Back.GetComponent<Image>().DOColor(new Color(0.45f, 0.43f, 0.45f,1), 0.5f);
        foreach(var obj in Images){
            obj.GetComponent<Image>().DOColor(new Color(1, 1, 1, 1), 0.5f);
        }
        text1.DOColor(new Color(0.76f, 0.96f, 0.96f, 1), 0.5f);
        text2.DOColor(new Color(0.76f, 0.96f, 0.96f, 1), 0.5f);
        text3.DOColor(new Color(0.76f, 0.96f, 0.96f, 1), 0.5f);
        text4.DOColor(new Color(0.76f, 0.96f, 0.96f, 1), 0.5f);
        GameName.DOColor(new Color(1, 0.85f, 0.07f, 1), 0.6f).onComplete += ()=>{
            isInited = true;
        };
        Vector3 pos = GameName.GetComponent<RectTransform>().position;
        GameName.GetComponent<RectTransform>().DOMoveY(pos.y + Screen.height / 6, 0.5f);
    }

    public void Start()
    {
        isInited = false;
        screenOffset = (float)Screen.height/(float)Screen.width / (1334f/750f);
        cloudFactor = (float)Screen.width / 750f;
        animOffset = 0f;
        animSpeed = 2f;
        textalpha = 1f;
        alphaSpeed = 0.8f;
        animOrinial = Back2.GetComponent<RectTransform>().position;
        cloudOffset = -916f*screenOffset*cloudFactor/2;
        Back1.transform.localScale = Vector3.one * screenOffset;
        Back2.transform.localScale = Vector3.one * screenOffset;
        Back0.transform.localScale = Vector3.one * screenOffset;
        Back2.GetComponent<RectTransform>().position = animOrinial + new Vector3(animOffset, 0, 0);
        Vector3 pos = Back0.GetComponent<RectTransform>().position;
        Back0.GetComponent<RectTransform>().position = new Vector3(cloudOffset, pos.y, pos.z);

        foreach (var obj in Images)
        {
            obj.GetComponent<Image>().color = new Color(1,1,1,0);
        }
        Back.GetComponent<Image>().color = new Color(0.45f, 0.43f, 0.45f, 0);
        text1.color = new Color(0.76f, 0.96f, 0.96f, 0);
        text2.color = new Color(0.76f, 0.96f, 0.96f, 0);
        text3.color = new Color(0.76f, 0.96f, 0.96f, 0);
        text4.color = new Color(0.76f, 0.96f, 0.96f, 0);
        GameName.color = new Color(1, 0.85f, 0.07f, 0);

    }

    public void Update()
    {
        if (isInited)
        {
            if (animOffset <= -80 * screenOffset * cloudFactor)
            {
                animSpeed = 2f;
            }
            else if (animOffset >= 80 * screenOffset * cloudFactor)
            {
                animSpeed = -2f;
            }
            if (textalpha <= 0.3f)
            {
                alphaSpeed = 0.8f;
            }
            else if (textalpha >= 1.8f) alphaSpeed = -0.8f;
            cloudOffset += Time.deltaTime * 5 * screenOffset * cloudFactor;
            textalpha += alphaSpeed * Time.deltaTime;
            if (cloudOffset > 916f * screenOffset * cloudFactor / 2) cloudOffset = -916f * screenOffset * cloudFactor / 2;
            animOffset += animSpeed * Time.deltaTime;
            Vector3 pos = Back0.GetComponent<RectTransform>().position;
            Back0.GetComponent<RectTransform>().position = new Vector3(cloudOffset, pos.y, pos.z);
            Vector3 animPos = Back2.GetComponent<RectTransform>().position;
            Back2.GetComponent<RectTransform>().position = animOrinial + new Vector3(animOffset, 0, 0);

            text1.color = new Color(text1.color.r, text1.color.g, text1.color.b, textalpha);
            text2.color = new Color(text2.color.r, text2.color.g, text2.color.b, textalpha);
            text3.color = new Color(text3.color.r, text3.color.g, text3.color.b, textalpha);
            text4.color = new Color(text3.color.r, text3.color.g, text3.color.b, textalpha);
        }
    }

    public void NewGame()
    {
        GameRoot.GetInstance().StartNewGame();
    }

    public void LoadGame(){
        GameRoot.GetInstance().LoadGame();
    }
}
