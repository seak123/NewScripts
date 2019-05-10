using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Data
{

    public class UnitData
    {
        public int id = 0;

        public int level = 0;

        public int uid = 0;

        public string name = "admin";

        //side: 1,player 2,enemy
        public int side = 0;

        //type: 0,creature  1,construction
        public int type = 1;

        public int enemy_level = 0;

        public int con_type = 0;

        //genus: 1,ground 2,fly
        public int genus = 1;

        public int init_room = 0;

        public int init_x = 0;

        public int init_y = 0;


        //property
        public float hp = 0;

        public float attack = 0;

        public float base_attack_interval = 1;

        public float attack_rate = 0;

        public float defence = 0;

        public float magic_resist = 0;

        public float crit = 0;

        public float crit_value = 0;

        public float hit_rate = 0;

        public float dodge = 0;

        public float speed = 0 ;

        public float physic_suck =0;

        public float magic_suck =0;

        public float coold_reduce = 0;

        public int radius =0;

        public int block_num = 1;

        public float attack_range =0;

        public float channal = 0;

        public float ready_time = 1;

        public float live_time = -1;

        public int card_uid = -1;

        public int[] skills;
    }
}
