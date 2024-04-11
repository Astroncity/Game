using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.Intrinsics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.UI;


public struct plateData{
    public float data;
    public Vector3 hitPoint;
}


public class BuildMode : MonoBehaviour{
    public GameObject mainCam;
    public Camera buildCam;

    public GameObject marker;
    public GameObject ui;

    private GameObject selectedObejectPrefab;
    private GameObject liveSelected;
    private Renderer[] liveRends;


    public Material holographicGreen;
    public Material holographicRed;

    float plateY;
    public int tileSize = 1;

    public float sens = 25f;
    private float rotationX = 0;
    private float rotationY = 0;
    public float speed;

    public bool focused = true;

    private BaseMachine baseMachine;
    private MachineData machineData;

    public GameObject rotationPoint;
    public Rotator rotationScript;

    public Vector3 oldPos;
    public Quaternion oldRot;

    private bool lerpForward = true;
    private bool lerpBack = false;

    public InitBuildUI buildUIData;
    public static Dictionary<string, int> limits = new Dictionary<string, int>();

    public bool setRedTemp = false;


    public Material buildPlateNormal;
    public Material buildPlateGrid;
    public Renderer plateRend;


    void Start(){
        foreach(string name in buildUIData.machineNames){
            limits.Add(name, 0);
        }
    }

    void OnEnable(){
        Cursor.lockState = CursorLockMode.Locked;
        focused = true;


        PlayerHandler.canMove = false;
        buildCam = GetComponent<Camera>();

        plateY = marker.GetComponent<BaseMarker>().plate.transform.position.y;
        plateY += marker.GetComponent<BaseMarker>().plate.GetComponent<Collider>().bounds.extents.y;

        oldPos = marker.transform.position;
        oldPos.y = plateY + 15f;
        oldRot = Quaternion.Euler(55, 0, 0);


        transform.position = mainCam.transform.position;
        transform.rotation = mainCam.transform.rotation;
        rotationX = 55; //? rotation that is set in the editor

        lerpForward = true;
        plateRend = marker.GetComponent<BaseMarker>().plate.GetComponent<Renderer>();

    }


    public void select(GameObject prefab){
        if(liveSelected != null) {
            Destroy(liveSelected);
        }

        //Prefab && object setup
        selectedObejectPrefab = prefab;
        liveSelected = Instantiate(selectedObejectPrefab, Vector3.zero, selectedObejectPrefab.transform.rotation);
        Collider[] temp = liveSelected.GetComponentsInChildren<Collider>();
        foreach(Collider c in temp){
            if(c.name != liveSelected.name) c.enabled = false;
        }

        Renderer liveRend; 

        //If object has children get their renderers instead
        if(TryGetComponent<Renderer>(out liveRend)){
            liveRends = new Renderer[1];
            liveRends[0] = liveRend;
        }
        else{
            liveRends = liveSelected.GetComponentsInChildren<Renderer>();
        }

        //Get collider and arrow
        baseMachine = liveSelected.GetComponent<BaseMachine>();
        machineData = liveSelected.GetComponent<MachineData>();

        if(baseMachine.displayArrow) baseMachine.arrow.SetActive(true);

        //Get rotator if there is one
        try{
            rotationPoint = liveSelected.transform.Find("rotationPoint").gameObject;
            rotationScript = rotationPoint.GetComponent<Rotator>();
        }
        catch{
            rotationScript = null;
            Debug.Log("No rotator");
        }

        //! exception for Dropper
        if(prefab.name == "Dropper"){
            liveSelected.GetComponentInChildren<Dropper>().active = false;
        }
        toggleMenu();
    }

    // Update is called once per frame
    void Update(){
        float epsilon = 0.05f;
        if(lerpForward){
            transform.position = Vector3.Lerp(transform.position, oldPos, 0.05f);
            transform.rotation = Quaternion.Lerp(transform.rotation, oldRot, 0.05f);
            plateRend.material = buildPlateGrid;
        }
        if(lerpBack){
            transform.position = Vector3.Lerp(transform.position, mainCam.transform.position, 0.05f);
            transform.rotation = Quaternion.Lerp(transform.rotation, mainCam.transform.rotation, 0.05f);
            plateRend.material = buildPlateNormal;
        }

        if(Math.Abs((transform.position - oldPos).magnitude) > epsilon && lerpForward){
            return;
            
        }
        else{
            lerpForward = false;
        }
        
        if(focused && !lerpBack && !lerpForward){
            float xr = Input.GetAxis("Mouse X") * sens;
            float yr = -Input.GetAxis("Mouse Y") * sens;
            rotationX += yr;
            rotationY += xr;
            transform.rotation = Quaternion.Euler(new Vector3(rotationX, rotationY, 0));
        }

        if(Input.GetKeyDown(KeyCode.B)){
            lerpBack = true;
        }

        if(Math.Abs((transform.position - mainCam.transform.position).magnitude) < epsilon && lerpBack){
            lerpBack = false;
            mainCam.SetActive(true);
            ui.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            PlayerHandler.canMove = true;
            PlayerHandler.inBuildMode = false;

            if(liveSelected != null){
                Destroy(liveSelected);
            }
            gameObject.SetActive(false);
        }

        if(Input.GetKeyDown(KeyCode.Tab)){
            toggleMenu();
        }

        if(!lerpForward && !lerpBack) move();

        if(liveSelected != null && focused){
            displayHolo();
        }
    }


    public void toggleMenu(){
        focused = !focused;
        ui.SetActive(!focused);
        Cursor.lockState = focused ? CursorLockMode.Locked : CursorLockMode.None;
    }


    void move(){
        if(Input.GetKey(KeyCode.W)){
            transform.Translate(transform.forward * speed * Time.deltaTime, Space.World);
        }
        if(Input.GetKey(KeyCode.S)){
            transform.Translate(transform.forward * -speed * Time.deltaTime, Space.World);
        }
        if(Input.GetKey(KeyCode.A)){
            transform.Translate(transform.right * -speed * Time.deltaTime, Space.World);
        }
        if(Input.GetKey(KeyCode.D)){
            transform.Translate(transform.right * speed * Time.deltaTime, Space.World);
        }
        if(Input.GetKey(KeyCode.Space)){
            transform.Translate(transform.up * speed * Time.deltaTime, Space.World);
        }
        if(Input.GetKey(KeyCode.LeftShift)){
            transform.Translate(transform.up * -speed * Time.deltaTime, Space.World);
        }
    }


    void setGreen(){
        foreach(Renderer rend in liveRends){
            rend.material = holographicGreen;
        }
        setRedTemp = false;
    }


    void setRed(){
        foreach(Renderer rend in liveRends){
            rend.material = holographicRed;
        }
    }


    public void displayHolo(){
        Vector3 mouse = Input.mousePosition;
        plateData dat = getDistanceFromPlate();
        float distance = dat.data;
        if(distance == -1){return;}
        mouse.z = distance;

        liveSelected.transform.position = dat.hitPoint;
        float posy = plateY + selectedObejectPrefab.transform.position.y;
        liveSelected.transform.position = new Vector3(liveSelected.transform.position.x, posy, liveSelected.transform.position.z);

        Vector3 temp = liveSelected.transform.position;

        temp.x -= selectedObejectPrefab.transform.position.x;
        temp.z -= selectedObejectPrefab.transform.position.z;

        temp.x = RoundTo(temp.x, tileSize);
        temp.z = RoundTo(temp.z, tileSize);

        temp.x += selectedObejectPrefab.transform.position.x;
        temp.z += selectedObejectPrefab.transform.position.z;

        try{
            if(rotationScript.rotation % 90 == 0 && rotationScript.rotation != 0){
                temp -= rotationScript.degree90offset;
            }
        }
        catch{}
        


        // if the width is odd, the object will be placed in the center of the tile
        if(Mathf.RoundToInt(liveSelected.GetComponent<Collider>().bounds.size.x) % 2 != 0){
            temp.x += (tileSize / 2f);
        }
        if(Mathf.RoundToInt(liveSelected.GetComponent<Collider>().bounds.size.z) % 2 != 0){
            temp.z += (tileSize / 2f);
        }
        liveSelected.transform.position = temp;

        // Check collision and limits
        bool collided = baseMachine.colliding;
        bool hitLimit = limits[machineData.mName] > (machineData.buildLimit - 1); // -1 is needed for some reason
        if(!setRedTemp){
            foreach(Renderer rend in liveRends){
                rend.material = collided || hitLimit ? holographicRed : holographicGreen;
            }
        }

        if(baseMachine.displayArrow) baseMachine.arrow.SetActive(true);

        if(Input.GetMouseButtonDown(0) && !collided && !hitLimit){
            if(PlayerHandler.money >= machineData.price){
                Instantiate(selectedObejectPrefab, liveSelected.transform.position, liveSelected.transform.rotation);
                limits[machineData.mName] += 1;
                PlayerHandler.money -= machineData.price;
            }
            else{
                setRed();
                setRedTemp = true;
                Invoke("setGreen", 0.25f);
            }
        }
        if(Input.GetMouseButtonDown(1) && collided){
            GameObject obj = baseMachine.col.gameObject;
            Destroy(obj);
            PlayerHandler.money += baseMachine.col.GetComponent<MachineData>().price;
            baseMachine.col = null;
            baseMachine.colliding = false;
            limits[machineData.mName] -= 1;
        }

        if(Input.GetKeyDown(KeyCode.R)){
            if(rotationPoint != null){
                liveSelected.transform.RotateAround(rotationPoint.transform.position, Vector3.up, 90);
                rotationScript.rotation += 90; 
                if(rotationScript.rotation == 360){
                    rotationScript.rotation = 0;
                }
            }
            else{
                liveSelected.transform.Rotate(0, 90, 0, Space.World);
            }
        }
    }

    public static float RoundTo(float value, float multipleOf){
        float halfMultiple = multipleOf / 2;
        float rounded = Mathf.Round(value / multipleOf) * multipleOf;

        if(value - (rounded - halfMultiple) < (rounded + halfMultiple) - value) {
            return rounded - halfMultiple;
        } 
        else{
            return rounded + halfMultiple;
        }
    }


    public plateData getDistanceFromPlate(){
        RaycastHit hit;
        Ray ray = buildCam.ScreenPointToRay(Input.mousePosition);
        bool DidHit = Physics.Raycast(ray, out hit, 200f);

        if(!DidHit){
            return new plateData{data = -1, hitPoint = Vector3.zero};
        }
        else{
            return new plateData{data = hit.distance, hitPoint = hit.point};
        }
    }
}
