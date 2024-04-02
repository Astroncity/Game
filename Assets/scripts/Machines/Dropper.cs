using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Dropper : MonoBehaviour{
    public GameObject milkPrefab;
    public Transform dropMarker;
    public static int itemCount = 0;
    public static int lastMilkPrice = 0;

    public TextMeshProUGUI inventoryText;
    
    public bool active = true;

    void Start(){
        InvokeRepeating("dropItem", 0, 1);
    }

    void Update(){
        inventoryText.text = itemCount.ToString();
    }
    

    void dropItem(){
        if(itemCount > 0 && active){
            GameObject milk = Instantiate(milkPrefab, dropMarker.position, milkPrefab.transform.rotation);
            itemCount--;
            milk.GetComponent<ItemScript>().data.value = lastMilkPrice;
        }
    }
}
