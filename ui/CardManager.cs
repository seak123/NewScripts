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
    public GameObject cardBackPrefab;
    public GameObject creatureCardPrefab;
    public GameObject structureCardPrefab;

    private CardViewState state;
    private List<CardEntity> cardboxs;
    private int[] enemyBoxs;
    private int cardIndex = 0;
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

    private List<int> playerCards;
    private List<int> enemyCards;
    // Use this for initialization
    void Start () {
        //GameRoot.BattleStartAction += CreateCard;
        cardboxs = new List<CardEntity>();
        playerCards = new List<int>();
        enemyCards = new List<int>();
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
        enemyBoxs = new int[GameRoot.GetInstance().PlayerMng.GetEnemyData().cardBoxNum];
        GameRoot.GetInstance().PlayerMng.enemyCards = enemyBoxs;
        for (int i = 0; i < enemyBoxs.Length; ++i)
        {
            enemyBoxs[i] = -1;
        }
        while (playerData.Count>0){
            int index = Random.Range(0, playerData.Count - 1);
            playerCards.Add(playerData[index]);
            playerData.RemoveAt(index);
        }
        while (enemyData.Count > 0)
        {
            int index = Random.Range(0, enemyData.Count - 1);
            enemyCards.Add(enemyData[index]);
            enemyData.RemoveAt(index);
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
                for (int i = 0; i < enemyBoxs.Length; ++i)
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
        int cIndex = playerCards[0];
        playerCards.RemoveAt(0);
        cardboxs[index].InjectData(GameRoot.GetInstance().BattleField.assetManager.GetCardData(cIndex));
        ++cardIndex;
        pushWaitTime = 0f;
    }

    public void PushEnemyCard(){
        bool flag = true;
        int index = 0;
        for (int i = 0; i < enemyBoxs.Length;++i){
            if (enemyBoxs[i] < 0) { flag = flag && false; index = i; }
            else flag = flag && true;
        }
        if (flag) return;
        if (enemyCards.Count == 0) return;
        int cIndex = enemyCards[0];
        enemyCards.RemoveAt(0);
        enemyBoxs[index] = cIndex;
        ++enemyCardIndex;
        enemyPushWaitTime = 0f;
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
        return enemyBoxs;
    }

    public void PlayEnemyCard(int id,int gridX,int gridY){
        CardData data = GameRoot.GetInstance().BattleField.assetManager.GetCardData(id);
        if (data == null) return;
        if (!playerMng.RequestCost(2, data.cost)) return;
        CreatureData creature = GameRoot.GetInstance().BattleField.assetManager.GetCreatureData(data.unitId);
        GameRoot.GetInstance().Bridge.CasterSkill(2, data.skillId, gridX, gridY, AssetManager.PackCreatureData(creature), data.num);
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
