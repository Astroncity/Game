using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class DropPoint : MonoBehaviour
{
    public int itemCount;
    public bool active;

    [Header("Options")]
    public bool permanent;
    public bool delivery;
    public bool pickup;

    public GameObject playerVehicle;

    void Start(){
        GetComponent<MeshRenderer>().enabled = permanent;
    }

    public void OnTriggerEnter(Collider other){
        if((other.gameObject == playerVehicle) && active){
            if(pickup) fillVehicle();
            if(delivery) unloadVehicle();
        }
    }


    public void init(bool permanent, bool delivery, int itemCount){
        this.permanent = permanent;
        this.delivery = delivery;
        this.pickup = !this.delivery;
        this.itemCount = itemCount;
        active = true;
    }


    void unloadVehicle(){
        Dropper.itemCount += playerVehicle.GetComponent<Drive>().itemCount;
        playerVehicle.GetComponent<Drive>().itemCount = 0;
    } 


    void fillVehicle(){
        //fill the vehicle with items up to its capacity
        if(playerVehicle.GetComponent<Drive>().itemCount + itemCount <= playerVehicle.GetComponent<Drive>().Capacity){
            playerVehicle.GetComponent<Drive>().itemCount += itemCount;
            itemCount = 0;
            active = false;
        }
        else{
            itemCount -= (int)playerVehicle.GetComponent<Drive>().Capacity - playerVehicle.GetComponent<Drive>().itemCount;
            playerVehicle.GetComponent<Drive>().itemCount = (int)playerVehicle.GetComponent<Drive>().Capacity;
        }
        GetComponent<MeshRenderer>().enabled = false;
        PlayerHandler.onMilkRun = false;
    }
}
