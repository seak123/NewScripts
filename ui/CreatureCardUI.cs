using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreatureCardUI : MonoBehaviour {

    public GameObject constructureCard;
    public Text constructureName;
    public Image constructureIcon;
    public GameObject unitCard;
    public Image unitSprite;
    public Text unitName;
    public Image[] stars;
    public GameObject skillCard;

    



    public void CleanUp(){
       
    }

    public void InjectData(CreatureFightData data){
        switch(data.type){
            case 0:
                unitCard.SetActive(true);
                skillCard.SetActive(false);
                constructureCard.SetActive(false);
                unitName.text = data.CreatureName;
                unitSprite.sprite = GameRoot.GetInstance().BattleField.assetManager.GetCards(data.icon);
                for (int i = 0; i < stars.Length;++i){
                    stars[i].color = i < data.star ? new Color(1,1,1) : new Color(0.3f,0.3f,0.3f);
                }
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
