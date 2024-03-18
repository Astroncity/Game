using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class itemButton : MonoBehaviour{
    public int addAmount;
    private bool isHovering = false;
    public GameObject company;
    
    void Start(){
        
    }


    void Update(){
        IsPointerOverUIObject();

        if(isHovering){
            if(Input.GetMouseButtonDown(0)){
                company.GetComponent<CompanyScript>().addItem(addAmount);
            }
        }
    }

    
    void IsPointerOverUIObject(){     
        if(Camera.main == null) return; //* temporary fix for player being in build mode
        RaycastHit hitInfo;
        if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo)){
            if(hitInfo.collider.gameObject.GetComponent<itemButton>() != null){
                if(hitInfo.collider.gameObject.GetComponent<itemButton>() == this){
                    isHovering = true;
                    return;
                }
            }
        }
        isHovering = false;
    }
}
