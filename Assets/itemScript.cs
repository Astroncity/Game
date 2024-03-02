using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class itemScript : MonoBehaviour
{
    // Start is called before the first frame update
    private bool canHold = false;
    private bool isHolding = false;
    private GameObject mainCamOBJ;
    private Camera mainCam;

    void Start() {
        mainCamOBJ = GameObject.Find("Main Camera");
        mainCam = mainCamOBJ.GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update() {
        checkMouseOn();
        hold();
    }

    void hold() {
        if (isHolding && Input.GetMouseButton(0)) {
            transform.position = mainCam.transform.position + mainCam.transform.forward * 2;
        } else {
            isHolding = false;
        }
    }

    void checkMouseOn(){
        RaycastHit hit;
        Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(ray, out hit)){
            if(hit.collider.gameObject == this.gameObject){
                if(Input.GetMouseButtonDown(0)){
                    canHold = true;
                }
                if(Input.GetMouseButtonUp(0)){
                    canHold = false;
                }
                if(canHold){
                    isHolding = true;
                }
            }
        }
    }
}
