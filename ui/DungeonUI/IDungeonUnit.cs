using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public interface IDungeonUnit {
    DungeonState GetState();
    Sprite GetDungeonSprite();
    void SetState(DungeonState _state);
    void OpenDungeon();
    void LeaveDungeon();
}
