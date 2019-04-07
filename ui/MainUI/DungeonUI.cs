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

public class DungeonUI : MonoBehaviour,ISceneUI {

    public GameObject background;
    public GameObject dungeonInfo;
    public ClickEvent clickEvent;
    public GameObject quit;
    public GameObject close;
    public GameObject packagePrefab;
    public GameObject IconPrefab;

    public GameObject ConstructureContainer;
    public GameObject[] PartContainer;

    private DungeonUIState state;
    private int currRoomId;
    private Vector2Int currRoomCenter;
    private GameObject constructureIcon;
    private GameObject[] partIcons;
    private GameObject package;

	// Use this for initialization
	void Start () {
        state = DungeonUIState.Idle;
        clickEvent.clickAction += OnClick;
        partIcons = new GameObject[4];
        for (int i = 0; i < 4;++i){
            partIcons[i] = Instantiate(IconPrefab);
            partIcons[i].transform.position = new Vector3(-2, 0, 0);
            partIcons[i].transform.parent = PartContainer[i].transform;
            partIcons[i].transform.localScale = Vector3.one;
            partIcons[i].SetActive(false);
        }
    }

    public void OnEnter(){
        //init
        background.SetActive(false);
        dungeonInfo.GetComponent<RectTransform>().position = new Vector3(Screen.width / 2, -Screen.height / 2, 0);
        quit.GetComponent<RectTransform>().position = new Vector3(-Screen.width*2/5, quit.GetComponent<RectTransform>().position.y, 0);
        quit.GetComponent<RectTransform>().DOMoveX(0, 0.5f);
        close.GetComponent<RectTransform>().position = new Vector3(Screen.width * 7 / 5, quit.GetComponent<RectTransform>().position.y, 0);
      
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
                Vector2Int center = GameRoot.GetInstance().MapField.GetRoomCenter(id.x, id.y);
                int key = id.x * 10 + id.y;
                if (key != 0)
                {
                    state = DungeonUIState.Strategy;
                    currRoomId = key;
                    currRoomCenter = center;
                    OpenDungeonInfo();
                }
            }
        }
    }

    private void OpenDungeonInfo(){
        background.SetActive(true);
        GameRoot.GetInstance().CameraMng.MoveClose(new Vector2(currRoomCenter.x / 25, currRoomCenter.y / 25));
        quit.GetComponent<RectTransform>().DOMoveX(-Screen.width*2/5, 0.5f);
        dungeonInfo.GetComponent<RectTransform>().DOMoveY(0,0.5f);
        close.GetComponent<RectTransform>().DOMoveX(Screen.width, 0.5f);
        if(constructureIcon==null){
            constructureIcon = Instantiate(IconPrefab);
            constructureIcon.transform.parent = ConstructureContainer.transform;
            constructureIcon.transform.localPosition = Vector3.zero;
            constructureIcon.transform.localScale = Vector3.one;
        }
        if(package == null){
            package = Instantiate(packagePrefab);
            package.transform.parent = gameObject.transform;
            package.transform.localScale = Vector3.one;
            package.GetComponent<RectTransform>().position = new Vector3(Screen.width * 3 / 2, 0, 0);
        }
        //card.SetActive(true);
        RefreshInfo();
    }

    public void CloseInfo(){
        if (GameRoot.GetInstance().CameraMng.closing == true) return;
        background.SetActive(false);
        GameRoot.GetInstance().CameraMng.MoveRecover();
        dungeonInfo.GetComponent<RectTransform>().DOMoveY(-Screen.height / 2, 0.5f);
        quit.GetComponent<RectTransform>().DOMoveX(0, 0.5f);
        close.GetComponent<RectTransform>().DOMoveX(Screen.width * 4 / 3, 0.5f);
        package.transform.DOMoveX(Screen.width * 3 / 2, 0.3f);

        state = DungeonUIState.Idle;
        currRoomId = -1;
        GameRoot.GetInstance().mainUIMng.CleanInfoUI();
        CleanUp();
    }

    public void Quit()
    {
        GameRoot.GetInstance().QuitStrategy();
    }

    public void OpenPackageConstructue(){
        PackageUI packageUI = package.GetComponentInChildren<PackageUI>();
        packageUI.Init(PackageType.IdleConstructure, 1);
        package.transform.DOMoveX(Screen.width / 2, 0.3f);
        packageUI.SelectAction += ChangeStructure;
    }

    public void OpenPackageCreature(){
        PackageUI packageUI = package.GetComponentInChildren<PackageUI>();
        packageUI.Init(PackageType.IdleCreature, 1);
        package.transform.DOMoveX(Screen.width / 2, 0.3f);
        packageUI.SelectAction += ChangeCreature;
    }

    public void ChangeStructure(List<int> list){
        GameDataManager mng = GameRoot.GetInstance().gameDataManager;
        package.transform.DOMoveX(Screen.width * 3 / 2, 0.3f);
        int newCreature;
        if (list.Count > 0)
        {
            newCreature = list[0];
            //data change

            mng.ChangeRoomConstructure(currRoomId, newCreature);
        }
        PackageUI packageUI = package.GetComponentInChildren<PackageUI>();
        packageUI.SelectAction -= ChangeStructure;
        RefreshInfo();

    }

    public void ChangeCreature(List<int> list){
        GameDataManager mng = GameRoot.GetInstance().gameDataManager;
        package.transform.DOMoveX(Screen.width * 3 / 2, 0.3f);
        int newCreature;
        if (list.Count > 0)
        {
            newCreature = list[0];
            //data change

            mng.ChangeRoomConstructure(currRoomId, newCreature);
        }
        PackageUI packageUI = package.GetComponentInChildren<PackageUI>();
        packageUI.SelectAction -= ChangeCreature;
        RefreshInfo();
    }

    public void ChangeGuard(List<int> creatures){

    }

    public void RefreshInfo(){
        CreatureFightData data = GameRoot.GetInstance().gameDataManager.GetInRoomConstructure(currRoomId);
        int num = 0;
        if (data == null)
        {
            constructureIcon.SetActive(false);
        }
        else
        {
            constructureIcon.GetComponent<CreatureIconUI>().InjectData(data);
            constructureIcon.SetActive(true);
        }

        List<CreatureFightData> datas = GameRoot.GetInstance().gameDataManager.GetInRoomCreature(currRoomId);
        if(data != null)
            num = data.contain_num;
        foreach(var obj in PartContainer){
            obj.SetActive(false);
        }
        float start_x = 0;
        switch(num){
            case 1:
                start_x = 0;
                break;
            case 2:
                start_x = -80;
                break;
            case 3:
                start_x = -150;
                break;
            case 4:
                start_x = -235;
                break;
        }
        for (int i = 0; i < num;++i){
            PartContainer[i].SetActive(true);
            PartContainer[i].transform.localPosition = new Vector3(start_x + i * 150, PartContainer[i].transform.localPosition.y, PartContainer[i].transform.localPosition.z);
            if(i<datas.Count){
                partIcons[i].GetComponent<CreatureIconUI>().InjectData(datas[i]);
                partIcons[i].SetActive(true);
            }else{
                partIcons[i].SetActive(false);
            }
        }

    }

    public void CleanUp(){
        if(constructureIcon!=null){
            Destroy(constructureIcon);
        }
        if(package!=null){
            Destroy(package);
        }
    }
    // Update is called once per frame
    void Update () {
      
    }

}
