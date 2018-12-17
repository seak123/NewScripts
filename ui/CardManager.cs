using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Data;

public enum CardViewState{
    Idle = 1,
    Creature = 2,
    Structure = 3,
    Magic = 4
}

public struct CardInform{
    public int uid;
    public int side;
    public int id;
}

public class CardManager : MonoBehaviour {

    public GameObject cardPrefab;
    public GameObject cardBackPrefab;
    public GameObject creatureCardPrefab;
    public GameObject structureCardPrefab;

    private CardViewState state;
    private List<CardEntity> cardboxs;
    private CardInform[] enemyboxs;
    private int[] enemyHandCards;
    private int cardIndex = 0;
    private int cardUid = 0;
    private int enemyCardIndex = 0;
    private float pushWaitTime = 0f;
    private float enemyPushWaitTime = 0f;
    private float enterBattleWaitTime = 0f;

    //private List<int> cardsId;
    private readonly float cardWidth = 120f;
    private readonly float cardInterl = 40f;

    private bool hasInited = false;
    private bool initCard = false;

    private GameObject creatureCardObj;
    private GameObject structureCardObj;
    private PlayerManager playerMng;

    private Dictionary<int,int> cardReviveMap;
    private List<CardInform> playerCards;
    private List<CardInform> playerCardGrave;
    private List<CardInform> enemyCards;
    private List<CardInform> enemyCardGrave;
    // Use this for initialization
    void Start () {
        //GameRoot.BattleStartAction += CreateCard;
        cardboxs = new List<CardEntity>();
        playerCards = new List<CardInform>();
        playerCardGrave = new List<CardInform>();
        enemyCards = new List<CardInform>();
        enemyCardGrave = new List<CardInform>();
        cardReviveMap = new Dictionary<int,int>();
        creatureCardObj = Instantiate(creatureCardPrefab);
        creatureCardObj.transform.SetParent(transform);
        structureCardObj = Instantiate(structureCardPrefab);
        structureCardObj.transform.SetParent(transform);
        state = CardViewState.Idle;
        creatureCardObj.SetActive(false);
        structureCardObj.SetActive(false);

        GameRoot.BattleStartAction += InjectData;
        

    }

    private void InjectData(){
        List<int> playerData = GameRoot.GetInstance().PlayerMng.GetPlayerData().cards;
        List<int> enemyData = GameRoot.GetInstance().PlayerMng.GetEnemyData().cards;
        playerMng = GameRoot.GetInstance().PlayerMng;
        playerMng.SetCardManager(this);
        enemyHandCards = new int[GameRoot.GetInstance().PlayerMng.GetEnemyData().cardBoxNum];
        enemyboxs = new CardInform[enemyHandCards.Length];
        GameRoot.GetInstance().PlayerMng.enemyCards = enemyHandCards;
        for (int i = 0; i < enemyHandCards.Length; ++i)
        {
            enemyHandCards[i] = -1;
        }
        while (playerData.Count>0){
            int index = Random.Range(0, playerData.Count - 1);
            playerCards.Add(new CardInform{
                uid = cardUid,
                side = 1,
                id = playerData[index]
            });
            playerData.RemoveAt(index);
            ++cardUid;
        }
        while (enemyData.Count > 0)
        {
            int index = Random.Range(0, enemyData.Count - 1);
            enemyCards.Add(new CardInform
            {
                uid = cardUid,
                side = 2,
                id = enemyData[index]
            });
            enemyData.RemoveAt(index);
            ++cardUid;
        }
        CreateCard();
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
                for (int i = 0; i < enemyHandCards.Length; ++i)
                {
                    PushEnemyCard();
                }
            }
            pushWaitTime += Time.deltaTime;
            enemyPushWaitTime += Time.deltaTime;
            if (pushWaitTime > GameRoot.GetInstance().battleData.player.cardSpeed)
            {
                pushWaitTime = 0f;
                PushCard();
            }
            if (enemyPushWaitTime > GameRoot.GetInstance().battleData.enemy.cardSpeed)
            {
                enemyPushWaitTime = 0f;
                PushEnemyCard();
            }
        }
        else if (hasInited) enterBattleWaitTime -= Time.deltaTime;
	}

    public void CreateCard(){
        int num = GameRoot.GetInstance().battleData.player.cardBoxNum;
        float startX = -(num * cardWidth + (num - 1) * cardInterl) / 2;
        for (int i = 0; i < num;++i){
            float posX = i * (cardWidth + cardInterl) + cardWidth / 2 + startX;
            GameObject cardBack = Instantiate(cardBackPrefab);
            cardBack.transform.SetParent(transform);
            cardBack.transform.localPosition = new Vector3(posX+4, 10, 0);
            cardBack.transform.localScale = new Vector3(1,0.25f,1);
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
        enterBattleWaitTime = GameRoot.GetInstance().GetBattleEnterDelay();
        //cards[0].InjectData(GameRoot.GetInstance().BattleField.assetManager.GetCardData(1));
    }

    public void PushCard(){
        int index = GetEmptyCardIndex();
        if (index < 0) return;
        if (playerCards.Count == 0) return;
        int cIndex = playerCards[0].id;
        playerCardGrave.Add(playerCards[0]);
        cardReviveMap.Add(playerCards[0].uid,GameRoot.GetInstance().BattleField.assetManager.GetCardData(cIndex).num);
        cardboxs[index].InjectData(GameRoot.GetInstance().BattleField.assetManager.GetCardData(cIndex),playerCards[0].uid);
        playerCards.RemoveAt(0);
        ++cardIndex;
        pushWaitTime = 0f;
    }

    public void PushEnemyCard(){
        bool flag = true;
        int index = 0;
        for (int i = 0; i < enemyHandCards.Length;++i){
            if (enemyHandCards[i] < 0) { flag = flag && false; index = i;break; }
            else flag = flag && true;
        }
        if (flag) return;
        if (enemyCards.Count == 0) return;
        int cIndex = enemyCards[0].id;
        cardReviveMap.Add(enemyCards[0].uid,GameRoot.GetInstance().BattleField.assetManager.GetCardData(cIndex).num);
        enemyCardGrave.Add(enemyCards[0]);
        enemyHandCards[index] = cIndex;
        enemyboxs[index] = enemyCards[0];
        enemyCards.RemoveAt(0);
        ++enemyCardIndex;
        enemyPushWaitTime = 0f;
    }

    public void RecoverCard(int cardUid){
        //cardReviveMap
        if(cardReviveMap.ContainsKey(cardUid)){
            cardReviveMap[cardUid] = cardReviveMap[cardUid] - 1;
            if(cardReviveMap[cardUid]==0){
                for (int i = 0; i < playerCardGrave.Count;++i){
                    if(playerCardGrave[i].uid == cardUid){
                        playerCards.Add(playerCardGrave[i]);
                        playerCardGrave.RemoveAt(i);
                        cardReviveMap.Remove(cardUid);
                        return;
                    }
                }
                for (int i = 0; i < enemyCardGrave.Count; ++i)
                {
                    if (enemyCardGrave[i].uid == cardUid)
                    {
                        enemyCards.Add(enemyCardGrave[i]);
                        enemyCardGrave.RemoveAt(i);
                        cardReviveMap.Remove(cardUid);
                        return;
                    }
                }
            }
        }
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

    public int[] GetEnemyCardBox(){
        return enemyHandCards;
    }

    public bool PlayEnemyCard(int id,int gridX,int gridY){
        CardData data = GameRoot.GetInstance().BattleField.assetManager.GetCardData(id);
        if (data == null) return false;
        //find id and delete then
        int index = -1;
        for (int i = 0; i < enemyHandCards.Length; ++i)
        {
            if (enemyHandCards[i] == id) {index = i; break; }
        }
        if (index == -1) return false;
        if (!playerMng.RequestCost(2, data.cost)) return false;
        CreatureData creature = GameRoot.GetInstance().BattleField.assetManager.GetCreatureData(data.unitId);
        UnitData unitData = AssetManager.PackCreatureData(creature);
        unitData.card_uid = enemyboxs[index].uid;
        GameRoot.GetInstance().Bridge.CasterSkill(2, data.skillId, gridX, gridY, unitData, data.num);

        enemyHandCards[index] = -1;
        return true;
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
