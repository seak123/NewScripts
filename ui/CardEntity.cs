using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;
using Map;

public enum CardEntityState{
    Empty = 0,
    Idle = 1,
    Select = 2,
    Caster = 3,
    Sleep = 4
}



public class CardEntity : MonoBehaviour, IPointerDownHandler{

    public CardEntityState state;
    public CardManager cardManager;
    public Image shadow;
    public Image sprite;
    public Image bound;
    public Image magic;
    public Text cost;
    public int index;
    public GameObject hightlight;

    private PlayerManager playerMng;


    //private Button button;
    private CardData cardData;
    private Vector3 defaultPos;

    private float camaraMoveFactor = 4;

    //creature card prop
    private CreatureData baseData;
    private CreatureData creatureData;
    private GameObject entityPrefab;


    public Sprite defaultSprite;
    private Color defaultCostRed = new Color(0.8301887f, 0.23f, 0.23f, 1);

   


    private void Start()
    {
        //button = GetComponent<Button>();

        defaultPos = gameObject.transform.localPosition;
        state = CardEntityState.Empty;
        playerMng = GameRoot.GetInstance().PlayerMng;
        hightlight.SetActive(false);
        CleanUp();
    }
    private void Update()
    {
        if(state == CardEntityState.Sleep){
            if (playerMng.GetPlayerSaving() >= cardData.cost)
            {
                EnterIdleState();
                state = CardEntityState.Idle;
                cost.color = Color.white;
                sprite.color = Color.white;
            }
        }
        if(state == CardEntityState.Idle){
            if (playerMng.GetPlayerSaving() < cardData.cost)
            {
                EnterSleepState();
                state = CardEntityState.Sleep;
                cost.color = defaultCostRed;
                sprite.color = Color.gray;
            }
        }
    }

    public void NotifyGameState(){
        //debug.Log("Press!");
        //GameRoot.GetInstance().StateManager
    }

    public void InjectData(CardData data){
        if (data == null) return;
        if (playerMng.GetPlayerSaving() >= data.cost) {
            EnterIdleState();
            state = CardEntityState.Idle; 
        }
        else {
            EnterSleepState();
            state = CardEntityState.Sleep;
            cost.color = defaultCostRed;
            sprite.color = Color.gray;
        }
        cardData = data;
        switch (cardData.cardType){
            case CardType.Creature:
                creatureData = GameRoot.GetInstance().BattleField.assetManager.GetCreatureData(cardData.unitId);
                baseData = CreatureData.Clone(creatureData);
                entityPrefab = Instantiate(data.entityPrefab);
                entityPrefab.SetActive(false);
                sprite.sprite = cardData.icon;
                cost.text = cardData.cost.ToString();
                break;
            case CardType.Structure:
                creatureData = GameRoot.GetInstance().BattleField.assetManager.GetCreatureData(cardData.unitId);
                baseData = CreatureData.Clone(creatureData);
                entityPrefab = Instantiate(data.entityPrefab);
                entityPrefab.SetActive(false);
                sprite.sprite = cardData.icon;
                cost.text = cardData.cost.ToString();
                break;
        }
        SetAlpha(1);

    }

    public void OnPointerDown(PointerEventData eventData)
    {

        if (state == CardEntityState.Empty || state == CardEntityState.Sleep) return;
        state = CardEntityState.Select;
        GameRoot.GetInstance().StateManager.selectCard = this;
    }

    public void OnMove(Vector3 newPos)
    {
        gameObject.transform.position = newPos;
        //Color color = GetComponent<Image>().color;
        float factor = 1-(newPos.y  - Screen.height*BattleDef.cardPanalViewFactor)*16/ (Screen.height * BattleDef.cardPanalViewFactor);

        if (factor >= 0)
        {
            state = CardEntityState.Select;
            //GetComponent<Image>().color = new Color(color.r, color.g, color.b, factor);
            SetAlpha(factor);
            hightlight.SetActive(false);
            entityPrefab.SetActive(false);
            cardManager.SelectCard(index,cardData, creatureData);
            //switch(cardData.cardType){
            //    case CardType.Creature:
            //        creatureObj.SetActive(true);
            //        creatureObj.GetComponent<CreatureCardUI>().SetAlpha(factor);
            //        button.GetComponent<Image>().color = 
            //        break;
            //    case CardType.Structure:
            //        structureObj.SetActive(true);
            //        structureObj.GetComponent<StructureCardUI>().SetAlpha(factor);
            //        break;
            //}

        }
        else
        {
            state = CardEntityState.Caster;
            cardManager.HideCard();
         
            //assit field active
            GameRoot.GetInstance().MapField.SetAssitActive(true);

            //camara move
            CamaraManager camareMng = GameRoot.GetInstance().Camara.GetComponent<CamaraManager>();
            if (newPos.x < Screen.width*3 / 32) camareMng.MoveCamera(new Vector3(-Time.deltaTime, 0, Time.deltaTime) * camaraMoveFactor);
            if (newPos.x > Screen.width * 29 / 32) camareMng.MoveCamera(new Vector3(Time.deltaTime, 0, -Time.deltaTime) * camaraMoveFactor);
            if (newPos.y < Screen.height * BattleDef.cardPanalViewFactor * 21 / 16) camareMng.MoveCamera(new Vector3(-Time.deltaTime, 0, -Time.deltaTime) * camaraMoveFactor);
            if (newPos.y > Screen.height * 14 / 16) camareMng.MoveCamera(new Vector3(Time.deltaTime, 0, Time.deltaTime) * camaraMoveFactor);


            //GetComponent<Image>().color = new Color(color.r, color.g, color.b, factor);
            hightlight.SetActive(false);
            SetAlpha(factor);

            Ray ray = Camera.main.ScreenPointToRay(newPos);
            RaycastHit hit;
            Physics.Raycast(ray, out hit);
            int gridX, gridY;
            GameRoot.GetInstance().MapField.GetGridPos(hit.point.x, hit.point.z, out gridX, out gridY);
            if(hit.collider == null){
                entityPrefab.SetActive(false);
                return;
            }
            switch (cardData.cardType)
            {
                case CardType.Creature:
                    if (MapField.CheckPosValiable(gridX, gridY)&&gridX<=BattleDef.UnitBound)
                    {
                        float viewX, viewY;
                        GameRoot.GetInstance().MapField.GetViewPos(gridX, gridY, out viewX, out viewY);
                        entityPrefab.transform.position = new Vector3(viewX, 0, viewY);
                        entityPrefab.SetActive(true);
                        SetPrefabActive(entityPrefab);
                    }
                    else
                    {
                        float viewX, viewY;
                        GameRoot.GetInstance().MapField.GetViewPos(gridX, gridY, out viewX, out viewY);
                        entityPrefab.transform.position = new Vector3(viewX, 0, viewY);
                        entityPrefab.SetActive(true);
                        SetPrefabDeActive(entityPrefab);
                    }
                    break;
                case CardType.Structure:
                    MapField map = GameRoot.GetInstance().MapField;
                    int maxX;
                    int maxY;
                    bool posValiable = map.CheckStructurePosValiable(gridX, gridY, cardData.size, out maxX, out maxY);
                    int centerX = maxX * 16 - (int)Mathf.Floor(cardData.size * 16 / 2);
                    int centerY = maxY * 16 - (int)Mathf.Floor(cardData.size * 16 / 2);
                    if (posValiable && maxX <= BattleDef.StructBound)
                    {
                        float viewX, viewY;
                        GameRoot.GetInstance().MapField.GetViewPos(centerX, centerY, out viewX, out viewY);
                        entityPrefab.transform.position = new Vector3(viewX, 0, viewY);
                        entityPrefab.SetActive(true);
                        SetPrefabActive(entityPrefab);
                    }
                    else
                    {
                        float viewX, viewY;
                        GameRoot.GetInstance().MapField.GetViewPos(centerX, centerY, out viewX, out viewY);
                        entityPrefab.transform.position = new Vector3(viewX, 0, viewY);
                        entityPrefab.SetActive(true);
                        SetPrefabDeActive(entityPrefab);
                    }
                    break;
            }
           
        }
    }

    public void OnRelease(Vector3 screenPos){
        GameRoot.GetInstance().MapField.SetAssitActive(false);
        cardManager.HideCard();
        // get gird_pos on map
        Ray ray = Camera.main.ScreenPointToRay(screenPos);
        RaycastHit hit;
        Physics.Raycast(ray, out hit);
        int gridX, gridY;

        GameRoot.GetInstance().MapField.GetGridPos(hit.point.x, hit.point.z, out gridX, out gridY);

        gameObject.transform.localPosition = defaultPos;
        entityPrefab.SetActive(false);
        SetAlpha(1);
       
        if (state == CardEntityState.Caster && hit.collider != null)
        ExecuteCard(gridX, gridY);

    }

    public void ExecuteCard(int posX,int posY){
        if (cardData == null) return;
        switch(cardData.cardType){
            case CardType.Creature:
                if (!MapField.CheckPosValiable(posX, posY)||posX>(BattleDef.UnitBound+32))break;
                if (!playerMng.RequestCost(1, cardData.cost)) break;
                posX = Mathf.Clamp(posX, 0, BattleDef.UnitBound);
                GameRoot.GetInstance().Bridge.CasterSkill(1, cardData.skillId, posX, posY, AssetManager.PackCreatureData(creatureData), cardData.num);
                CleanUp();
                break;
            case CardType.Structure:
                if (!playerMng.RequestCost(1, cardData.cost)) break;
                MapField map = GameRoot.GetInstance().MapField;
                int maxX;
                int maxY;
                bool posValiable = map.CheckStructurePosValiable(posX, posY, cardData.size, out maxX, out maxY);
                int centerX = maxX * 16 - (int)Mathf.Floor(cardData.size * 16 / 2);
                int centerY = maxY * 16 - (int)Mathf.Floor(cardData.size * 16 / 2);
                if (!posValiable || maxX > BattleDef.StructBound) break;
                int structUid = map.CreateStructure(maxX,maxY,cardData.size);
                GameRoot.GetInstance().Bridge.CasterSkill(1, cardData.skillId, centerX, centerY, AssetManager.PackCreatureData(creatureData), structUid);
                CleanUp();
                break;
        }

    }

    public void CleanUp(){
        state = CardEntityState.Empty;

        cardData = null;

        gameObject.transform.localPosition = defaultPos;
        sprite.sprite = null;
        SetAlpha(0);
       
        baseData = null;
        creatureData = null;
        Destroy(entityPrefab);
        //Destroy(truePrefab);

    }

    private void EnterIdleState(){
        gameObject.transform.DOScale(Vector3.one * 1.15f, 0.2f).SetLoops(2, LoopType.Yoyo);
        //gameObject.transform.DOShakeScale(0.4f, new Vector3(0.1f, 0.1f, 0.1f),0,90);
        hightlight.SetActive(true);
    }
    private void EnterSleepState(){
        hightlight.SetActive(false);
    }

    private void SetAlpha(float alpha){
        shadow.color = new Color(shadow.color.r, shadow.color.g, shadow.color.b, alpha);
        sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, alpha);
        bound.color = new Color(bound.color.r, bound.color.g, bound.color.b, alpha);
        magic.color = new Color(magic.color.r, magic.color.g, magic.color.b, alpha);
        cost.color = new Color(cost.color.r, cost.color.g, cost.color.b, alpha);
    }

    //private void CleanObj(){
    //    creatureObj.SetActive(false);
    //    structureObj.SetActive(false);
    //    //magic
    //}

    private void SetPrefabActive(GameObject prefab){
        foreach(var comp in prefab.GetComponentsInChildren<SkinnedMeshRenderer>()){
            comp.material.SetFloat("_Alpha", 0.6f);
            comp.material.SetColor("_LightTint", new Color(0.285f, 1, 0.226f));
        }
        foreach (var comp in prefab.GetComponentsInChildren<SpriteRenderer>())
        {
            comp.material.SetFloat("_Alpha", 0.6f);
            comp.material.SetColor("_LightTint", new Color(0.285f, 1, 0.226f));
        }
    }

    private void SetPrefabDeActive(GameObject prefab)
    {
        foreach (var comp in prefab.GetComponentsInChildren<SkinnedMeshRenderer>())
        {
            comp.material.SetFloat("_Alpha", 0.6f);
            comp.material.SetColor("_LightTint", new Color(1, 0.425f, 0.222f));
        }
        foreach (var comp in prefab.GetComponentsInChildren<SpriteRenderer>())
        {
            comp.material.SetFloat("_Alpha", 0.6f);
            comp.material.SetColor("_LightTint", new Color(1, 0.425f, 0.222f));
        }
    }
}
