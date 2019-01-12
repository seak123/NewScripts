using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Data;


public class GameDataManager
{
    //player data
    float playerHp;
    float magicAttack;
    float defence;
    float magicResist;
    int mainCasterId;
    public List<int> playCards;


    //hero data
    //enhance data

    public void InitData(){
        playerHp = 3000;
        magicAttack = 1;
        defence = 100;
        magicResist = 0.5f;
        mainCasterId = 101;
        playCards = new List<int>
            {
                1081,
                1091,
                1081,
                1091,
                5011,
                5011,
                7,
        };
    }

    public PlayerData GetPlayerData(){
        PlayerData res = new PlayerData
        {
            hp = playerHp,
            attack = magicAttack,
            denfence = defence,
            magic_resist = magicResist,
            cards = new List<int>()
        };
        UnitData mainCastle = AssetManager.PackCreatureData(GameRoot.GetInstance().BattleField.assetManager.GetCreatureData(mainCasterId));
        res.mainCastle = mainCastle;
        for (int i = 0; i < playCards.Count; ++i)
        {
            res.cards.Add(playCards[i]);
        }
        return res;
    }

    public PlayerData GetEnemyData(){
        IDungeonUnit dungeon = GameRoot.GetInstance().DungeonMng.GetCurrDungeonData();
        FightDungeonData data = (dungeon as FightDungeon).GetFightData();
        PlayerData res = new PlayerData
        {
            hp = data.mainCastleHp,
        };
        UnitData mainCastle = AssetManager.PackCreatureData(GameRoot.GetInstance().BattleField.assetManager.GetCreatureData(data.mainCastleId));
        res.mainCastle = mainCastle;
        res.cards = new List<int>();
        for (int i = 0; i < data.enemyCards.Length;++i){
            res.cards.Add(data.enemyCards[i]);
        }
        return res;
    }
       
}
