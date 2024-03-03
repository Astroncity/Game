using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class itemScript : MonoBehaviour
{
    // Start is called before the first frame update
    private bool canHold = false;
    public bool isHolding = false;
    private GameObject mainCamOBJ;
    private Camera mainCam;
    private Image Crosshair;

    private Color canHoldColor;
    private Color isHoldingColor;

    private Color defaultColor;
    private Vector3 defaultScale;
    private Vector3 activeScale;   

    public playerHandler player;

    public static int count = 0;


    void Start() {
        count++;
        mainCamOBJ = GameObject.Find("Main Camera");
        mainCam = mainCamOBJ.GetComponent<Camera>();
        player = GameObject.Find("Player").GetComponent<playerHandler>();
        Crosshair = GameObject.Find("Crosshair").GetComponent<Image>();
        defaultColor = Crosshair.color;
        Debug.Log("DEFAULT: " + defaultColor);

        canHoldColor = createColor(255, 209, 0, 255);
        isHoldingColor = createColor(255, 165, 167, 255);
        defaultScale = Crosshair.transform.localScale;
        activeScale = defaultScale * 1.5f;
    }

    // Update is called once per frame
    void Update() {
        if(player.inBuildMode){
            return;
        }
        if(mainCamOBJ == null){
            mainCamOBJ = GameObject.Find("Main Camera");
            mainCam = mainCamOBJ.GetComponent<Camera>();
            return;
        }
        checkMouseOn();
        hold();
        if(isHolding){
            Crosshair.color = Color.Lerp(Crosshair.color, isHoldingColor, 10f * Time.deltaTime);
            Crosshair.transform.localScale = Vector3.Lerp(Crosshair.transform.localScale, activeScale, 10f * Time.deltaTime);
        }
        else if(canHold){
            Crosshair.color = Color.Lerp(Crosshair.color, canHoldColor, 10f * Time.deltaTime);
            Crosshair.transform.localScale = Vector3.Lerp(Crosshair.transform.localScale, activeScale, 10f * Time.deltaTime);
        }
        else{
            Crosshair.color = Color.Lerp(Crosshair.color, defaultColor, 10f * Time.deltaTime);
            Crosshair.transform.localScale = Vector3.Lerp(Crosshair.transform.localScale, defaultScale, 10f * Time.deltaTime);
        }
    }

    void hold() {
        if (isHolding && Input.GetMouseButton(0)) {
            transform.position = mainCam.transform.position + mainCam.transform.forward * 2;
        } else {
            isHolding = false;
        }
    }


    public static Color createColor(float r, float g, float b, float a){
        return new Color(r / 255, g / 255, b / 255, a / 255);
    }

    void checkMouseOn(){
        RaycastHit hit;
        Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(ray, out hit)){
            if(hit.collider.gameObject == this.gameObject){
                canHold = true;
                isHolding = true;
            }
            else{
                canHold = false;
            }
        }
    }
}
