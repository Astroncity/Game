using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Drive : MonoBehaviour{
    public GameObject player;
    public GameObject mainCamera;
    public GameObject frontLeftWheel;

    public Rigidbody rb;

    public int Capacity;


    public bool inCar = false;

    [Header("Item Display")]
    public GameObject startPosMarker;
    public GameObject itemPrefab;
    public TextMeshProUGUI itemText;
    public Canvas itemCanvas;
    private GameObject[] displayItems;
    public uint unitSize = 2;
    
    public float tempOffset;

    public List<ItemData> items;

    public enum Axel{
        Front,
        Rear
    }

    [Serializable]
    public struct Wheel{
        public GameObject wheelModel;
        public WheelCollider wheelCollider;
        public Axel axel;
    }

    public float maxAcceleration = 30.0f;
    public float brakeAcceleration = 50.0f;

    public float turnSensitivity = 1.0f;
    public float maxSteerAngle = 30.0f;


    public List<Wheel> wheels;

    float moveInput;
    float steerInput;

    public GameObject arrow;


    void Start(){
        displayItems = new GameObject[Capacity / unitSize];
        initItemDisplay();
        
        Debug.Log(rb.centerOfMass);
        rb.centerOfMass -= new Vector3(0, 3f, 0);
        items = new List<ItemData>();
        arrow.SetActive(false);
    }


    void Update(){
        handleDisplayItems();
        if(Input.GetKeyDown(KeyCode.E) && inCar) exit();
        else if(Input.GetKeyDown(KeyCode.E) && Vector3.Distance(player.transform.position, this.transform.position) < 10f) enter();
        
        moveInput = Input.GetAxis("Vertical");
        steerInput = Input.GetAxis("Horizontal");

        AnimateWheels();
        handleArrow();
    }

    void handleArrow(){
        if(PlayerHandler.onMilkRun && DropPoint.currentPickupPoint != null){
            arrow.SetActive(true);
            arrow.transform.LookAt(DropPoint.currentPickupPoint.transform);
            arrow.transform.Rotate(0, -90, 0);
        }
        else{
            arrow.SetActive(false);
        }
    }


    void handleDisplayItems(){
        for(int i = 0; i < displayItems.Length; i++){
            if(i < items.Count / unitSize){
                displayItems[i].SetActive(true);
            }
            else{
                displayItems[i].SetActive(false);
            }
        }
        itemText.text = items.Count + "L" + "/" + Capacity + "L";
        itemCanvas.transform.rotation = Quaternion.LookRotation(itemCanvas.transform.position - mainCamera.transform.position);
    } 


    public void initItemDisplay(){
        Debug.Log("Init Item Display");
        int rows = 4; int cols = 3;
        //int xOffset = 1; int zOffset = -1;
        float xOffset = itemPrefab.GetComponent<Renderer>().bounds.size.x + tempOffset;
        float zOffset = -itemPrefab.GetComponent<Renderer>().bounds.size.z;
        for(int i = 0; i < rows; i++){
            for(int j = 0; j < cols; j++){
                GameObject item = Instantiate(itemPrefab, startPosMarker.transform.position + new Vector3(xOffset * j, 0, zOffset * i), itemPrefab.transform.rotation);
                item.transform.SetParent(this.transform);
                item.SetActive(false);
                displayItems[i * cols + j] = item;
            }
        }
    }


    void enter(){
        mainCamera.GetComponent<CameraController>().followingPlayer = false;
        mainCamera.GetComponent<CameraController>().car = this.gameObject;
        mainCamera.GetComponent<CameraController>().followingCar = true;
        inCar = true;
        player.SetActive(false);
    }

    void exit(){
        player.SetActive(true);
        mainCamera.GetComponent<CameraController>().followingPlayer = true;
        mainCamera.GetComponent<CameraController>().followingCar = false;
        inCar = false;
        player.transform.position = this.transform.position + new Vector3(2, 0, 0);
    }

    
    public void addItem(ItemData item){
        items.Add(item);
    }


    void FixedUpdate(){
        if(!inCar){
            moveInput = 0;
            steerInput = 0;
        }
        
        drive();
    }


    void drive(){
        Move();
        Steer();
        Brake();
    }


    void Move(){
        foreach(var wheel in wheels){
            wheel.wheelCollider.motorTorque = moveInput * 100 * maxAcceleration * Time.deltaTime;
        }
    }

    void Steer(){
        foreach(var wheel in wheels){
            if(wheel.axel == Axel.Front){
                var _steerAngle = steerInput * turnSensitivity * maxSteerAngle;
                wheel.wheelCollider.steerAngle = Mathf.Lerp(wheel.wheelCollider.steerAngle, _steerAngle, 0.6f);
            }
        }
    }

    void Brake(){
        if(Input.GetKey(KeyCode.Space) || moveInput == 0){
            foreach(var wheel in wheels){
                wheel.wheelCollider.brakeTorque = 1200 * brakeAcceleration * Time.deltaTime;
            }
        }
        else{
            foreach(var wheel in wheels){
                wheel.wheelCollider.brakeTorque = 0;
            }
        }
    }

    void AnimateWheels(){
        foreach(var wheel in wheels){
            Quaternion rot;
            Vector3 pos;
            wheel.wheelCollider.GetWorldPose(out pos, out rot);
            wheel.wheelModel.transform.position = pos;
            wheel.wheelModel.transform.rotation = rot;
        }
    }
}