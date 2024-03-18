using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dropper : MonoBehaviour{
    public GameObject milkPrefab;
    public Transform dropMarker;
    public static int itemCount = 0;

    void Start(){
        InvokeRepeating("dropItem", 0, 1);
    }
    

    void dropItem(){
        if(itemCount > 0){
            GameObject milk = Instantiate(milkPrefab, dropMarker.position, milkPrefab.transform.rotation);
            itemCount--;
        }
    }
}
