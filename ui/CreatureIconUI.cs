using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreatureIconUI : MonoBehaviour {

    public GameObject[] stars;

    public Text level;

    public Image icon;

    public Image bound;

    public GameObject selected;

    public GameObject mask;

    public CreatureFightData creatureData;

    private void Start()
    {
        ClickEvent clickEvent = GetComponent<ClickEvent>();
        clickEvent.longClickAction += OpenDestination;
    }


    public void InjectData(CreatureFightData data){
        //init star
        for (int i = 0; i < stars.Length;++i){
            if(i<data.star){
                stars[i].SetActive(true);
            }else
            {
                stars[i].SetActive(false);
            }
        }
        //init lvl
        level.text = "Lv." + data.level;

        //init icon
        icon.sprite = GameRoot.GetInstance().BattleField.assetManager.GetIcon(data.icon);

        //reset color
        if(data.type == 1){
            bound.color = new Color(0.66f,0.66f,0.66f);
        }else{
            bound.color = new Color(0.51f,0.38f,0.51f);
        }
        creatureData = data;
    }

    public void MarkSelected(bool flag){
        if(flag){
            mask.SetActive(true);
            selected.SetActive(true);
        }else{
            mask.SetActive(false);
            selected.SetActive(false);
        }
    }

    public void OpenDestination(){
        GameRoot.GetInstance().mainUIMng.OpenCreatureCard(creatureData,gameObject.transform.position);
    }
}
