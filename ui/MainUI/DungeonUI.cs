using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public enum DungeonUIState{
    Idle = 1,
    Strategy = 2,
}

public class DungeonUI : MonoBehaviour {

    public GameObject background;
    public GameObject dungeonInfo;
    public ClickEvent clickEvent;
    public GameObject quit;
    public GameObject close;
    public GameObject card;

    private DungeonUIState state;
    private int currRoomId;

	// Use this for initialization
	void Start () {
        state = DungeonUIState.Idle;
        clickEvent.clickAction += OnClick;

        //init
        background.SetActive(false);
        dungeonInfo.GetComponent<RectTransform>().position = new Vector3(Screen.width/2, -Screen.height/2, 0);
        quit.GetComponent<RectTransform>().position = new Vector3(0, quit.GetComponent<RectTransform>().position.y,0);
        close.GetComponent<RectTransform>().position = new Vector3(Screen.width*4/3, quit.GetComponent<RectTransform>().position.y, 0);

    }

    public void OnClick(){
        if(state == DungeonUIState.Idle){
            Vector3 point;
            if (BattleDef.useMouse)
            {
                point = Input.mousePosition;
            }
            else
            {
                point = Input.touches[0].position;
            }
            Ray ray = Camera.main.ScreenPointToRay(point);
            RaycastHit hit;
            Physics.Raycast(ray, out hit);
            int gridX, gridY;
            GameRoot.GetInstance().MapField.GetGridPos(hit.point.x, hit.point.z, out gridX, out gridY);
            if (hit.collider != null)
            {
                Vector2Int id = GameRoot.GetInstance().MapField.GetRoomViewId(gridX, gridY);
                int key = id.x * 10 + id.y;
                if (key != 0)
                {
                    state = DungeonUIState.Strategy;
                    currRoomId = key;
                    OpenDungeonInfo();
                }
            }
        }
    }

    private void OpenDungeonInfo(){
        background.SetActive(true);
        GameRoot.GetInstance().CameraMng.active = false;
        quit.GetComponent<RectTransform>().DOMoveX(-Screen.width/3, 0.5f);
        dungeonInfo.GetComponent<RectTransform>().DOMoveY(0,0.5f);
        close.GetComponent<RectTransform>().DOMoveX(Screen.width, 0.5f);
        card.SetActive(true);

    }

    public void CloseInfo(){
        background.SetActive(false);
        GameRoot.GetInstance().CameraMng.active = true;
        dungeonInfo.GetComponent<RectTransform>().DOMoveY(-Screen.height / 2, 0.5f);
        quit.GetComponent<RectTransform>().DOMoveX(0, 0.5f);
        close.GetComponent<RectTransform>().DOMoveX(Screen.width * 4 / 3, 0.5f);

        state = DungeonUIState.Idle;
        currRoomId = -1;
        card.SetActive(false);
    }

    public void Quit()
    {
        GameRoot.GetInstance().QuitStrategy();
    }

    public void OpenPackage(){
        GameObject package = GameRoot.GetInstance().mainUIMng.OpenUI(16);
        PackageUI packageUI = package.GetComponent<PackageUI>();
        packageUI.Init(PackageType.IdleCreature,1);
        packageUI.SelectAction += ChangeGuard;
    }

    public void ChangeGuard(List<CreatureFightData> creatures){

    }
    // Update is called once per frame
    void Update () {
      
    }

}
