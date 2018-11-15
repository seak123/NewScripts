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
   
    private Button button;
    private CardData cardData;
    private Vector3 defaultPos;

    private float camaraMoveFactor = 2;

    //creature card prop
    private CreatureData baseData;
    private CreatureData creatureData;
    private GameObject entityPrefab;

    public int TestUnitId;


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
        button.GetComponent<Image>().sprite = cardData.icon;
        if(cardData.cardType == CardType.Creature){
            creatureData = GameRoot.GetInstance().BattleField.assetManager.GetCreatureData(cardData.unitId);
            baseData = CreatureData.Clone(creatureData);
            entityPrefab = Instantiate(data.entityPrefab);
            entityPrefab.SetActive(false);
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
        }
        else
        {
            state = CardEntityState.Caster;

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
                    if (MapField.CheckPosValiable(gridX, gridY)&&gridX<=BattleDef.StructBound)
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
            }
           
        }
    }

    public void OnRelease(Vector3 screenPos){
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
                if (!MapField.CheckPosValiable(posX, posY)||posX>(BattleDef.StructBound+64))break;
                posX = Mathf.Clamp(posX, 0, BattleDef.StructBound);
                GameRoot.GetInstance().Bridge.CasterSkill(1, cardData.skillId, posX, posY, AssetManager.PackCreatureData(creatureData), cardData.num);
                CleanUp();
                break;
                
        }

    }

    public void CleanUp(){
        state = CardEntityState.Empty;

        cardData = null;

        gameObject.transform.localPosition = defaultPos;
        button.GetComponent<Image>().sprite = null;

        baseData = null;
        creatureData = null;
        Destroy(entityPrefab);
        //Destroy(truePrefab);

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
