using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemScript : MonoBehaviour{
    public bool isHolding = false;

    private GameObject mainCamOBJ;
    private Camera mainCam;


    public static int count = 0;


    void Start(){
        count++;
        mainCamOBJ = GameObject.Find("Main Camera");
        mainCam = Camera.main;
    }


    void Update(){
        if(PlayerHandler.inBuildMode){
            return;
        }
        if(mainCamOBJ == null){
            mainCamOBJ = Camera.main.gameObject;
            return;
        }

        checkMouseOn();
        hold();

    }

    void hold(){
        if (isHolding && Input.GetMouseButton(0)){
            transform.position = mainCam.transform.position + mainCam.transform.forward * 2;
        } 
        else{
            isHolding = false;
            PlayerHandler.holdingItem = false;
        }
    }


    void checkMouseOn(){
        RaycastHit hit;
        Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(ray, out hit)){
            if(hit.collider.gameObject == this.gameObject){
                PlayerHandler.canHoldItem = true;
                isHolding = true; PlayerHandler.holdingItem = true;
            }
            else{
                PlayerHandler.canHoldItem = false;
            }
        }
    }
}
