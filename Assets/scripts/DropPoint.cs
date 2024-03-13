using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropPoint : MonoBehaviour
{
    public uint itemCount;
    public bool active;

    public GameObject playerVehicle;

    public void OnTriggerEnter(Collider other){
        if(other.gameObject == playerVehicle && active){
            fill();
        }
    }


    void fill(){
        //fill the vehicle with items up to its capacity
        if(playerVehicle.GetComponent<Drive>().itemCount + itemCount <= playerVehicle.GetComponent<Drive>().Capacity){
            playerVehicle.GetComponent<Drive>().itemCount += itemCount;
            itemCount = 0;
            active = false;
        }
        else{
            itemCount -= playerVehicle.GetComponent<Drive>().Capacity - playerVehicle.GetComponent<Drive>().itemCount;
            playerVehicle.GetComponent<Drive>().itemCount = playerVehicle.GetComponent<Drive>().Capacity;
        }
        GetComponent<MeshRenderer>().enabled = false;
    }
}
