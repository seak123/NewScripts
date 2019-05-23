using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeroIconUI : MonoBehaviour {

    public Image icon;
    public GameObject back;
    public GameObject lockObj;
    public GameObject select;

    public HeroData heroData;

    private bool isSelect = false;
    private float animSpeed;
	// Use this for initialization
	void Awake () {
        isSelect = false;
        animSpeed = 100;
	}
	
	// Update is called once per frame
	void Update () {
        float screenFactor = (float)Screen.width / 750f;
        if(isSelect){
            if(select.transform.localPosition.y>100){
                animSpeed = -80*screenFactor;
            }else if(select.transform.localPosition.y<80){
                animSpeed = 80*screenFactor;
            }
            select.transform.position += new Vector3(0, animSpeed * Time.deltaTime, 0);
        }
	}

    public void InjectData(HeroData data){
        heroData = data;
        icon.sprite = GameRoot.GetInstance().BattleField.assetManager.GetIcon(data.icon);
    }

    public void SetSelect(bool flag){
        if(flag){
            back.SetActive(false);
            select.SetActive(true);
            isSelect = true;
        }else{
            back.SetActive(true);
            select.SetActive(false);
            isSelect = false;
        }
    }
}
