using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;


public enum DropType{
    delivery,
    pickup,
    sell
}


public class DropPoint : MonoBehaviour
{
    public int itemCount;
    public bool active;

    [Header("Options")]
    public bool permanent;
    
    public DropType type;

    public GameObject playerVehicle;

    //if pickup
    private int initPrice;

    void Start(){
        GetComponent<MeshRenderer>().enabled = permanent;
    }

    public void OnTriggerEnter(Collider other){
        if((other.gameObject == playerVehicle) && active){
            if(type == DropType.pickup) fillVehicle();
            if(type == DropType.delivery) unloadVehicle();
            if(type == DropType.sell) sell();
        }
    }


    public void init(bool permanent, DropType type, int itemCount, int initPrice){
        this.permanent = permanent;
        this.type = type;   
        this.itemCount = itemCount;
        this.initPrice = initPrice;
        active = true;
    }


    void unloadVehicle(){
        Drive vehicle = playerVehicle.GetComponent<Drive>();
        Dropper.itemCount += vehicle.itemCount;
        vehicle.itemCount = 0;
    } 


    void sell(){
        Drive vehicle = playerVehicle.GetComponent<Drive>();
        int sellPrice = 0;
        foreach(ItemData item in vehicle.items){
            sellPrice += item.value;
        }
        PlayerHandler.money += sellPrice;
        vehicle.items.Clear();
    }


    void fillVehicle(){
        //fill the vehicle with items up to its capacity
        Drive vehicle = playerVehicle.GetComponent<Drive>();
        if(vehicle.itemCount + itemCount <= vehicle.Capacity){
            vehicle.itemCount += itemCount;
            itemCount = 0;
            active = false;
        }
        else{
            itemCount -= (int)vehicle.Capacity - vehicle.itemCount;
            vehicle.itemCount = (int)vehicle.Capacity;
        }
        GetComponent<MeshRenderer>().enabled = false;
        PlayerHandler.onMilkRun = false;

        for(int i = 0; i < itemCount; i++){
            vehicle.items.Add(new ItemData(initPrice, new List<Modifier>()));
        }
    }
}
