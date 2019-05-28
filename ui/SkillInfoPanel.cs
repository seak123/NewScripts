using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillInfoPanel : MonoBehaviour {

    public SkillIconUI icon;

    public GameObject back;
    public Text desTxt;
    public Text lockTxt;

    private SkillData data;
	// Use this for initialization
	void Start () {
		
	}


    public void InjectData(SkillData _data,HeroData heroData){
        data = _data;
        icon.InjectData(_data);
        desTxt.text = StrUtil.GetText(_data.skill_name);
        lockTxt.text = StrUtil.GetText("解锁需要熟练度") + " " + data.needExp;
    }

    public void Lock(bool flag){
        icon.SetLock(flag);
        if(flag){
            back.SetActive(true);
        }else{
            back.SetActive(false);
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
