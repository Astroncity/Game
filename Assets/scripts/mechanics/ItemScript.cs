using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemScript : MonoBehaviour{
    public bool isHolding = false;
    public static int count = 0; //for UI display

    private GameObject mainCamOBJ;
    private Camera mainCam;

    public TextMeshProUGUI valueText;
    public Canvas infoCanvas;

    public ItemData data;
    public int basePrice;

    private static Dictionary<Modifier, double> modifierMultipliers = new Dictionary<Modifier, double>{
        {Modifier.bronze, 1.6},
        {Modifier.silver, 1.9},
        {Modifier.gold, 2.3},
        {Modifier.diamond, 3},
    };

    void Start(){

        basePrice = data.value;
        count++;
        mainCamOBJ = GameObject.Find("Main Camera");
        mainCam = Camera.main;
    }


    public int calcPrice(){
        double tempPrice = basePrice;
        foreach(Modifier mod in data.modifiers){
            tempPrice *= modifierMultipliers[mod];
        }
        return (int)tempPrice;
    }

    void Update(){
        data.value = calcPrice();
        if(PlayerHandler.inBuildMode){  
            return;
        }
        if(mainCamOBJ == null){
            mainCamOBJ = Camera.main.gameObject;
            mainCam = Camera.main;
            return;
        }

        checkMouseOn();
        hold();
        handleUI();
    }


    void handleUI(){
        valueText.text = data.value.ToString() + "<color=yellow>g";
        infoCanvas.transform.rotation = Quaternion.LookRotation(infoCanvas.transform.position - Camera.main.transform.position);
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
