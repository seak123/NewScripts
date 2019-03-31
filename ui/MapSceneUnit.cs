using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapSceneUnit : MonoBehaviour {

    private BaseSceneUnit ownScene;

    public void EnterUnit(){
        if(ownScene!=null){
            ownScene.EnterScene();
        }
        GameRoot.GetInstance().StartBattle();
    }

    public void InjectScene(BaseSceneUnit _scene){
        ownScene = _scene;
    }
}
