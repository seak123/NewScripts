using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Map
{

    public class Entity : MonoBehaviour
    {
        public void SetTransform(float x,float y){
            gameObject.transform.position = new Vector3(x, 0, y);
        }
    }
}
