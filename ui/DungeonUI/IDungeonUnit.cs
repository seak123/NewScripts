using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public interface IDungeonUnit {
    DungeonState GetState();
    Sprite GetDungeonSprite();
    bool GetVisiable();
    Vector2Int GetPos();
    void SetPos(Vector2Int pos);
    void SetVisiable(bool flag);
    void SetViewUnit(DungeonUnit view);
    void SetState(DungeonState _state);
    void OpenDungeon();
    void LeaveDungeon();
}
