using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillIconUI : MonoBehaviour {

    public Image bound;
    public Image icon;
    public Text skillName;

    private ClickEvent clickEvent;
    private string detailName;
    private SkillData skillData;

    private void Start()
    {
        clickEvent = gameObject.GetComponent<ClickEvent>();
        clickEvent.longClickAction += OpenDestination;
    }

    public void InjectData(SkillData data){
        skillData = data;
        switch(data.skill_level){
            case 1:
                detailName = StrUtil.GetText(data.skill_name) + "(S)";
                skillName.color = new Color(0.85f, 0.64f, 0.27f);
                bound.color = new Color(0.85f,0.64f,0.27f);
                break;
            case 2:
                detailName = StrUtil.GetText(data.skill_name) + "(A)";
                skillName.color = new Color(0.81f, 0.43f, 0.68f);
                bound.color = new Color(0.81f, 0.43f, 0.68f);
                break;
            case 3:
                detailName = StrUtil.GetText(data.skill_name) + "(B)";
                skillName.color = new Color(0.36f, 0.65f, 0.85f);
                bound.color = new Color(0.36f, 0.65f, 0.85f);
                break;
            case 4:
                detailName = StrUtil.GetText(data.skill_name) + "(C)";
                skillName.color = new Color(0.43f, 0.82f, 0.47f);
                bound.color = new Color(0.43f, 0.82f, 0.47f);
                break;
            case 5:
                detailName = StrUtil.GetText(data.name) + "(D)";
                skillName.color = new Color(0.9f, 0.9f, 0.9f);
                bound.color = new Color(0.9f, 0.9f, 0.9f);
                break;
        }
        skillName.text = StrUtil.GetText(data.skill_name);
        icon.sprite = data.skill_icon;
    }

    public void OpenDestination(){
        Debug.Log("show skill des");
        if (skillData == null) return;
        List<string> contents = new List<string>();
        contents.Add("<color=#2CFFFFFF><b>"+StrUtil.GetText(skillData.skill_name)+"</b></color>"+":\n"+StrUtil.GetText(skillData.skill_des));
        for (int i = 0; i < skillData.tips.Length;++i){
            switch(skillData.tips[i]){
                case SkillTip.Energy:
                    contents.Add(StrUtil.GetText("<color=#2CFFFFFF><b>力量</b></color>:每层力量提供额外1点攻击力，普通攻击后层数减半"));
                    break;
                case SkillTip.Strength:
                    contents.Add(StrUtil.GetText("<color=#2CFFFFFF><b>力量</b></color>:每层力量提供额外1点攻击力，普通攻击后层数减半"));
                    break;
            }
        }
        GameRoot.GetInstance().mainUIMng.OpenTip(contents,gameObject.GetComponent<RectTransform>().position,50,50);
    }
}
