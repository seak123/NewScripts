using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using UnityEngine.UI;
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

    public GameObject removeBtn;
    public GameObject replaceBtn;
    public GameObject infoBtn;
    public GameObject dungeonInfoBack;

    public GameObject ConstructureContainer;
    public GameObject[] PartContainer;
    public Text[] IdleTxt;
    private float idleTime;

    public DungeonUIState state;
    private int currRoomId;
    private int currIndex;
    private Vector2Int currRoomCenter;
    private GameObject constructureIcon;
    private GameObject[] partIcons;
    private GameObject package;


    // 0 constructure 1 creature_1 2 creature_2 ..
    private int iconIndex;

	// Use this for initialization
	void Start () {
        idleTime = 0;
        state = DungeonUIState.Idle;
        clickEvent.clickAction += OnClick;
        partIcons = new GameObject[4];
        for (int i = 0; i < 4;++i){
            partIcons[i] = Instantiate(IconPrefab);
            partIcons[i].transform.parent = PartContainer[i].transform;
            switch(i){
                case 0:
                    partIcons[i].GetComponent<ClickEvent>().clickAction += () => {
                        ClickCreature(0);
                    };
                    break;
                case 1:
                    partIcons[i].GetComponent<ClickEvent>().clickAction += () => {
                        ClickCreature(1);
                    };
                    break;
                case 2:
                    partIcons[i].GetComponent<ClickEvent>().clickAction += () => {
                        ClickCreature(2);
                    };
                    break;
                case 3:
                    partIcons[i].GetComponent<ClickEvent>().clickAction += () => {
                        ClickCreature(3);
                    };
                    break;
            }

            partIcons[i].transform.localPosition = new Vector3(-2, 0, 0);
            partIcons[i].transform.localScale = Vector3.one;
            partIcons[i].SetActive(false);
        }
    }

    public void OnEnter(){
        //init
        background.SetActive(false);
        removeBtn.SetActive(false);
        replaceBtn.SetActive(false);
        infoBtn.SetActive(false);
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
                Debug.Log("center Pos:" + center);
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

        string roomName = StrUtil.GetText("开始布置地牢房间");
        int key_1 = currRoomId / 10;
        int key_2 = currRoomId - key_1 * 10;
        roomName += ((char)(62 + key_1)).ToString()+"-";
        if(key_2-3<0){
            roomName += (3 - key_2).ToString() + "-L";
        }else if(key_2-3>0){
            roomName += (key_2-3).ToString() + "-R";
        }else{
            roomName += "0";
        }
        if(key_1<3)roomName = StrUtil.GetText("开始布置BOSS房间");

        GameRoot.GetInstance().mainUIMng.PushMessage(roomName, SystemTipType.Tip);
        GameRoot.GetInstance().CameraMng.MoveClose(new Vector2((float)currRoomCenter.x / 25f, (float)currRoomCenter.y / 25f));
        quit.GetComponent<RectTransform>().DOMoveX(-Screen.width*2/5, 0.5f);
        dungeonInfo.GetComponent<RectTransform>().DOMoveY(0,0.5f);
        close.GetComponent<RectTransform>().DOMoveX(Screen.width, 0.5f);
        if(constructureIcon==null){
            constructureIcon = Instantiate(IconPrefab);
            constructureIcon.GetComponent<ClickEvent>().clickAction += ClickConstructure;
            constructureIcon.transform.parent = ConstructureContainer.transform;
            constructureIcon.transform.localPosition = Vector3.zero;
            constructureIcon.transform.localScale = Vector3.one;
        }
        if (package == null)
        {
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
        GameRoot.GetInstance().CameraMng.MoveRecover(this);
        dungeonInfo.GetComponent<RectTransform>().DOMoveY(-Screen.height / 2, 0.5f);
        quit.GetComponent<RectTransform>().DOMoveX(0, 0.5f);
        close.GetComponent<RectTransform>().DOMoveX(Screen.width * 4 / 3, 0.5f);

        currRoomId = -1;
        GameRoot.GetInstance().mainUIMng.CleanInfoUI();
        CleanUp();
    }

    public void Quit()
    {
        GameRoot.GetInstance().QuitStrategy();
    }

    // dungeon ui function

    public void OpenPackageConstructue(){

        PackageUI packageUI = package.GetComponentInChildren<PackageUI>();
        packageUI.Init(PackageType.IdleConstructure, 1);
        package.transform.DOMoveX(Screen.width / 2, 0.3f);
        packageUI.SelectAction += ChangeStructure;
    }

    public void OpenPackageCreature(int index){
        if (package == null)
        {
            package = Instantiate(packagePrefab);
            package.transform.parent = gameObject.transform;
            package.transform.localScale = Vector3.one;
            package.GetComponent<RectTransform>().position = new Vector3(Screen.width * 3 / 2, 0, 0);
        }
        CreatureFightData roomData = GameRoot.GetInstance().gameDataManager.GetInRoomConstructure(currRoomId);
        PackageUI packageUI = package.GetComponentInChildren<PackageUI>();
        if (roomData == null) return;
        switch(roomData.con_type){
            case 0:
                packageUI.Init(PackageType.IdleCreature, 1);
                package.transform.DOMoveX(Screen.width / 2, 0.3f);
                currIndex = index;
                packageUI.SelectAction += ChangeCreature;
                break;
            case 2:
                packageUI.Init(PackageType.IdlePartTool, 1);
                package.transform.DOMoveX(Screen.width / 2, 0.3f);
                currIndex = index;
                packageUI.SelectAction += ChangeCreature;
                break;
        }
    }

    public void ClickConstructure(){
        iconIndex = -1;
        float sizeOffset = Screen.width / 750f;
        Vector3 pos = ConstructureContainer.GetComponent<RectTransform>().position;
        removeBtn.SetActive(true);
        removeBtn.GetComponent<RectTransform>().position = pos;
        removeBtn.GetComponent<RectTransform>().DOMove(new Vector3(pos.x - 120 * sizeOffset, pos.y + 110 * sizeOffset, 0), 0.2f);
        replaceBtn.SetActive(true);
        replaceBtn.GetComponent<RectTransform>().position = pos;
        replaceBtn.GetComponent<RectTransform>().DOMove(new Vector3(pos.x, pos.y + 140 * sizeOffset, 0), 0.2f);
        infoBtn.SetActive(true);
        infoBtn.GetComponent<RectTransform>().position = pos;
        infoBtn.GetComponent<RectTransform>().DOMove(new Vector3(pos.x + 120 * sizeOffset, pos.y + 110 * sizeOffset, 0), 0.2f);
        dungeonInfoBack.SetActive(true);
    }

    public void ClickCreature(int index){
        iconIndex = index;
        currIndex = index;
        float sizeOffset = Screen.width / 750f;

        Vector3 pos = partIcons[index].GetComponent<RectTransform>().position;
        removeBtn.SetActive(true);
        removeBtn.GetComponent<RectTransform>().position = pos;
        removeBtn.GetComponent<RectTransform>().DOMove(new Vector3(pos.x - 120 * sizeOffset, pos.y + 110 * sizeOffset, 0), 0.2f);
        replaceBtn.SetActive(true);
        replaceBtn.GetComponent<RectTransform>().position = pos;
        replaceBtn.GetComponent<RectTransform>().DOMove(new Vector3(pos.x, pos.y + 140 * sizeOffset, 0), 0.2f);
        infoBtn.SetActive(true);
        infoBtn.GetComponent<RectTransform>().position = pos;
        infoBtn.GetComponent<RectTransform>().DOMove(new Vector3(pos.x + 120 * sizeOffset, pos.y + 110 * sizeOffset, 0), 0.2f);
        dungeonInfoBack.SetActive(true);
    }

    public void RemoveItem(){
        switch(iconIndex){
            case -1:
                RemoveStructure();
                break;
            default:
                RemoveCreature();
                break;
        }
    }

    public void ReplaceItem(){
        switch (iconIndex)
        {
            case -1:
                OpenPackageConstructue();
                break;
            default:
                OpenPackageCreature(iconIndex);
                break;
        }
    }

    public void InfoItem(){
        switch (iconIndex)
        {
            case -1:
                CreatureIconUI iconUI = constructureIcon.GetComponent<CreatureIconUI>() ;
                if(iconUI!=null){
                    CloseDungeonBtn();
                    iconUI.OpenDestination();
                }
                break;
            default:
                CreatureIconUI iconUI_2 = partIcons[iconIndex].GetComponent<CreatureIconUI>();
                if (iconUI_2 != null)
                {
                    CloseDungeonBtn();
                    iconUI_2.OpenDestination();
                }
                break;
        }
    }
    /// ///
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

    public void RemoveStructure(){
        GameDataManager mng = GameRoot.GetInstance().gameDataManager;
        mng.ChangeRoomConstructure(currRoomId, -1);
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

            mng.ChangeRoomSubData(currRoomId,currIndex,newCreature);
        }
        PackageUI packageUI = package.GetComponentInChildren<PackageUI>();
        packageUI.SelectAction -= ChangeCreature;

        RefreshInfo();
    }

    public void RemoveCreature(){
        GameDataManager mng = GameRoot.GetInstance().gameDataManager;
        mng.ChangeRoomSubData(currRoomId,currIndex, -1);
        RefreshInfo();
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

        List<CreatureFightData> datas = GameRoot.GetInstance().gameDataManager.GetInRoomSubData(currRoomId);
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
       CloseDungeonBtn();

    }

    public void CloseDungeonBtn(){
        removeBtn.SetActive(false);

        replaceBtn.SetActive(false);

        infoBtn.SetActive(false);
        dungeonInfoBack.SetActive(false);
    }

    public void CleanUp(){
        if(constructureIcon!=null){
            Destroy(constructureIcon);
        }
        if(package!=null){
            Destroy(package);
        }
        CloseDungeonBtn();
    }
    // Update is called once per frame
    void Update () {
        idleTime += Time.deltaTime;
        float offset = idleTime % 2;
        float v = 0.2f + Mathf.Abs((offset - 1)) * 0.6f;
        foreach (var txt in IdleTxt){
            txt.color = new Color(txt.color.r, txt.color.g, txt.color.b, v);

        }
    }

}
