using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SkillTip{
    Energy = 0,
    Strength = 1,
    Vampire,
}

public class SkillData : ScriptableObject
{
    public int skill_id;

    //xiyoudu: 1,orange_S 2,violet_A 3,blue_B 4,green_C 5,white_D
    public int skill_level;

    public float skill_coold;

    public Sprite skill_icon;

    public string skill_name;

    public string skill_des;

    public SkillTip[] tips;
}
