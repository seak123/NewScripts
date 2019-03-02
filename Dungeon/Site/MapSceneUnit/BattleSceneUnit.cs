using UnityEngine;
using System.Collections;



public class BattleSceneUnit:BaseSceneUnit
{
    private SceneType sceneType;

    override public void EnterScene(){
        GameRoot.GetInstance().StartBattle();
    }
}