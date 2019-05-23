using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public static class StrUtil  {

    private static Dictionary<string, string> texts;
    private static TextAsset textAsset;

    public static void Init(){
        texts = new Dictionary<string, string>();
        textAsset = Resources.Load("Language/English") as TextAsset;
        //read text
        string content = textAsset.text;
        string[] array = content.Split('\n');
        foreach(string str in array){
            if(str[0]=='-'){
                continue;
            }
            string[] txt = str.Split('@');
            //if (texts.ContainsKey(txt[0])) Debug.Log(txt[0]);
            texts.Add(txt[0], txt[1]);
        }
    }

    public static string GetText(string key){
        string res;
        if(BattleDef.language == "chinese"){
            res = key;
        }else{
            res = texts[key];
        }
        if(res==null){
            Debug.Log("Cannot Find Txt, key: " + key);
        }
        return res;
    }
}
