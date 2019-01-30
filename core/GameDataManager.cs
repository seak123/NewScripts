using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Data;

public enum PlayerProperty{
    PlayerHp = 1,
    MagicAttack = 2,
    Defence = 3,
    MagicResist = 4,
    HeroHp = 10,
    HeroAttack = 11,
}


public class GameDataManager
{
    //property
    public float[] properties;

    public int castleId;
    public int heroId;
    public List<int> playCards;


    //hero data
    public int heroLvl = 0
    public GameObject heroPrefab;
    public Sprite heroIcon;
    public int Skill1Lvl =1;
    public int Skill2Lvl =0;
    public int Skill3Lvl =0;
    public int Skill4Lvl =0;
    public int Skill5Lvl = 0;
    //enhance data
    public int Hero1Lvl =0;
    public int Hero2Lvl = 0;
    public int Hero3Lvl = 0;


    private void RefreshEnhance(){
        switch(Hero1Lvl){
            case 1:
                break;
            case 2:
                break;
        }
    }

    public void InitData(){
        //playerHp = 3000;
        //magicAttack = 1;
        //defence = 100;
        //magicResist = 0.5f;
        //mainCasterId = 1011;
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
            hp = properties[0],
            attack = properties[1],
            denfence = properties[2],
            magic_resist = properties[3],
            cards = new List<int>()
        };
        UnitData mainCastle = AssetManager.PackCreatureData(GameRoot.GetInstance().BattleField.assetManager.GetCreatureData(castleId));
        res.mainCastle = mainCastle;
        for (int i = 0; i < playCards.Count; ++i)
        {
            res.cards.Add(playCards[i]);
        }
        return res;
    }

    public CreatureData GetHeroData(){
        AssetManager assetManager = GameRoot.GetInstance().field.assetManager;
        HeroData heroData = assetManager.GetHeroData(heroId);
        CreatureData data = new CreatureData
        {
            id = heroId,
            type = -1,
            opposite_type = heroData.opposite_type,
            CreatureName = heroData.creatureName,
            hp = heroData.base_hp + heroLvl * heroData.add_hp + properties[(int)PlayerProperty.HeroHp],
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
        AssetManager assetManager = GameRoot.GetInstance().field.assetManager;
        HeroData heroData = assetManager.GetHeroData(heroId);
        CardData data = new CardData
        {
            cardId = heroId,
            cardName = heroData.card_name,
            cardType = CardType.Hero,
            race = heroData.race,
            icon = heroData.icon,
            entityPrefab = heroData.heroPrefab,
            skillId = 1,
            cost = 30 + heroLvl*5,
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
