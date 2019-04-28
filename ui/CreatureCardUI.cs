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
                baseAttackFrame.text = (data.base_attack_interval).ToString("F2")+"s";
                attackRange.text = Mathf.FloorToInt(data.attack_range).ToString();
                defence.text = Mathf.FloorToInt(data.defence).ToString();
                reduce.text = data.defence>=0?(data.defence*2f/(100f+data.defence*2f)*100f).ToString("F2")+"%":"-"+((1-Mathf.Pow((100f-2f)/100f,-data.defence))*100f).ToString("F2")+"%";
                speed.text = Mathf.FloorToInt(data.speed).ToString();

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
