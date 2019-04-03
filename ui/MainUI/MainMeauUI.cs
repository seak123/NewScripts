using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMeauUI : MonoBehaviour {

    public void NewGame()
    {
        GameRoot.GetInstance().StartNewGame();
    }

    public void LoadGame(){
        GameRoot.GetInstance().LoadGame();
    }
}
