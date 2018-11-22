using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour {

    public GameObject cardPrefab;
    private List<CardEntity> cardboxs;
    private int cardIndex = 0;
    private float pushWaitTime = 0f;
    private float enterBattleWaitTime = 3f;

    private List<int> cardsId;
    private readonly float cardWidth = 120f;
    private readonly float cardInterl = 20f;

    private bool hasInited = false;
    private bool initCard = false;
	// Use this for initialization
	void Start () {
        GameRoot.BattleStartAction += CreateCard;
        cardboxs = new List<CardEntity>();

        //temp info
        cardsId = new List<int>
        {
            7,
            7,
            7,
            7,
            7,
            7,
            7,
            1
        };
    }
	
	// Update is called once per frame
	void Update () {
        if (hasInited && enterBattleWaitTime < 0)
        {
            if (initCard)
            {
                initCard = false;
                for (int i = 0; i < cardboxs.Count; ++i)
                {
                    PushCard();
                }
            }
            pushWaitTime += Time.deltaTime;
            if (pushWaitTime > GameRoot.GetInstance().battleData.player.cardSpeed)
            {
                pushWaitTime = 0f;
                PushCard();
            }
        }
        else if (hasInited) enterBattleWaitTime -= Time.deltaTime;
	}

    public void CreateCard(){
        int num = GameRoot.GetInstance().battleData.player.cardBoxNum;
        float startX = -(num * cardWidth + (num - 1) * cardInterl) / 2;
        for (int i = 0; i < num;++i){
            float posX = i * (cardWidth + cardInterl) + cardWidth / 2 + startX;
            GameObject card = Instantiate(cardPrefab);
            card.transform.SetParent(transform);
            card.transform.localPosition = new Vector3(posX, 10, 0);
            cardboxs.Add(card.GetComponent<CardEntity>());
        }
        hasInited = true;
        initCard = true;
        //cards[0].InjectData(GameRoot.GetInstance().BattleField.assetManager.GetCardData(1));
    }

    public void PushCard(){
        int index = GetEmptyCardIndex();
        if (index < 0) return;
        int cIndex = cardsId[cardIndex % cardsId.Count];
        cardboxs[index].InjectData(GameRoot.GetInstance().BattleField.assetManager.GetCardData(cIndex));
        ++cardIndex;
        pushWaitTime = 0f;
    }

    private int GetEmptyCardIndex(){
        for (int i = 0; i < cardboxs.Count;++i){
            if(cardboxs[i].state == CardEntityState.Empty){
                return i;
            }
        }
        return -1;
    }


}
