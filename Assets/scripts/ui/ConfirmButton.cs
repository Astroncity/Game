using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum ActionType{
    YES,
    BACK
}

public class ConfirmButton : MonoBehaviour{
    private bool isHovering = false;
    public ActionType type;
    public Confirmation Confirmation;

    void Start(){
        
    }


    void Update(){
        IsPointerOverUIObject();

        if(isHovering){
            transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(1.1f, 1.1f, 1.1f), 10f * Time.deltaTime);
        }
        else{
            transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(1f, 1f, 1f), 10f * Time.deltaTime);
        }

        if(Input.GetMouseButtonDown(0) && isHovering){
            switch(type){
                case ActionType.YES:
                    Confirmation.yes();
                    break;
                case ActionType.BACK:
                    Confirmation.back();
                    break;
            }
        }
    }

    
    void IsPointerOverUIObject(){        
        RaycastHit hitInfo;
        if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo)){
            if(hitInfo.collider.gameObject.GetComponent<ConfirmButton>() != null){
                if(hitInfo.collider.gameObject.GetComponent<ConfirmButton>() == this){
                    isHovering = true;
                    return;
                }
            }
        }
        isHovering = false;
    }
}
