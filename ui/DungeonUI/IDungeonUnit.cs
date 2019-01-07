using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public interface IDungeonUnit {
    Sprite GetDungeonSprite();
    void OpenDungeon(DungeonState state);
    void LeaveDungeon();
}
