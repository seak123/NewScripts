using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum SiteType{
    Wilds = 4,
    City = 5,
    Village = 6,
}

public class MapSite
{
    public List<BaseSceneUnit> sceneUnits;
    public MapSite(){
        sceneUnits = new List<BaseSceneUnit>();
    }
    public static MapSite SiteCreator(SiteType type){
        MapSite newSite = null;
        switch(type){
            case SiteType.Wilds:
                newSite = new MapSite();
                newSite.sceneUnits.Add(new BaseSceneUnit());
                newSite.sceneUnits.Add(new BaseSceneUnit());
                newSite.sceneUnits.Add(new BaseSceneUnit());
                break;
        }
        return newSite;
    }
}
