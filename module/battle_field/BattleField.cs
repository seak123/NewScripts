using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PowerInject;
using Data;

[Insert]
public class BattleField : MonoBehaviour {

    public AssetManager assetManager;
    public int battleRound;

    //public void AddCreature(int id,int side ,int x,int y){
    //    CreatureData data = assetManager.GetCreatureData(id);
    //    var unitdata = AssetManager.PackCreatureData(data);
    //    unitdata.side = side;
    //    unitdata.init_x = x;
    //    unitdata.init_y = y;
    //    GameRoot.GetInstance().Bridge.AddUnit(unitdata);
    //}

    [OnInjected]
    public void AddRootAction(){
        GameRoot.moduleInit += Init;
    }

    public void Init(){
        Debug.Log("BattleField Init");
        battleRound = 0;
    }

    public void CasterEnemy(){
        FightDungeon dungeon = GameRoot.GetInstance().DungeonMng.GetCurrDungeonData() as FightDungeon;
        FightDungeonData data = dungeon.GetFightData();
        string enemyCode = data.enemys[battleRound];

        List<string> line1 = new List<string>();
        List<string> line2 = new List<string>();
        List<string> line3 = new List<string>();

        string[] enemyArray = enemyCode.Split('|');
        foreach(var str in enemyArray){
            if(str[0]=='1'){
                line1.Add(str.Substring(1));
            }else if(str[0]=='2'){
                line2.Add(str.Substring(1));
            }else{
                line3.Add(str.Substring(1));
            }
        }
        CreateEnemy(line1, 700);
        CreateEnemy(line2, 740);
        CreateEnemy(line3, 780);
        battleRound++;
    }

    private void CreateEnemy(List<string> list,int posX){
        //GameRoot.GetInstance().Bridge.CasterSkill(1, cardData.skillId, cenX, cenY, unitData, cardData.num, sUid);
        int num = list.Count;
        if (num == 0) return;
        int interval = BattleDef.rowGridNum / (num+1);
        for (int i = 0; i < num;++i){
            int posY = (i + 1) * interval;
            UnitData data = AssetManager.PackCreatureData(GameRoot.GetInstance().BattleField.assetManager.GetCreatureData(int.Parse(list[i].Substring(1)) * 10 + 1));
            GameRoot.GetInstance().Bridge.CasterSkill(2, 1, posX, posY, data, int.Parse(list[i].Substring(0,1)), -1);
        }
    }
   
}
