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
    private List<BaseSceneUnit> sceneUnits;
    public static MapSite SiteCreator(){
        MapSite newSite = null;
        return newSite;
    }
}
