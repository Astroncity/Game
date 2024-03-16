using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropPoint : MonoBehaviour
{
    public int itemCount;
    public bool active;

    public GameObject playerVehicle;
    private GameObject player;

    void Start(){
        player = GameObject.Find("Player");
    }

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
            itemCount -= (int)playerVehicle.GetComponent<Drive>().Capacity - playerVehicle.GetComponent<Drive>().itemCount;
            playerVehicle.GetComponent<Drive>().itemCount = (int)playerVehicle.GetComponent<Drive>().Capacity;
        }
        GetComponent<MeshRenderer>().enabled = false;
        player.GetComponent<PlayerHandler>().onMilkRun = false;
    }
}
