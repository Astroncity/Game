using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;


public enum DropType{
    delivery,
    pickup,
    sell,
    storagePickup
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
            if(type == DropType.storagePickup) storagePickup();
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
        for(int i = 0; i < vehicle.items.Count; i++){
            if(vehicle.items[i].type.Equals("milk")){
                vehicle.items.RemoveAt(i);
                i--;
                Dropper.itemCount++;
            }
        }
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
        Drive vehicle = playerVehicle.GetComponent<Drive>();
        int cap = vehicle.Capacity - vehicle.items.Count;
        int itemsToAdd = Math.Min(cap, itemCount);
        itemCount -= itemsToAdd;
        for(int i = 0; i < itemsToAdd; i++){
            vehicle.addItem(new ItemData(initPrice, "milk", new List<Modifier>()));
        }
        if(itemCount == 0){
            active = false; 
            GetComponent<MeshRenderer>().enabled = false;
        }
    }


    void storagePickup(){
        Debug.Log("Loading...");
        Drive vehicle = playerVehicle.GetComponent<Drive>();
        int num = Math.Min(vehicle.Capacity - vehicle.items.Count, Storage.items.Count);
        for(int i = 0; i < num; i++){    
            vehicle.addItem(Storage.items.Pop());
        }
    }
}
