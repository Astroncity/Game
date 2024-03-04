using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.Intrinsics;
using UnityEngine;
using UnityEngine.UI;



public struct plateData{
    public float data;
    public Vector3 hitPoint;
}

public class BuildMode : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject mainCam;
    public Camera buildCam;

    public float speed;
    public GameObject player;
    public GameObject marker;
    public GameObject ui;

    private GameObject prefab;

    private GameObject selectedObejectPrefab;

    private GameObject liveSelected;
    private Renderer[] liveRends;


    public Material holographicGreen;
    public Material holographicRed;

    float plateY;
    public float sens = 25f;
    public int tileSize = 1;

    private float rotationX = 0;
    private float rotationY = 0;

    public bool focused = true;

    private Collider col;
    private BaseMachine baseMachine;
    private Vector2 offset;

    private GameObject plate;

    private GameObject panels;
    public GameObject rotationPoint;
    public rotator rotationScript;

    public Vector3 oldPos;
    public Quaternion oldRot;
    private bool lerpForward = true;
    private bool lerpBack = false;


    void OnEnable()
    {

        //?ui.SetActive(true);
        Cursor.lockState = CursorLockMode.Locked;
        focused = true;


        player.GetComponent<playerHandler>().canMove = false;
        buildCam = GetComponent<Camera>();
        plateY = marker.GetComponent<BaseMarker>().plate.transform.position.y;
        plate = marker.GetComponent<BaseMarker>().plate;
        plateY += marker.GetComponent<BaseMarker>().plate.GetComponent<Collider>().bounds.extents.y;

        oldPos = marker.transform.position;
        oldPos.y = plateY + 15f;
        oldRot = Quaternion.Euler(55, 0, 0);


        transform.position = mainCam.transform.position;
        transform.rotation = mainCam.transform.rotation;
        rotationX = 55; //? rotation that is set in the editor

        lerpForward = true;

    
    }


    public void select(GameObject prefab){
        Debug.Log("ran Select");
        if(liveSelected != null) {
            Destroy(liveSelected);
        }
        Debug.Log(prefab.name);
        selectedObejectPrefab = prefab;
        liveSelected = Instantiate(selectedObejectPrefab, Vector3.zero, selectedObejectPrefab.transform.rotation);
        Collider[] temp = liveSelected.GetComponentsInChildren<Collider>();
        foreach(Collider c in temp) {
            if(c.name != liveSelected.name) c.enabled = false;
        }

        Renderer liveRend; 

        if(TryGetComponent<Renderer>(out liveRend)){
            liveRends = new Renderer[1];
            liveRends[0] = liveRend;
        }
        else {
            liveRends = liveSelected.GetComponentsInChildren<Renderer>();
        }
        col = liveSelected.GetComponent<Collider>();
        baseMachine = liveSelected.GetComponent<BaseMachine>();
        baseMachine.arrow.SetActive(true);
        try{
            rotationPoint = liveSelected.transform.Find("rotationPoint").gameObject;
            rotationScript = rotationPoint.GetComponent<rotator>();
        }
        catch{
            rotationScript = null;
        }
        toggleMenu();
    }

    // Update is called once per frame
    void Update()
    {

        float epsilon = 0.05f;
        if(lerpForward){
            transform.position = Vector3.Lerp(transform.position, oldPos, 0.05f);
            transform.rotation = Quaternion.Lerp(transform.rotation, oldRot, 0.05f);
        }

        if(lerpBack){
            transform.position = Vector3.Lerp(transform.position, mainCam.transform.position, 0.05f);
            transform.rotation = Quaternion.Lerp(transform.rotation, mainCam.transform.rotation, 0.05f);
        }


        if(Math.Abs((transform.position - oldPos).magnitude) > epsilon && lerpForward){
            return;
        }else{
            lerpForward = false;
        }
        
        if(focused && !lerpBack && !lerpForward){
            float xr = Input.GetAxis("Mouse X") * sens;
            float yr = -Input.GetAxis("Mouse Y") * sens;
            rotationX += yr;
            rotationY += xr;
            transform.rotation = Quaternion.Euler(new Vector3(rotationX, rotationY, 0));
        }

        if(Input.GetKeyDown(KeyCode.B)) {
            lerpBack = true;
        }

        if(Math.Abs((transform.position - mainCam.transform.position).magnitude) < epsilon && lerpBack) {
            lerpBack = false;
            mainCam.SetActive(true);
            ui.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            player.GetComponent<playerHandler>().canMove = true;
            player.GetComponent<playerHandler>().inBuildMode = false;

            if(liveSelected != null) {
                Destroy(liveSelected);
            }
            gameObject.SetActive(false);
        }

        if(Input.GetKeyDown(KeyCode.Tab)){
            toggleMenu();
        }

        if(!lerpForward && !lerpBack) move();

        if(liveSelected != null && focused) {
            displayHolo();
        }

    }


    public void toggleMenu(){
        focused = !focused;
        ui.SetActive(!focused);
        Cursor.lockState = focused ? CursorLockMode.Locked : CursorLockMode.None;
    }

    void move() {
        if(Input.GetKey(KeyCode.W)) {
            transform.Translate(transform.forward * speed * Time.deltaTime, Space.World);
        }
        if(Input.GetKey(KeyCode.S)) {
            transform.Translate(transform.forward * -speed * Time.deltaTime, Space.World);
        }
        if(Input.GetKey(KeyCode.A)) {
            transform.Translate(transform.right * -speed * Time.deltaTime, Space.World);
        }
        if(Input.GetKey(KeyCode.D)) {
            transform.Translate(transform.right * speed * Time.deltaTime, Space.World);
        }
        if(Input.GetKey(KeyCode.Space)) {
            transform.Translate(transform.up * speed * Time.deltaTime, Space.World);
        }
        if(Input.GetKey(KeyCode.LeftShift)) {
            transform.Translate(transform.up * -speed * Time.deltaTime, Space.World);
        }
    }

    public void displayHolo() {
        Vector3 mouse = Input.mousePosition;
        plateData dat = getDistanceFromPlate();
        float distance = dat.data;
        if(distance == -1) { return; }
        mouse.z = distance;

        liveSelected.transform.position = dat.hitPoint;
        float posy = plateY + selectedObejectPrefab.transform.position.y;
        liveSelected.transform.position = new Vector3(liveSelected.transform.position.x, posy, liveSelected.transform.position.z);



        // Snap to grid
        Vector3 temp = liveSelected.transform.position;

        if(rotationScript != null && (rotationScript.rotation == 90 || rotationScript.rotation == 270)) {
            temp += rotationScript.degree90offset * 1.2f;
        }

        temp.x = RoundTo(temp.x, tileSize);
        temp.z = RoundTo(temp.z, tileSize);

        // if the width is odd, the object will be placed in the center of the tile
        // do not use scale
        if(Mathf.RoundToInt(liveSelected.GetComponent<Collider>().bounds.size.x) % 2 != 0) {
            Debug.Log("Before X: " + temp.x);
            temp.x += (tileSize / 2f);
            Debug.Log("After X: " + temp.x);
        }
        if(Mathf.RoundToInt(liveSelected.GetComponent<Collider>().bounds.size.z) % 2 != 1) {
            temp.z += (tileSize / 2f);
        }
        liveSelected.transform.position = temp;

        // Check collision
        bool collided = baseMachine.colliding;
        foreach(Renderer rend in liveRends) {
            rend.material = collided ? holographicRed : holographicGreen;
        }

        baseMachine.arrow.SetActive(true);

        // Instantiate object on left-click if not collided
        if(Input.GetMouseButtonDown(0) && !collided) {
            Instantiate(selectedObejectPrefab, liveSelected.transform.position, liveSelected.transform.rotation);
        }
        // Destroy object on right-click if collided
        if(Input.GetMouseButtonDown(1) && collided) {
            Destroy(baseMachine.col.gameObject);
            baseMachine.col = null;
            baseMachine.colliding = false;
        }
        // Rotate object on 'R' key press
        if(Input.GetKeyDown(KeyCode.R)) {
            if(rotationPoint != null) {
                liveSelected.transform.RotateAround(rotationPoint.transform.position, Vector3.up, 90);
                rotationScript.rotation += 90; 
            } else {
                liveSelected.transform.Rotate(0, 90, 0, Space.World);
            }
        }
    }

    public static float RoundTo(float value, float multipleOf) {
        float halfMultiple = multipleOf / 2;
        float rounded = Mathf.Round(value / multipleOf) * multipleOf;

        if (value - (rounded - halfMultiple) < (rounded + halfMultiple) - value) {
            return rounded - halfMultiple;
        } else {
            return rounded + halfMultiple;
        }
    }


    public void loadUI() {

    }

    public plateData getDistanceFromPlate() {
        RaycastHit hit;
        Ray ray = buildCam.ScreenPointToRay(Input.mousePosition);
        //Debug.DrawRay(ray.origin, ray.direction, Color.red, 200, false);
        bool DidHit = Physics.Raycast(ray, out hit, 200f);
        if(!DidHit){
            return new plateData{data = -1, hitPoint = Vector3.zero};
        }
        else {
            return new plateData{data = hit.distance, hitPoint = hit.point};
        }
    }
}
