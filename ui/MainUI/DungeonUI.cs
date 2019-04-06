using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using Data;

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
    public GameObject packagePrefab;
    public GameObject IconPrefab;

    public GameObject ConstructureContainer;

    private DungeonUIState state;
    private int currRoomId;
    private GameObject constructureIcon;
    private GameObject package;
    private GameObject[] creatureIcon;

	// Use this for initialization
	void Start () {
        state = DungeonUIState.Idle;
        clickEvent.clickAction += OnClick;

        //init
        background.SetActive(false);
        dungeonInfo.GetComponent<RectTransform>().position = new Vector3(Screen.width/2, -Screen.height/2, 0);
        quit.GetComponent<RectTransform>().position = new Vector3(0, quit.GetComponent<RectTransform>().position.y,0);
        close.GetComponent<RectTransform>().position = new Vector3(Screen.width*7/5, quit.GetComponent<RectTransform>().position.y, 0);

        package = Instantiate(packagePrefab);
        package.transform.parent = gameObject.transform;
        package.transform.localScale = Vector3.one;
        package.GetComponent<RectTransform>().position = new Vector3(Screen.width * 3 / 2, 0, 0);


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
        quit.GetComponent<RectTransform>().DOMoveX(-Screen.width*2/5, 0.5f);
        dungeonInfo.GetComponent<RectTransform>().DOMoveY(0,0.5f);
        close.GetComponent<RectTransform>().DOMoveX(Screen.width, 0.5f);
        if(constructureIcon==null){
            constructureIcon = Instantiate(IconPrefab);
            constructureIcon.transform.parent = ConstructureContainer.transform;
            constructureIcon.transform.localPosition = Vector3.zero;
            constructureIcon.transform.localScale = Vector3.one;
        }
        //card.SetActive(true);
        CreatureFightData data = GameRoot.GetInstance().gameDataManager.GetInRoomConstructure(currRoomId);
        if(data == null){
            constructureIcon.SetActive(false);
        }else{
            constructureIcon.GetComponent<CreatureIconUI>().InjectData(data);
            constructureIcon.SetActive(true);
        }
    }

    public void CloseInfo(){
        background.SetActive(false);
        GameRoot.GetInstance().CameraMng.active = true;
        dungeonInfo.GetComponent<RectTransform>().DOMoveY(-Screen.height / 2, 0.5f);
        quit.GetComponent<RectTransform>().DOMoveX(0, 0.5f);
        close.GetComponent<RectTransform>().DOMoveX(Screen.width * 4 / 3, 0.5f);

        state = DungeonUIState.Idle;
        currRoomId = -1;
        GameRoot.GetInstance().mainUIMng.CleanInfoUI();
    }

    public void Quit()
    {
        GameRoot.GetInstance().QuitStrategy();
    }

    public void OpenPackage(){
       //GameObject package = Instantiate(packagePrefab);
        package.gameObject.transform.parent = gameObject.transform;
        package.gameObject.transform.localPosition = Vector3.zero;
        PackageUI packageUI = package.GetComponent<PackageUI>();
        packageUI.Init(700,PackageType.AllCreature,1);
        packageUI.SelectAction += ChangeGuard;
    }

    public void OpenPackageConstructue(){
        PackageUI packageUI = package.GetComponentInChildren<PackageUI>();
        packageUI.Init(700, PackageType.IdleConstructure, 1);
        package.transform.DOMoveX(Screen.width / 2, 0.5f);
        packageUI.SelectAction += ChangeStructure;
    }

    public void ChangeStructure(List<int> list){
        GameDataManager mng = GameRoot.GetInstance().gameDataManager;
        package.transform.DOMoveX(Screen.width * 3 / 2, 0.5f);
        int newCreature = list[0];
        //data change
        mng.ChangeRoomConstructure(currRoomId, newCreature);
        PackageUI packageUI = package.GetComponentInChildren<PackageUI>();
        packageUI.SelectAction -= ChangeStructure;

    }

    public void ChangeGuard(List<int> creatures){

    }
    // Update is called once per frame
    void Update () {
      
    }

}
