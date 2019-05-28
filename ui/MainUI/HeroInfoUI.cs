using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class HeroInfoUI : MonoBehaviour, ISceneUI
{

    public Text proficiency;
    public Text talent;
    public Image heroSprite;
    public Text heroName;
    public ScrollRect skillPanel;
    public GameObject skillInfoPrefab;

    private HeroData heroData;
    private int exp;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Next()
    {
        DOTween.Clear();
        GameRoot.GetInstance().mainUIMng.OpenUI(9);
    }

    public void Back()
    {
        DOTween.Clear();
        GameRoot.GetInstance().StartNewGame();
    }

    public void OnEnter()
    {
        heroData = GameRoot.GetInstance().BattleField.assetManager.GetHeroData(GameRoot.GetInstance().gameDataManager.boss.id);
        heroName.text = StrUtil.GetText(heroData.CreatureName);
        exp = GameRoot.GetInstance().gameDataManager.bossExp[heroData.id - 1001];
        proficiency.text = StrUtil.GetText("熟练度:") + exp;

        heroSprite.sprite = GameRoot.GetInstance().BattleField.assetManager.GetCards(heroData.icon);

        skillPanel.content.sizeDelta = new Vector2(0, 160 * heroData.talents.Length + 20);

        int unlockNum = 0;
        for (int i = 0; i < heroData.talents.Length;++i){
            SkillData skillData = GameRoot.GetInstance().BattleField.assetManager.GetSkillData(heroData.talents[i]);
            GameObject obj = Instantiate(skillInfoPrefab);
            obj.transform.parent = skillPanel.content.transform;
            obj.transform.localScale = Vector3.one;
            obj.transform.localPosition = new Vector3(325, -100 - i * 160, 0);
            obj.GetComponent<SkillInfoPanel>().InjectData(skillData,heroData);
            if (skillData.needExp <= exp)
            {
                ++unlockNum;
                obj.GetComponent<SkillInfoPanel>().Lock(false);
            }else{
                obj.GetComponent<SkillInfoPanel>().Lock(true);
            }
        }

        talent.text = StrUtil.GetText("天赋") + " "+unlockNum+"/"+heroData.talents.Length;
    }
}
