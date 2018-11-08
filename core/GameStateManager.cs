using UnityEngine;
using System.Collections;
using PowerInject;

[Insert]
public class GameStateManager : MonoBehaviour
{

    void Start()
    {

    }
    [OnInjected]
    void AddRootAction(){
        GameRoot.init += Init;
    }
    public void Init(){

    }
    private void Update()
    {

    }

}
