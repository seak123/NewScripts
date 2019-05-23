using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class HeroSelectUI : MonoBehaviour,ISceneUI {

    public GameObject quitBtn;
    public GameObject nextBtn;
    public GameObject heroPanel;
    public GameObject skillPanel;
    public GameObject introducePanel;


    //heropanel
    public GameObject heroIconPrefab;
    public ScrollRect heroScroll;
    //skillpanel
    public SkillIconUI skill1;
    public SkillIconUI skill2;
    public SkillIconUI skill3;
    public SkillIconUI skill4;
    //introducepanel
    public Text bossName;
    public Text bossProfi;

    private List<HeroIconUI> heroScripts;
    private int currBossId;

    public void OnEnter()
    {
        GameRoot.GetInstance().CameraMng.PlaySelectHero();
        quitBtn.GetComponent<RectTransform>().position = new Vector3(-Screen.width * 2 / 5, quitBtn.GetComponent<RectTransform>().position.y, 0);
        quitBtn.GetComponent<RectTransform>().DOMoveX(0, 0.5f);
        nextBtn.GetComponent<RectTransform>().position = new Vector3(Screen.width * 7 / 5, quitBtn.GetComponent<RectTransform>().position.y, 0);
        nextBtn.GetComponent<RectTransform>().DOMoveX(Screen.width, 0.5f);
        introducePanel.GetComponent<RectTransform>().position = new Vector3(-Screen.width / 2, introducePanel.GetComponent<RectTransform>().position.y, 0);
        introducePanel.GetComponent<RectTransform>().DOMoveX(0, 0.5f).SetDelay(2f);
        skillPanel.GetComponent<RectTransform>().position = new Vector3(Screen.width *3/ 2, introducePanel.GetComponent<RectTransform>().position.y, 0);
        skillPanel.GetComponent<RectTransform>().DOMoveX(Screen.width, 0.5f).SetDelay(2f);
        heroPanel.GetComponent<RectTransform>().position = new Vector3(Screen.width/2, -Screen.height/2, 0);
        heroPanel.GetComponent<RectTransform>().DOMoveY(0, 0.5f).SetDelay(2f).onComplete +=()=>{
            GameRoot.GetInstance().mainUIMng.PushMessage(StrUtil.GetText("选择你的魔王"),SystemTipType.Warning);
        };

        InitData();
    }

    public void InitData()
    {
        heroScripts = new List<HeroIconUI>();
        HeroData[] datas = GameRoot.GetInstance().BattleField.assetManager.heroDatas;
        heroScroll.content.sizeDelta = new Vector2((datas.Length-1)*190-500, 210);
        for (int i = 0; i < datas.Length; ++i)
        {
            GameObject entity = Instantiate(heroIconPrefab);
            entity.GetComponent<ClickEvent>().clickActionObj += ClickHero;
            entity.transform.parent = heroScroll.content.gameObject.transform;
            entity.transform.localScale = Vector3.one;
            entity.GetComponent<RectTransform>().localPosition = new Vector2(i*190+90, -120);
            heroScripts.Add(entity.GetComponent<HeroIconUI>());
            entity.GetComponent<HeroIconUI>().InjectData(datas[i]);
        }
        ClickHero(heroScripts[0].gameObject);
    }

    public void ClickHero(GameObject obj){
        foreach(var s in heroScripts){
            s.SetSelect(false);
        }
        obj.GetComponent<HeroIconUI>().SetSelect(true);
        RefreshView(obj.GetComponent<HeroIconUI>().heroData.id);
    }

    private void RefreshView(int key){

        currBossId = key;
        HeroData data = GameRoot.GetInstance().BattleField.assetManager.GetHeroData(key);
        bossName.text = StrUtil.GetText(data.CreatureName);
        bossProfi.text = StrUtil.GetText("熟练度:")+ GameRoot.GetInstance().gameDataManager.bossExp[data.id - 1001].ToString();
        skill1.InjectData(GameRoot.GetInstance().BattleField.assetManager.GetSkillData(data.skills[0]));
        skill2.InjectData(GameRoot.GetInstance().BattleField.assetManager.GetSkillData(data.skills[1]));
        skill3.InjectData(GameRoot.GetInstance().BattleField.assetManager.GetSkillData(data.skills[2]));
        skill4.InjectData(GameRoot.GetInstance().BattleField.assetManager.GetSkillData(data.skills[3]));
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Next(){
        DOTween.Clear();
        GameRoot.GetInstance().CameraMng.StopSelectHero();
        GameRoot.GetInstance().mainUIMng.CloseScene();
        GameRoot.GetInstance().QuitSelectHero();
        GameRoot.GetInstance().mainUIMng.OpenUI(9);
    }

    public void Back(){
        DOTween.Clear();
        GameRoot.GetInstance().CameraMng.StopSelectHero();
        GameRoot.GetInstance().mainUIMng.CloseScene();
        GameRoot.GetInstance().QuitSelectHero();
        GameRoot.GetInstance().mainUIMng.OpenUI(0);
    }
}
