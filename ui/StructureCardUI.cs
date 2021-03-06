﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StructureCardUI : MonoBehaviour {

    public Text cost;
    public Text lost;
    public Text hp;
    public Text defence;
    public Text skill1Name;
    public Text skill1CD;
    public Text skill1Des;
    public Text skill2Name;
    public Text skill2CD;
    public Text skill2Des;
    public Text skill3Name;
    public Text skill3CD;
    public Text skill3Des;
    public Text englishName;

    public Image back;
    public Image skillBack;
    public Image cardIcon;
    public Image skill1Icon;
    public Image skill2Icon;
    public Image skill3Icon;
    public Image star1Icon;
    public Image star2Icon;
    public Image star3Icon;

    public GameObject cardName1;
    public Text[] Name1Texts;
    public GameObject cardName2;
    public Text[] Name2Texts;

    public GameObject skillContent;
    public GameObject skill1;
    public GameObject skill2;
    public GameObject skill3;



    public void CleanUp()
    {
        cost.text = "";
        hp.text = "";
        defence.text = "";
        skill1Name.text = "";
        skill1CD.text = "";
        skill1Des.text = "";
        skill2Name.text = "";
        skill2CD.text = "";
        skill2Des.text = "";
        skill3Name.text = "";
        skill3CD.text = "";
        skill3Des.text = "";
        foreach (var t in Name1Texts)
        {
            t.text = "";
        }
        foreach (var t in Name2Texts)
        {
            t.text = "";
        }
        skill1.SetActive(false);
        skill2.SetActive(false);
        skill3.SetActive(false);
    }

    public void InjectData(CardData cardData,bool openSkill = true)
    {

        AssetManager assetMng = GameRoot.GetInstance().BattleField.assetManager;

        CreatureData data = assetMng.GetCreatureData(cardData.unitId);

        cost.text = cardData.cost.ToString();

        //lost.text = "-"+cardData.lost.ToString();

        hp.text = data.hp.ToString();

        defence.text = data.defence.ToString();

        if (data.skills.Length >= 1)
        {
            SkillData skillData = assetMng.GetSkillData(data.skills[0]);
            if (skillData != null)
            {
                skill1.SetActive(true);
                skill1Name.text = StrUtil.GetText(skillData.skill_name);
                skill1CD.text = skillData.skill_coold > 0 ? skillData.skill_coold.ToString() + "s" : StrUtil.GetText("被动");
                skill1Des.text = StrUtil.GetText(skillData.skill_des);
                skill1Icon.sprite = skillData.skill_icon;
            }
        }
        if (data.skills.Length >= 2)
        {
            SkillData skillData = assetMng.GetSkillData(data.skills[1]);
            if (skillData != null)
            {
                skill2.SetActive(true);
                skill2Name.text = StrUtil.GetText(skillData.skill_name);
                skill2CD.text = skillData.skill_coold > 0 ? skillData.skill_coold.ToString() + "s" : StrUtil.GetText("被动");
                skill2Des.text = StrUtil.GetText(skillData.skill_des);
                skill2Icon.sprite = skillData.skill_icon;
            }
        }
        if (data.skills.Length >= 3)
        {
            SkillData skillData = assetMng.GetSkillData(data.skills[2]);
            if (skillData != null)
            {
                skill3.SetActive(true);
                skill3Name.text = StrUtil.GetText(skillData.skill_name);
                skill3CD.text = skillData.skill_coold > 0 ? skillData.skill_coold.ToString() + "s" : StrUtil.GetText("被动");
                skill3Des.text = StrUtil.GetText(skillData.skill_des);
                skill3Icon.sprite = skillData.skill_icon;
            }
        }

        back.color = BattleDef.backColor[cardData.race];
        skillBack.color = BattleDef.backColor[cardData.race];
        cardIcon.sprite = cardData.icon;
        //stars
        int starNum = cardData.cardId % 10;
        star1Icon.color = starNum >= 1 ? Color.white : new Color(0.6f, 0.6f, 0.6f);
        star2Icon.color = starNum >= 2 ? Color.white : new Color(0.6f, 0.6f, 0.6f);
        star3Icon.color = starNum >= 3 ? Color.white : new Color(0.6f, 0.6f, 0.6f);



        //calc card Name Text
        string name = StrUtil.GetText(cardData.cardName);
        if (BattleDef.language != "chinese")
        {
            cardName1.SetActive(false);
            cardName2.SetActive(false);
            englishName.text = name;
        }
        else
        {
            englishName.text = "";
            int num = name.Length;
            if (num % 2 == 1)
            {
                cardName1.SetActive(true);
                cardName2.SetActive(false);
                int index = (7 - num) / 2;
                for (int i = 0; i < num; ++i)
                {
                    Name1Texts[index + i].text = name[i].ToString();
                }
            }
            else
            {
                cardName1.SetActive(false);
                cardName2.SetActive(true);
                int index = (8 - num) / 2;
                for (int i = 0; i < num; ++i)
                {
                    Name2Texts[index + i].text = name[i].ToString();
                }
            }
        }
        if (openSkill == false) skillContent.SetActive(false);
    }

    public void SetAlpha(float alpha){
        foreach(var v in gameObject.GetComponentsInChildren<Text>()){
            v.color = new Color(v.color.r, v.color.g, v.color.b, alpha);
        }
        foreach (var v in gameObject.GetComponentsInChildren<Image>())
        {
            v.color = new Color(v.color.r, v.color.g, v.color.b, alpha);
        }
    }
}
