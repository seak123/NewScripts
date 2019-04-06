using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreatureCardUI : MonoBehaviour {

    public GameObject constructureCard;
    public Text constructureName;
    public Image constructureIcon;
    public GameObject unitCard;
    public GameObject skillCard;
    



    public void CleanUp(){
       
    }

    public void InjectData(CreatureFightData data){
        switch(data.type){
            case 0:
                unitCard.SetActive(false);
                skillCard.SetActive(false);
                constructureCard.SetActive(true);
                constructureName.text = data.CreatureName;
                constructureIcon.sprite = GameRoot.GetInstance().BattleField.assetManager.GetIcon(data.icon);
                break;
            case 1:
                unitCard.SetActive(false);
                skillCard.SetActive(false);
                constructureCard.SetActive(true);
                constructureName.text = data.CreatureName;
                constructureIcon.sprite = GameRoot.GetInstance().BattleField.assetManager.GetIcon(data.icon);
                break;
            case 2:
                break;
        }
   
    }
}
