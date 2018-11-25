using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Map;

public enum CardEntityState{
    Empty = 0,
    Idle = 1,
    Select = 2,
    Caster = 3
}



public class CardEntity : MonoBehaviour, IPointerDownHandler{

    public CardEntityState state;
    public CardManager cardManager;
    public int index;


    private Button button;
    private CardData cardData;
    private Vector3 defaultPos;

    private float camaraMoveFactor = 2;

    //creature card prop
    private CreatureData baseData;
    private CreatureData creatureData;
    private GameObject entityPrefab;


    public Sprite defaultSprite;
    public GameObject creatureObj;
    public GameObject structureObj;
    public GameObject MagicObj;


   


    private void Start()
    {
        button = GetComponent<Button>();

        defaultPos = gameObject.transform.localPosition;
        state = CardEntityState.Empty;
    }

    public void NotifyGameState(){
        //debug.Log("Press!");
        //GameRoot.GetInstance().StateManager
    }

    public void InjectData(CardData data){
        if (data == null) return;
        state = CardEntityState.Idle;
        cardData = data;
        switch (cardData.cardType){
            case CardType.Creature:
                creatureData = GameRoot.GetInstance().BattleField.assetManager.GetCreatureData(cardData.unitId);
                baseData = CreatureData.Clone(creatureData);
                entityPrefab = Instantiate(data.entityPrefab);
                entityPrefab.SetActive(false);
                CleanObj();
                creatureObj.GetComponent<CreatureCardUI>().CleanUp();
                creatureObj.GetComponent<CreatureCardUI>().InjectData(data, false);
                creatureObj.transform.localPosition = Vector3.zero;
                creatureObj.SetActive(true);
                break;
            case CardType.Structure:
                creatureData = GameRoot.GetInstance().BattleField.assetManager.GetCreatureData(cardData.unitId);
                baseData = CreatureData.Clone(creatureData);
                entityPrefab = Instantiate(data.entityPrefab);
                entityPrefab.SetActive(false);
                CleanObj();
                structureObj.GetComponent<StructureCardUI>().CleanUp();
                structureObj.GetComponent<StructureCardUI>().InjectData(data, false);
                structureObj.transform.localPosition = Vector3.zero;
                structureObj.SetActive(true);
                break;
        }

    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (state == CardEntityState.Empty) return;
        state = CardEntityState.Select;
        GameRoot.GetInstance().StateManager.selectCard = this;
    }

    public void OnMove(Vector3 newPos)
    {
        gameObject.transform.position = newPos;
        Color color = GetComponent<Image>().color;
        float factor = 1-(newPos.y  - Screen.height*BattleDef.cardPanalViewFactor)*16/ (Screen.height * BattleDef.cardPanalViewFactor);

        if (factor >= 0)
        {
            state = CardEntityState.Select;
            GetComponent<Image>().color = new Color(color.r, color.g, color.b, factor);
            entityPrefab.SetActive(false);
            cardManager.SelectCard(index,cardData, creatureData);
            switch(cardData.cardType){
                case CardType.Creature:
                    creatureObj.SetActive(true);
                    creatureObj.GetComponent<CreatureCardUI>().SetAlpha(factor);
                    break;
                case CardType.Structure:
                    structureObj.SetActive(true);
                    structureObj.GetComponent<StructureCardUI>().SetAlpha(factor);
                    break;
            }

        }
        else
        {
            state = CardEntityState.Caster;
            cardManager.HideCard();
            CleanObj();
            //assit field active
            GameRoot.GetInstance().MapField.SetAssitActive(true);

            //camara move
            CamaraManager camareMng = GameRoot.GetInstance().Camara.GetComponent<CamaraManager>();
            if (newPos.x < Screen.width / 16) camareMng.MoveCamera(new Vector3(-Time.deltaTime, 0, Time.deltaTime) * camaraMoveFactor);
            if (newPos.x > Screen.width * 15 / 16) camareMng.MoveCamera(new Vector3(Time.deltaTime, 0, -Time.deltaTime) * camaraMoveFactor);
            if (newPos.y < Screen.height * BattleDef.cardPanalViewFactor * 21 / 16) camareMng.MoveCamera(new Vector3(-Time.deltaTime, 0, -Time.deltaTime) * camaraMoveFactor);
            if (newPos.y > Screen.height * 14 / 16) camareMng.MoveCamera(new Vector3(Time.deltaTime, 0, Time.deltaTime) * camaraMoveFactor);


            GetComponent<Image>().color = new Color(color.r, color.g, color.b, factor);

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
        Color color = GetComponent<Image>().color;
        GetComponent<Image>().color = new Color(color.r, color.g, color.b, 1);
       
        if (state == CardEntityState.Caster && hit.collider != null)
        ExecuteCard(gridX, gridY);

    }

    public void ExecuteCard(int posX,int posY){
        if (cardData == null) return;
        switch(cardData.cardType){
            case CardType.Creature:
                if (!MapField.CheckPosValiable(posX, posY)||posX>(BattleDef.UnitBound+32))break;
                posX = Mathf.Clamp(posX, 0, BattleDef.UnitBound);
                GameRoot.GetInstance().Bridge.CasterSkill(1, cardData.skillId, posX, posY, AssetManager.PackCreatureData(creatureData), cardData.num);
                CleanUp();
                break;
            case CardType.Structure:
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
        button.GetComponent<Image>().sprite = defaultSprite;
        CleanObj();

        baseData = null;
        creatureData = null;
        Destroy(entityPrefab);
        //Destroy(truePrefab);

    }

    private void CleanObj(){
        creatureObj.SetActive(false);
        structureObj.SetActive(false);
        //magic
    }

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
