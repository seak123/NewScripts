using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public enum CardViewState{
    Idle = 1,
    Creature = 2,
    Structure = 3,
    Magic = 4
}

public class CardManager : MonoBehaviour {

    public GameObject cardPrefab;
    public GameObject creatureCardPrefab;
    public GameObject structureCardPrefab;

    private CardViewState state;
    private List<CardEntity> cardboxs;
    private int cardIndex = 0;
    private float pushWaitTime = 0f;
    private float enterBattleWaitTime = 3f;

    private List<int> cardsId;
    private readonly float cardWidth = 120f;
    private readonly float cardInterl = 20f;

    private bool hasInited = false;
    private bool initCard = false;

    private GameObject creatureCardObj;
    private GameObject structureCardObj;
    // Use this for initialization
    void Start () {
        GameRoot.BattleStartAction += CreateCard;
        cardboxs = new List<CardEntity>();
        creatureCardObj = Instantiate(creatureCardPrefab);
        creatureCardObj.transform.SetParent(transform);
        structureCardObj = Instantiate(structureCardPrefab);
        structureCardObj.transform.SetParent(transform);
        state = CardViewState.Idle;
        creatureCardObj.SetActive(false);
        structureCardObj.SetActive(false);

        //temp info
        cardsId = new List<int>
        {
            7,
            2,
            1,
            2,
            1,
            2,
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
            card.transform.localScale = Vector3.one;
            card.GetComponent<CardEntity>().cardManager = this;
            card.GetComponent<CardEntity>().index = i;
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

    public void SelectCard(int index,CardData cardData,CreatureData creatureData){
        if(cardData.cardType == CardType.Creature){
            if(state == CardViewState.Idle){
                creatureCardObj.SetActive(true);
                creatureCardObj.transform.localPosition = cardboxs[index].transform.localPosition;
                creatureCardObj.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
                if (creatureData.skills.Length == 0)
                    creatureCardObj.transform.DOLocalMove(new Vector3(600, 400, 0), 0.5f);
                else creatureCardObj.transform.DOLocalMove(new Vector3(350, 400, 0), 0.5f);
                creatureCardObj.transform.DOScale(Vector3.one, 0.5f);
                CreatureCardUI ui = creatureCardObj.GetComponent<CreatureCardUI>();
                ui.CleanUp();
                ui.InjectData(cardData);
                state = CardViewState.Creature;
            }
        }else if(cardData.cardType == CardType.Structure){
            if(state == CardViewState.Idle){
                structureCardObj.SetActive(true);
                structureCardObj.transform.localPosition = cardboxs[index].transform.localPosition;
                structureCardObj.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
                structureCardObj.transform.DOLocalMove(new Vector3(350, 400, 0), 0.5f);
                structureCardObj.transform.DOScale(Vector3.one, 0.5f);
                StructureCardUI ui = structureCardObj.GetComponent<StructureCardUI>();
                ui.CleanUp();
                ui.InjectData(cardData);
                state = CardViewState.Structure;
            }
        }
    }

    public void HideCard(){
        creatureCardObj.SetActive(false);
        structureCardObj.SetActive(false);
        state = CardViewState.Idle;
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
