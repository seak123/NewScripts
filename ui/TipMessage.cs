using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TipMessage : MonoBehaviour {

    public GameObject CritLogo;

    public void SetLogo(int value){
        int num = 0;
        CritLogo.SetActive(true);
        while(value/10>0){
            value = value / 10;
            ++num;
        }
        CritLogo.GetComponent<RectTransform>().localPosition = new Vector3(7*num, 0, 0);
    }
}
