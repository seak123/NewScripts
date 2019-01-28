using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Data;


public class GameDataManager
{
    //player data
    public float playerHp;
    public float magicAttack;
    public float defence;
    public float magicResist;
    public int mainCasterId;
    public List<int> playCards;


    //hero data
    public int heroId;
    public GameObject heroPrefab;
    public Sprite heroIcon;
    public int Skill1Lvl =1;
    public int Skill2Lvl =0;
    public int Skill3Lvl =0;
    //enhance data
    

    public void InitData(){
        playerHp = 3000;
        magicAttack = 1;
        defence = 100;
        magicResist = 0.5f;
        mainCasterId = 1011;
        playCards = new List<int>
            {
                0,
                1081,
                1091,
                1081,
                1091,
                5011,
                5011,
                1071,
        };
        //temp
        heroId = 10001;
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

    public CreatureData GetHeroData(){
        CreatureData data = new CreatureData
        {
            id = heroId,
            type = -1,
            opposite_type = 3,
            CreatureName = "月神阿尔忒弥斯",
            hp = 800,
            attack = 180,
            attack_rate = 0.6f,
            defence = 40,
            magic_resist = 0,
            crit = 0,
            crit_value = 1,
            hit_rate = 0,
            dodge = 0,
            speed = 24,
            base_speed = 12,
            physic_suck = 0,
            magic_suck = 0,
            coold_reduce = 0,
            radius = 4,
            attack_range = 160,
            channal = 0.5f,
            ready_time = 1,
            cost = 10,
            prefab = heroPrefab,
            skills = new int[0],
            Skill1Lvl = Skill1Lvl,
            Skill2Lvl = Skill2Lvl,
            Skill3Lvl = Skill3Lvl
        };
       
        return data;
    }

    public CardData GetHeroCardData(){

    CardData data = new CardData
        {
            cardId = heroId,
            cardName = "月神阿尔忒弥斯",
            cardType = CardType.Hero,
            race = 1,
            icon = heroIcon,
            entityPrefab = heroPrefab,
            skillId = 1,
            cost = 10,
            liveTime = -1,
            num = 1

    };
        return data;
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

    public int GetFieldId(){
        IDungeonUnit dungeon = GameRoot.GetInstance().DungeonMng.GetCurrDungeonData();
        FightDungeonData data = (dungeon as FightDungeon).GetFightData();
        return data.FieldId;
    }
       
}
