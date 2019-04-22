using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public enum MapType{
    DailyMap = 1,
    Castle = 2,
    Monster = 3,
    Preview = 4
}

public class MapUIManager : MonoBehaviour,ISceneUI {

    public GameObject dailyBtn;
    public GameObject dailyBack;
    public GameObject dailyIcon;
    public GameObject dailyName;
    public GameObject dailyArrow1;
    public GameObject castleBtn;
    public GameObject castleBack;
    public GameObject castleIcon;
    public GameObject castleName;
    public GameObject castleArrow1;
    public GameObject castleArrow2;
    public GameObject monsterBtn;
    public GameObject monsterBack;
    public GameObject monsterIcon;
    public GameObject monsterName;
    public GameObject monsterArrow1;
    public GameObject monsterArrow2;
    public GameObject previewBtn;
    public GameObject previewBack;
    public GameObject previewIcon;
    public GameObject previewName;
    public GameObject previewArrow1;

    private MapType currMapType;

    private bool isAnim = false;

    private float sizeOffset;

	// Use this for initialization
	void Start () {

        sizeOffset = Screen.width / 750f;
        Init();
	}

    private void Init()
    {
        currMapType = MapType.Monster;
        isAnim = false;
        SwitchToDaily();

    }

    public void OnEnter(){

    }

    // Update is called once per frame
    void Update () {
		
	}

    public void EnterDailyPlan(){
        GameRoot.GetInstance().mainUIMng.OpenUI(15);
    }

    public void EnterStrategy(){
        GameRoot.GetInstance().StartStrategy();
    }

    public void SwitchToDaily(){
        if (currMapType == MapType.DailyMap) return;
        if (isAnim) return;
        isAnim = true;
        currMapType = MapType.DailyMap;
        RefreshView();
    }

    public void SwitchToCastle(){
        if (currMapType == MapType.Castle) return;
        if (isAnim) return;
        isAnim = true;
        currMapType = MapType.Castle;
        RefreshView();
    }

    public void SwitchToMonster()
    {
        if (currMapType == MapType.Monster) return;
        if (isAnim) return;
        isAnim = true;
        currMapType = MapType.Monster;
        RefreshView();
    }

    public void SwitchToPreview()
    {
        if (currMapType == MapType.Preview) return;
        if (isAnim) return;
        isAnim = true;
        currMapType = MapType.Preview;
        RefreshView();
    }

    public void RefreshView(){
        switch(currMapType){
            case MapType.DailyMap:
                dailyBack.GetComponent<Image>().DOColor(new Color(0.68f, 0.86f, 0.86f),0.2f).onComplete += ()=>{

                    dailyName.SetActive(true);
                    dailyArrow1.SetActive(true);
                    dailyArrow1.transform.DOScale(Vector3.one * 1.5f, 0.1f).onComplete += () =>
                    {
                        dailyArrow1.transform.DOScale(Vector3.one * 1f, 0.1f).onComplete += () =>
                        {
                            isAnim = false;
                        };
                    };
                };
                dailyBack.transform.DOScale(new Vector3(1.68f, 1, 1), 0.2f);
                dailyBtn.GetComponent<RectTransform>().DOMoveX(135*sizeOffset, 0.2f);
                dailyIcon.transform.DOScale(Vector3.one * 1.5f, 0.2f).onComplete +=()=>{
                    dailyIcon.transform.DOScale(Vector3.one * 1.3f, 0.1f);
                };
                dailyIcon.GetComponent<RectTransform>().DOLocalMoveY(60, 0.2f);

                castleBack.GetComponent<Image>().DOColor(new Color(0.4f, 0.5f, 0.5f), 0.2f);
                castleBack.transform.DOScale(new Vector3(1, 1, 1), 0.2f);
                castleBtn.GetComponent<RectTransform>().DOMoveX(350 * sizeOffset, 0.2f);
                castleIcon.transform.DOScale(Vector3.one, 0.2f);
                castleIcon.GetComponent<RectTransform>().DOLocalMoveY(30, 0.2f);
                castleName.SetActive(false);
                castleArrow1.SetActive(false);
                castleArrow2.SetActive(false);

                monsterBack.GetComponent<Image>().DOColor(new Color(0.4f, 0.5f, 0.5f), 0.2f);
                monsterBack.transform.DOScale(new Vector3(1, 1, 1), 0.2f);
                monsterBtn.GetComponent<RectTransform>().DOMoveX(510 * sizeOffset, 0.2f);
                monsterIcon.transform.DOScale(Vector3.one, 0.2f);
                monsterIcon.GetComponent<RectTransform>().DOLocalMoveY(30, 0.2f);
                monsterName.SetActive(false);
                monsterArrow1.SetActive(false);
                monsterArrow2.SetActive(false);

                previewBack.GetComponent<Image>().DOColor(new Color(0.4f, 0.5f, 0.5f), 0.2f);
                previewBack.transform.DOScale(new Vector3(1, 1, 1), 0.2f);
                previewBtn.GetComponent<RectTransform>().DOMoveX(670 * sizeOffset, 0.2f);
                previewIcon.transform.DOScale(Vector3.one, 0.2f);
                previewIcon.GetComponent<RectTransform>().DOLocalMoveY(30, 0.2f);
                previewName.SetActive(false);
                previewArrow1.SetActive(false);
                break;
            case MapType.Castle:
                dailyBack.GetComponent<Image>().DOColor(new Color(0.4f, 0.5f, 0.5f), 0.2f);
                dailyBack.transform.DOScale(Vector3.one, 0.2f);
                dailyBtn.GetComponent<RectTransform>().DOMoveX(80 * sizeOffset, 0.2f);
                dailyIcon.transform.DOScale(Vector3.one, 0.2f);
                dailyIcon.GetComponent<RectTransform>().DOLocalMoveY(30, 0.2f);
                dailyName.SetActive(false);
                dailyArrow1.SetActive(false);

                castleBack.GetComponent<Image>().DOColor(new Color(0.68f, 0.86f, 0.86f), 0.2f).onComplete += () => {

                    castleName.SetActive(true);
                    castleArrow1.SetActive(true);
                    castleArrow2.SetActive(true);
                    castleArrow1.transform.DOScale(Vector3.one * 1.5f, 0.1f).onComplete += () =>
                    {
                        castleArrow1.transform.DOScale(Vector3.one * 1f, 0.1f).onComplete += () =>
                        {
                            isAnim = false;
                        };
                    };
                    castleArrow2.transform.DOScale(Vector3.one * 1.5f, 0.1f).onComplete += () =>
                    {
                        castleArrow2.transform.DOScale(Vector3.one * 1f, 0.1f);
                    };
                };
                castleBack.transform.DOScale(new Vector3(1.68f, 1, 1), 0.2f);
                castleBtn.GetComponent<RectTransform>().DOMoveX(295 * sizeOffset, 0.2f);
                castleIcon.transform.DOScale(Vector3.one*1.5f, 0.2f).onComplete += ()=>{
                    castleIcon.transform.DOScale(Vector3.one * 1.3f, 0.1f);
                };
                castleIcon.GetComponent<RectTransform>().DOLocalMoveY(60, 0.2f);


                monsterBack.GetComponent<Image>().DOColor(new Color(0.4f, 0.5f, 0.5f), 0.2f);
                monsterBack.transform.DOScale(new Vector3(1, 1, 1), 0.2f);
                monsterBtn.GetComponent<RectTransform>().DOMoveX(510 * sizeOffset, 0.2f);
                monsterIcon.transform.DOScale(Vector3.one, 0.2f);
                monsterIcon.GetComponent<RectTransform>().DOLocalMoveY(30, 0.2f);
                monsterName.SetActive(false);
                monsterArrow1.SetActive(false);
                monsterArrow2.SetActive(false);

                previewBack.GetComponent<Image>().DOColor(new Color(0.4f, 0.5f, 0.5f), 0.2f);
                previewBack.transform.DOScale(new Vector3(1, 1, 1), 0.2f);
                previewBtn.GetComponent<RectTransform>().DOMoveX(670 * sizeOffset, 0.2f);
                previewIcon.transform.DOScale(Vector3.one, 0.2f);
                previewIcon.GetComponent<RectTransform>().DOLocalMoveY(30, 0.2f);
                previewName.SetActive(false);
                previewArrow1.SetActive(false);
                break;
            case MapType.Monster:
                dailyBack.GetComponent<Image>().DOColor(new Color(0.4f, 0.5f, 0.5f), 0.2f);
                dailyBack.transform.DOScale(Vector3.one, 0.2f);
                dailyBtn.GetComponent<RectTransform>().DOMoveX(80 * sizeOffset, 0.2f);
                dailyIcon.transform.DOScale(Vector3.one, 0.2f);
                dailyIcon.GetComponent<RectTransform>().DOLocalMoveY(30, 0.2f);
                dailyName.SetActive(false);
                dailyArrow1.SetActive(false);

                castleBack.GetComponent<Image>().DOColor(new Color(0.4f, 0.5f, 0.5f), 0.2f);
                castleBack.transform.DOScale(new Vector3(1, 1, 1), 0.2f);
                castleBtn.GetComponent<RectTransform>().DOMoveX(240 * sizeOffset, 0.2f);
                castleIcon.transform.DOScale(Vector3.one, 0.2f);
                castleIcon.GetComponent<RectTransform>().DOLocalMoveY(30, 0.2f);
                castleName.SetActive(false);
                castleArrow1.SetActive(false);
                castleArrow2.SetActive(false);

                monsterBack.GetComponent<Image>().DOColor(new Color(0.68f, 0.86f, 0.86f), 0.2f).onComplete += () => {

                    monsterName.SetActive(true);
                    monsterArrow1.SetActive(true);
                    monsterArrow2.SetActive(true);
                    monsterArrow1.transform.DOScale(Vector3.one * 1.5f, 0.1f).onComplete += () =>
                    {
                        monsterArrow1.transform.DOScale(Vector3.one * 1f, 0.1f).onComplete += () =>
                        {
                            isAnim = false;
                        };
                    };
                    monsterArrow2.transform.DOScale(Vector3.one * 1.3f, 0.1f).onComplete += () =>
                    {
                        monsterArrow2.transform.DOScale(Vector3.one * 1f, 0.1f);
                    };
                };
                monsterBack.transform.DOScale(new Vector3(1.68f, 1, 1), 0.2f);
                monsterBtn.GetComponent<RectTransform>().DOMoveX(455 * sizeOffset, 0.2f);
                monsterIcon.transform.DOScale(Vector3.one * 1.5f, 0.2f).onComplete +=()=>{
                    monsterIcon.transform.DOScale(Vector3.one * 1.3f, 0.1f);
                };
                monsterIcon.GetComponent<RectTransform>().DOLocalMoveY(60, 0.2f);

                previewBack.GetComponent<Image>().DOColor(new Color(0.4f, 0.5f, 0.5f), 0.2f);
                previewBack.transform.DOScale(new Vector3(1, 1, 1), 0.2f);
                previewBtn.GetComponent<RectTransform>().DOMoveX(670 * sizeOffset, 0.2f);
                previewIcon.transform.DOScale(Vector3.one, 0.2f);
                previewIcon.GetComponent<RectTransform>().DOLocalMoveY(30, 0.2f);
                previewName.SetActive(false);
                previewArrow1.SetActive(false);
                break;
            case MapType.Preview:
                dailyBack.GetComponent<Image>().DOColor(new Color(0.4f, 0.5f, 0.5f), 0.2f);
                dailyBack.transform.DOScale(Vector3.one, 0.2f);
                dailyBtn.GetComponent<RectTransform>().DOMoveX(80 * sizeOffset, 0.2f);
                dailyIcon.transform.DOScale(Vector3.one, 0.2f);
                dailyIcon.GetComponent<RectTransform>().DOLocalMoveY(30, 0.2f);
                dailyName.SetActive(false);
                dailyArrow1.SetActive(false);

                castleBack.GetComponent<Image>().DOColor(new Color(0.4f, 0.5f, 0.5f), 0.2f);
                castleBack.transform.DOScale(new Vector3(1, 1, 1), 0.2f);
                castleBtn.GetComponent<RectTransform>().DOMoveX(240 * sizeOffset, 0.2f);
                castleIcon.transform.DOScale(Vector3.one, 0.2f);
                castleIcon.GetComponent<RectTransform>().DOLocalMoveY(30, 0.2f);
                castleName.SetActive(false);
                castleArrow1.SetActive(false);
                castleArrow2.SetActive(false);

                monsterBack.GetComponent<Image>().DOColor(new Color(0.4f, 0.5f, 0.5f), 0.2f);
                monsterBack.transform.DOScale(new Vector3(1, 1, 1), 0.2f);
                monsterBtn.GetComponent<RectTransform>().DOMoveX(400 * sizeOffset, 0.2f);
                monsterIcon.transform.DOScale(Vector3.one, 0.2f);
                monsterIcon.GetComponent<RectTransform>().DOLocalMoveY(30, 0.2f);
                monsterName.SetActive(false);
                monsterArrow1.SetActive(false);
                monsterArrow2.SetActive(false);

                previewBack.GetComponent<Image>().DOColor(new Color(0.68f, 0.86f, 0.86f), 0.2f).onComplete += () => {

                    previewName.SetActive(true);
                    previewArrow1.SetActive(true);
                    previewArrow1.transform.DOScale(Vector3.one * 1.3f, 0.1f).onComplete += () =>
                    {
                        previewArrow1.transform.DOScale(Vector3.one * 1f, 0.1f).onComplete += () =>
                        {
                            isAnim = false;
                        };
                    };
                  
                };
                previewBack.transform.DOScale(new Vector3(1.68f, 1, 1), 0.2f);
                previewBtn.GetComponent<RectTransform>().DOMoveX(615 * sizeOffset, 0.2f);
                previewIcon.transform.DOScale(Vector3.one * 1.5f, 0.2f).onComplete +=()=>{
                    previewIcon.transform.DOScale(Vector3.one * 1.3f, 0.1f);
                };
                previewIcon.GetComponent<RectTransform>().DOLocalMoveY(60, 0.2f);
                break;
        }
    }
}
