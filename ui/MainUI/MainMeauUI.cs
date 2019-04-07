using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISceneUI{
    void OnEnter();
}

public class MainMeauUI : MonoBehaviour,ISceneUI {

    public void OnEnter()
    {

    }

    public void NewGame()
    {
        GameRoot.GetInstance().StartNewGame();
    }

    public void LoadGame(){
        GameRoot.GetInstance().LoadGame();
    }
}
