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
        public int radius;
        public int posX = 0;
        public int posY = 0;
        public Animator animator;

        private int RouteUpdateFlag = 0;
        private float animatorAttackSpeed = 1f;
        private GameObject hpPrefab;
        private GameObject hpBar;
        private float hpBarCacheTime = 0;

        HeapNode currMapNode;

        private AssetManager mng;
        private TransformState state = TransformState.Straight;

        private void SetTransform(int gridX,int gridY){
            float x = 0f;
            float y = 0f;
            GameRoot.GetInstance().MapField.GetViewPos(gridX,gridY,out x,out y);
            gameObject.transform.position = new Vector3(x, 0, y);
        }
        // >>>>>>>>>>>>>>>>> Transform 
        public void SetRotation(int toX, int toY){
            Vector2 direct = new Vector2(toX - posX, toY - posY);
            float angle = Vector2.Angle(new Vector2(1, 0), direct);
            angle = direct.y<0? angle:-angle;
            Quaternion start = gameObject.transform.rotation;
            Quaternion end = Quaternion.Euler(0, angle, 0);
            gameObject.transform.rotation = Quaternion.Lerp(start,end,0.1f);
        }

        public Vector3 GetSocketPos(string name){
            return gameObject.transform.Find(name).gameObject.transform.position;
        }
     
        public void Move(int toX,int toY,float value,out int gridX,out int gridY,out float offset)
        {
            //init data

            MapField field = GameRoot.GetInstance().MapField;

            float toViewX, toViewY;
            field.GetViewPos(toX, toY, out toViewX, out toViewY);

            field.MarkMovable(posX, posY, radius, false);

            if (state == TransformState.Straight)
            {
                float nowViewX = gameObject.transform.position.x;
                float nowViewY = gameObject.transform.position.z;
                float factor = value/BattleDef.Transfer2GridFactor / Vector2.Distance(new Vector2(toViewX, toViewY), new Vector2(nowViewX, nowViewY));
                float nextViewX = Mathf.Max(0,Mathf.Min(BattleDef.columnGridNum-1,nowViewX + (toViewX - nowViewX) * factor));
                float nextViewY = Mathf.Max(0, Mathf.Min(BattleDef.rowGridNum - 1,nowViewY + (toViewY - nowViewY) * factor));
                field.GetGridPos(nextViewX, nextViewY, out gridX, out gridY);
                if (field.IsCanMove(gridX, gridY, radius))
                {
                    offset = 0;
                    gameObject.transform.position = new Vector3(nextViewX, 0f, nextViewY);
                    posX = gridX;
                    posY = gridY;
                    field.MarkMovable(posX, posY, radius, true);
                    return;
                }else{
                    state = TransformState.AStar;
                    currMapNode = field.GetAStarRoute(id, posX, posY, toX, toY);
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
                offset = value - (currMapNode.G - startG);
                gridX = currMapNode.X;
                gridY = currMapNode.Y;
                SetTransform(gridX, gridY);
                posX = gridX;
                posY = gridY;
                if(RouteUpdateFlag == BattleDef.aStarUpdateFrame){
                    state = TransformState.Straight;
                }
                field.MarkMovable(posX, posY, radius, true);
                return;
            }
            gridX = 0;
            gridY = 0;
            offset = 0;
        }
        // >>>>>>>>>>>>>>>>>>>> UI
        public void SetHp(float hp, float maxHp)
        {
            if (hpBar == null)
            {
                hpBar = Instantiate(hpPrefab);
                hpBar.GetComponent<RectTransform>().parent = GameRoot.GetInstance().battleUI.GetComponent<RectTransform>();
                hpBar.SetActive(false);
                GameRoot.GetInstance().Camara.GetComponent<CamaraManager>().UpdateUI += UpdateHpBar;
            }
            hpBar.SetActive(true);
            hpBar.GetComponent<Slider>().value = hp / maxHp;
            hpBarCacheTime = 0;
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

        public void Die(){
            if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
            {
                animator.SetTrigger("Break");
            }
            animator.SetTrigger("Die");
            GameRoot.GetInstance().MapField.RemoveEntity(this);
            Destroy(hpBar, 0.5f);
            Destroy(gameObject, 2f);
        }

        private void Start()
        {
            mng = GameRoot.GetInstance().BattleField.assetManager;
            //init hp bar
            if(side == 1){
                hpPrefab = GameRoot.GetInstance().BattleField.assetManager.GreenSlider;
            }else{
                hpPrefab = GameRoot.GetInstance().BattleField.assetManager.RedSlider;
            }
        }

        private void Update()
        {
            if(animator.GetCurrentAnimatorStateInfo(0).IsName("Attack")){
                animator.speed = animatorAttackSpeed;
            }else{
                animator.speed = 1;
            }
           
        }
        public void UpdateHpBar(){
            if (hpBar != null && hpBar.activeSelf == true)
            {
                hpBarCacheTime += Time.deltaTime;
                Canvas canvas = GameRoot.GetInstance().battleUI.GetComponent<Canvas>();
                Vector2 screenPos = Camera.main.WorldToScreenPoint(GetSocketPos("S_Hp"));
                Vector2 uiPos = Vector2.zero;
                //RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, screenPos, canvas.worldCamera, out uiPos);
                uiPos.x = screenPos.x - (Screen.width / 2);
                uiPos.y = screenPos.y - (Screen.height / 2);
                //hpBar.GetComponent<RectTransform>().position = new Vector3(0, 0, 0);
                hpBar.transform.position = new Vector3(screenPos.x, screenPos.y, 0);

                if (hpBarCacheTime > 3)
                {
                    hpBar.SetActive(false);
                    hpBarCacheTime = 0;
                }
            }
        }

    }
}
