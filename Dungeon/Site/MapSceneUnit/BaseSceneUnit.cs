using UnityEngine;
using System.Collections;

public enum SceneType{
    Battle = 1,
}

public class BaseSceneUnit
{
    private SceneType sceneType;

    virtual public void EnterScene(){
        
    }
}