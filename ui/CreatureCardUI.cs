using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class CreatureCardUI : MonoBehaviour {

    public GameObject constructureCard;
    public Text constructureName;
    public Image constructureIcon;
    //unit property
    public GameObject unitCard;
    public Image unitSprite;
    public Text unitName;
    public Image[] stars;
    public Text level;
    public Text exp;
    public Slider expSlider;
    public Text hp;
    public Text attack;
    public Text baseAttackFrame;
    public Text attackRange;
    public Text defence;
    public Text reduce;
    public Text speed;


    //skill property
    public GameObject skillCard;
    public SkillIconUI[] skillIcons;

    //notify
    public GameObject notify;

    //click event
    public ClickEvent clickEvent_1;
    public ClickEvent clickEvent_2;



    private CreatureFightData _data;
    private bool isAnim = false;
    private bool isNotify = false;
    private int notifyState = 1;

    private void Start()
    {
        clickEvent_1.clickAction += ExchangeCard;
        clickEvent_2.clickAction += ExchangeCard;
    }

    public void CleanUp(){
       
    }

    public void InjectData(CreatureFightData data){
        _data = data;
        isAnim = false;
        switch(data.type){
            case 0:
                unitCard.SetActive(true);
                skillCard.SetActive(false);
                constructureCard.SetActive(false);
                unitName.text = StrUtil.GetText(data.CreatureName);
                unitSprite.sprite = GameRoot.GetInstance().BattleField.assetManager.GetCards(data.icon);
                for (int i = 0; i < stars.Length;++i){
                    stars[i].color = i < data.star ? new Color(1,1,1) : new Color(0.3f,0.3f,0.3f);
                }
                level.text = "Lv." + data.level;
                exp.text = data.exp + "/" + data.expMax;
                expSlider.value = (float)data.exp / (float)data.expMax;
                hp.text = Mathf.FloorToInt(data.hp).ToString();
                attack.text = Mathf.FloorToInt(data.attack).ToString();
                baseAttackFrame.text = StrUtil.GetText(GetAttackRateString(data.base_attack_interval));
                attackRange.text = Mathf.FloorToInt(data.attack_range).ToString();
                defence.text = Mathf.FloorToInt(data.defence).ToString();
                reduce.text = data.defence>=0?(data.defence*2f/(100f+data.defence*2f)*100f).ToString("F2")+"%":"-"+((1-Mathf.Pow((100f-2f)/100f,-data.defence))*100f).ToString("F2")+"%";
                speed.text = StrUtil.GetText(GetSpeedString(data.speed));

                notify.SetActive(true);
                isNotify = true;
                notifyState = -1;
                notify.GetComponent<Text>().text = StrUtil.GetText("点击翻转查看能力");
                //skill data
                int[] skills = data.skills;
                int num = skills.Length;
                for (int i = 0; i < skillIcons.Length;++i){
                    skillIcons[i].gameObject.SetActive(false);
                }
                for (int i = 0; i < num;++i){
                    skillIcons[i].gameObject.SetActive(true);
                    skillIcons[i].InjectData(GameRoot.GetInstance().BattleField.assetManager.GetSkillData(skills[i]));
                }

                break;
            case 1:
                unitCard.SetActive(false);
                skillCard.SetActive(false);
                constructureCard.SetActive(true);
                constructureName.text = StrUtil.GetText(data.CreatureName);
                constructureIcon.sprite = GameRoot.GetInstance().BattleField.assetManager.GetIcon(data.icon);
                notify.SetActive(false);
                isNotify = false;
                break;
            case 2:
                break;
        }
   
    }

    private string GetAttackRateString(float rate){
        if(rate<=0.4f){
            return "极其快";
        }else if(rate>0.4f&&rate<=0.8f){
            return "非常快";
        }else if(rate>0.8f&&rate<=1.2f){
            return "快";
        }else if(rate>1.2f&&rate<=1.6f){
            return "较快";
        }else if(rate>1.6f&&rate<=2f){
            return "普通";
        }else if(rate>2f&&rate<=2.4f){
            return "较慢";
        }else if(rate>2.4f&&rate<=2.8f){
            return "慢";
        }else if(rate>2.8f&&rate<=3.2f){
            return "非常慢";
        }else{
            return "极其慢";
        }
    }

    private string GetSpeedString(float rate)
    {
        if (rate > 72f)
        {
            return "极其快";
        }
        else if (rate > 64f && rate <= 72f)
        {
            return "非常快";
        }
        else if (rate > 56f && rate <= 64f)
        {
            return "快";
        }
        else if (rate > 48f && rate <= 56f)
        {
            return "较快";
        }
        else if (rate > 40f && rate <= 48f)
        {
            return "普通";
        }
        else if (rate > 32f && rate <= 40f)
        {
            return "较慢";
        }
        else if (rate > 24f && rate <= 32f)
        {
            return "慢";
        }
        else if (rate > 16f && rate <= 24f)
        {
            return "非常慢";
        }
        else
        {
            return "极其慢";
        }
    }

    public void ExchangeCard(){
        if (isAnim) return;
        if (!isNotify) return;
        isAnim = true;
        notify.SetActive(false);
        isNotify = false;
        gameObject.transform.DOScale(new Vector3(0, 1, 1), 0.15f).onComplete += () =>
        {
            if(unitCard.activeSelf){
                unitCard.SetActive(false);
                skillCard.SetActive(true);
                constructureCard.SetActive(false);
                notify.GetComponent<Text>().text = StrUtil.GetText("点击翻转查看属性");
            }
            else{
                unitCard.SetActive(true);
                skillCard.SetActive(false);
                constructureCard.SetActive(false);
                notify.GetComponent<Text>().text = StrUtil.GetText("点击翻转查看能力");
            }
            gameObject.transform.DOScale(new Vector3(1, 1, 1), 0.15f).onComplete += () =>
            {
                isAnim = false;
                notify.SetActive(true);
                isNotify = true;
                notifyState = -1;

            };
        };
    }

    private void Update()
    {
        if(isNotify){
            if (notifyState == -1)
            {
                notifyState = 0;
                notify.GetComponent<Text>().DOColor(new Color(1, 1, 1, 1), 1f).onComplete += () =>
                {
                    notifyState = 1;
                };
            }else if(notifyState == 1){
                notifyState = 0;
                notify.GetComponent<Text>().DOColor(new Color(1, 1, 1, 0), 1f).onComplete += () =>
                {
                    notifyState = -1;
                };
            }
        }
    }
}
