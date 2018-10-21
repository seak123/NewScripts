using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Map
{
    public enum TransformState{
        Straight = 1,
        AStar = 2
    }

    public class Entity : MonoBehaviour
    {
        public int id;
        public int uid;
        public int radius;
        private int RouteUpdateFlag = 0;
        private int posX = 0;
        private int posY = 0;

        private int toX = 0;
        private int toY = 0;
        private float toViewX = 0;
        private float toViewY = 0;

        HeapNode currMapNode;

        private AssetManager mng;
        private TransformState state = TransformState.Straight;

        private void SetTransform(int gridX,int gridY){
            float x = 0f;
            float y = 0f;
            GameRoot.GetInstance().MapField.GetViewPos(gridX,gridY,out x,out y);
            gameObject.transform.position = new Vector3(x, 0, y);
        }

        public void GoForward(int s_x,int s_y,int e_x,int e_y){
            RouteUpdateFlag = 0;
            toX = e_x;
            toY = e_y;
            GameRoot.GetInstance().MapField.GetViewPos(toX, toY, out toViewX, out toViewY);
            //currMapNode = GameRoot.GetInstance().MapField.GetAStarRoute(id, s_x, s_y, e_x, e_y);
        }

        public void Move(float value,out int gridX,out int gridY,out float offset)
        {
            /*MapField field = GameRoot.GetInstance().MapField;
            int startX = currMapNode.X;
            int startY = currMapNode.Y;
            while(field.Distance(currMapNode.Next.X,currMapNode.Next.Y,startX,startY)<value){
                currMapNode = currMapNode.Next;
            }
            offset = value - field.Distance(currMapNode.X, currMapNode.Y, startX, startY);
            gridX = currMapNode.X;
            gridY = currMapNode.Y;
            SetTransform(gridX, gridY);
            posX = gridX;
            posY = gridY;*/

            MapField field = GameRoot.GetInstance().MapField;
            //int radius = GameRoot.GetInstance().BattleField.assetManager.GetCreatureData(id).radius;
            //int radius = 4;
            field.MarkMovable(posX, posY, radius, false);

            if (state == TransformState.Straight)
            {
                float nowViewX = gameObject.transform.position.x;
                float nowViewY = gameObject.transform.position.z;
                float factor = value/BattleDef.Transfer2GridFactor / Vector2.Distance(new Vector2(toViewX, toViewY), new Vector2(nowViewX, nowViewY));
                float nextViewX = nowViewX + (toViewX - nowViewX) * factor;
                float nextViewY = nowViewY + (toViewY - nowViewY) * factor;
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
                while ((currMapNode.Next.G - startG) <= value)
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

        private void Start()
        {
            mng = GameRoot.GetInstance().BattleField.assetManager;
        }

    }
}
