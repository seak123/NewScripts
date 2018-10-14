using UnityEngine;
using System.Collections;
using PowerInject;

[Insert]
public class GameStateManager : MonoBehaviour
{
    // Use this for initialization
    void Start()
    {

    }
    [OnInjected]
    void AddRootAction(){
        GameRoot.init += Init;
    }
    public void Init(){

    }

}
