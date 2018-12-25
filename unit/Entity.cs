using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Map
{
    public enum TransformState{
        Straight = 1,
        AStar = 2
    }

    public class Entity : MonoBehaviour
    {
        public int id;
        public int side;
        public int uid;
        public int structUid;
        public int radius;
        public int cost;
        public int posX = 0;
        public int posY = 0;
        private int desX = 0;
        private int desY = 0;

        public Animator animator;

        private int RouteUpdateFlag = 0;
        private int RouteUpdateFactor = 0;
        private float animatorAttackSpeed = 1f;
        private float moveSpeed = 1;
        private float baseSpeed = 1;
        private GameObject shadow;
        private Vector3 shadowPos;
        private GameObject hpPrefab;
        private GameObject hpBar;
        private float hpBarCacheTime = 0;
		private float damageCacheTime = 0;
		private float healCacheTime = 0;
        private Quaternion forward;

        HeapNode currMapNode;

        private AssetManager mng;
        private TransformState state = TransformState.Straight;

        private void SetTransform(int gridX,int gridY){
            float x = 0f;
            float y = 0f;
            GameRoot.GetInstance().MapField.GetViewPos(gridX,gridY,out x,out y);
            gameObject.transform.position = new Vector3(x, 0, y);
        }

        public void Appear(){
            gameObject.SetActive(true);
        }
        // >>>>>>>>>>>>>>>>> Transform 
        public void SetRotation(int toX, int toY){
            if(toX == posX && toY == posY)return;
            Vector2 direct = new Vector2(toX - posX, toY - posY);
            float angle = Vector2.Angle(new Vector2(1, 0), direct);

            angle = direct.y<0? angle:-angle;

            forward = Quaternion.Euler(0, angle, 0);

           
            //Quaternion start = gameObject.transform.rotation;
            //Quaternion end = Quaternion.Euler(0, angle, 0);
            //gameObject.transform.rotation = Quaternion.Lerp(start,end,0.1f);
        }

        public Vector3 GetSocketPos(string name){
            return gameObject.transform.Find(name).gameObject.transform.position;
        }

        public void ResetMapNode(){
            MapField field = GameRoot.GetInstance().MapField;
            field.MarkMovable(posX, posY, radius, false);
            //currMapNode = field.GetAStarRoute(id, posX, posY, desX, desY);
            field.MarkMovable(posX, posY, radius, true);
        }
     
        public void Move(int toX,int toY,int speed,float value,out int gridX,out int gridY,out float offset)
        {
            //init data
            desX = toX;
            desY = toY;
            moveSpeed = speed;
            MapField field = GameRoot.GetInstance().MapField;


            float toViewX, toViewY;
            field.GetViewPos(toX, toY, out toViewX, out toViewY);

            field.MarkMovable(posX, posY, radius, false);

            if (state == TransformState.Straight)
            {
                
                RouteUpdateFlag += 1;
                float nowViewX = gameObject.transform.position.x;
                float nowViewY = gameObject.transform.position.z;
                float factor = value/BattleDef.Transfer2GridFactor / Vector2.Distance(new Vector2(toViewX, toViewY), new Vector2(nowViewX, nowViewY));
                float nextViewX = Mathf.Max(0,Mathf.Min(BattleDef.columnGridNum-1,nowViewX + (toViewX - nowViewX) * factor));
                float nextViewY = Mathf.Max(0, Mathf.Min(BattleDef.rowGridNum - 1,nowViewY + (toViewY - nowViewY) * factor));
                field.GetGridPos(nextViewX, nextViewY, out gridX, out gridY);
                if (field.IsCanMove(gridX, gridY, radius))
                {
                    RouteUpdateFactor = 0;
                    offset = 0;
                    //if (debug) Debug.Log("Straight:"+gridX+" "+gridY+"offset:"+offset);
                    gameObject.transform.position = new Vector3(nextViewX, 0f, nextViewY);
                    if(RouteUpdateFlag>5) SetRotation(toX, toY);
                    posX = gridX;
                    posY = gridY;
                    field.MarkMovable(posX, posY, radius, true);

                    return;
                }else{
                    RouteUpdateFactor += 1;
                    if (RouteUpdateFactor > 4) RouteUpdateFactor = 4;
                    state = TransformState.AStar;
                    currMapNode = field.GetAStarRoute(id, posX, posY, toX, toY,speed*BattleDef.aStarUpdateFrame*RouteUpdateFactor/30);
                    RouteUpdateFlag = 0;
                }
            }

            if (state == TransformState.AStar)
            {
                RouteUpdateFlag += 1;
                float startG = currMapNode.G;
                while (currMapNode.Next != null && (currMapNode.Next.G - startG) <= value)
                {

                    currMapNode = currMapNode.Next;
                }
                if(field.IsCanMove(currMapNode.X, currMapNode.Y, radius)){
                    if(currMapNode.Next!= null)offset = value - (currMapNode.G - startG);
                    else offset = 0;
                    gridX = currMapNode.X;
                    gridY = currMapNode.Y;
                    //if (debug) Debug.Log("AStar:" + gridX + " " + gridY+ "offset:"+offset);
                    SetTransform(gridX, gridY);
                    SetRotation(gridX, gridY);
                    posX = gridX;
                    posY = gridY;
                    if(RouteUpdateFlag == BattleDef.aStarUpdateFrame*RouteUpdateFactor){
                        state = TransformState.Straight;
                        RouteUpdateFlag = 0;
                    }
                     field.MarkMovable(posX, posY, radius, true);
                    return;
                }else{
                    currMapNode.Next = null;
                }
                if(currMapNode.Next == null){
                    gridX = posX;
                    gridY = posY;
                    offset = 0;
                    field.MarkMovable(posX, posY, radius, true);
                    RouteUpdateFlag = 0;
                    state = TransformState.Straight;
                    return;
                }
            }
            gridX = 0;
            gridY = 0;
            offset = 0;
        }
        // >>>>>>>>>>>>>>>>>>>> UI
        public void SetHp(float hp, float maxHp,int isHeal)
        {
            if (hpBar == null)
            {
                hpBar = Instantiate(hpPrefab);
                hpBar.GetComponent<RectTransform>().parent = GameRoot.GetInstance().battleGroundUI.GetComponent<RectTransform>();
                //hpBar.GetComponent<RectTransform>().sizeDelta = new Vector2(Mathf.Sqrt(radius)/2*80,22);
                hpBar.transform.localScale = new Vector3(Mathf.Sqrt(radius) / 2, 1, 1)*1.2f;
                hpBar.SetActive(false);
                GameRoot.GetInstance().Camara.GetComponent<CamaraManager>().UpdateUI += UpdateHpBar;
            }
            hpBar.SetActive(true);
            hpBar.GetComponent<Slider>().value = hp / maxHp;
            hpBarCacheTime = 0;

			if(isHeal==0){
                //damage
				damageCacheTime = 0.4f;
			}else{
                //heal
				healCacheTime = 0.4f;
			}
        }

        // >>>>>>>>>>>>>>>>>>>> Animator
        public void AnimCasterBreak(){
            if(!animator.GetCurrentAnimatorStateInfo(0).IsName("Idle")){
                animator.SetTrigger("Break");
            }
        }

        public void AnimCasterAction(string name){
            if(!animator.GetCurrentAnimatorStateInfo(0).IsName("Idle")){
                animator.SetTrigger("Break");
            }
            animator.SetTrigger(name);
        }

        public void AnimCasterAttack(float attack_rate){
            if(!animator.GetCurrentAnimatorStateInfo(0).IsName("Idle")){
                animator.SetTrigger("Break");
            }
            animatorAttackSpeed = attack_rate;
            animator.SetTrigger("Attack");

        }

        public void Die(int cardUid){
            if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
            {
                animator.SetTrigger("Break");
            }
            animator.SetTrigger("Die");

            GameRoot.GetInstance().PlayerMng.AddSaving(GetSocketPos("S_Center"), 3 - side, (int)(cost*BattleDef.KillEarnFactor));
            GameRoot.GetInstance().MapField.RemoveEntity(this,2f);
            Destroy(hpBar, 0.5f);
            Destroy(gameObject, 2f);
            GameRoot.GetInstance().PlayerMng.GetCardManager().RecoverCard(cardUid);
        }

        private void Start()
        {
            mng = GameRoot.GetInstance().BattleField.assetManager;
            shadow = gameObject.transform.Find("Shadow").gameObject;
            baseSpeed = GameRoot.GetInstance().BattleField.assetManager.GetCreatureData(id).base_speed;
            //init hp bar
            if (side == 1){
                hpPrefab = GameRoot.GetInstance().BattleField.assetManager.GreenSlider;
                gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);
                shadow.transform.rotation = Quaternion.Euler(90, 0, 0);
                shadowPos = shadow.transform.localPosition;
            }
            else{
                hpPrefab = GameRoot.GetInstance().BattleField.assetManager.RedSlider;
                gameObject.transform.rotation = Quaternion.Euler(0, 180, 0);
                shadow.transform.rotation = Quaternion.Euler(90, 0, 0);
                shadowPos = -shadow.transform.localPosition;
                shadowPos.y = 0.01f;
            }
        }

        private void Update()
        {
            if(animator.GetCurrentAnimatorStateInfo(0).IsName("Attack")){
                animator.speed = animatorAttackSpeed;
            }else if(animator.GetCurrentAnimatorStateInfo(0).IsName("Walk")){
                animator.speed = moveSpeed/baseSpeed;
            }else{
                animator.speed = 1;
            }
            //set rotation
            Quaternion start = gameObject.transform.rotation;
            gameObject.transform.rotation = Quaternion.Lerp(start, forward, 0.1f);
            shadow.transform.rotation = Quaternion.Euler(90, 0, 0);
            shadow.transform.localPosition = shadowPos;

            //set damage or heal
			if(damageCacheTime>0){
                float delta = Mathf.Abs(damageCacheTime - 0.2f);
                foreach (var comp in gameObject.GetComponentsInChildren<SkinnedMeshRenderer>())
                {
                    comp.material.SetColor("_LightTint", new Color(1, 0.65f+0.35f*delta/0.2f, 0.65f + 0.35f * delta / 0.2f));
                 }
                //if (hpBar != null)
                //{
                //    if(side==2)
                //    hpBar.transform.Find("Mask").Find("Fill Area").Find("Fill").GetComponent<Image>().color = new Color(1, 0.8f - 0.24f * delta / 0.2f, 0.8f + 0.14f * delta / 0.2f);
                //    else hpBar.transform.Find("Mask").Find("Fill Area").Find("Fill").GetComponent<Image>().color = new Color(0.8f - 0.4f * delta / 0.2f,1, 0.8f - 0.4f * delta / 0.2f);
                //}
				damageCacheTime -= Time.deltaTime; 
            }else if(damageCacheTime<0){
				damageCacheTime = 0;
                foreach (var comp in gameObject.GetComponentsInChildren<SkinnedMeshRenderer>())
                {
                    comp.material.SetColor("_LightTint", new Color(1, 1, 1));
                }
                //if (hpBar != null) { 
                //    if(side == 2)
                //    hpBar.transform.Find("Fill Area").Find("Fill").GetComponent<Image>().color = new Color(1, 0.56f, 0.94f);
                //    else hpBar.transform.Find("Fill Area").Find("Fill").GetComponent<Image>().color = new Color(0.4f,1, 0.4f);
                //}
            }

        }
        public void UpdateHpBar(){
            if (hpBar != null && hpBar.activeSelf == true)
            {
                hpBarCacheTime += Time.deltaTime;
                if (hpBarCacheTime > 3)
                {
                    hpBar.SetActive(false);
                    hpBarCacheTime = 0;
                    return;
                }
                Canvas canvas = GameRoot.GetInstance().battleUI.GetComponent<Canvas>();
                CamaraManager camara = GameRoot.GetInstance().Camara.GetComponent<CamaraManager>();
                Vector2 screenPos = Camera.main.WorldToScreenPoint(GetSocketPos("S_Hp"));
                //Vector2 uiPos = Vector2.zero;
                //RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, screenPos, canvas.worldCamera, out uiPos);
                //uiPos.x = screenPos.x - (Screen.width / 2);
                //uiPos.y = screenPos.y - (Screen.height / 2);
                //hpBar.GetComponent<RectTransform>().position = new Vector3(0, 0, 0);
                hpBar.transform.position = new Vector3(screenPos.x, screenPos.y, 0);

                float scale = camara.minSize / camara.size;
                hpBar.transform.localScale = new Vector3(Mathf.Sqrt(radius) / 2, 1, 1)*scale*1.2f; ;
               
            }
        }

    }
}
