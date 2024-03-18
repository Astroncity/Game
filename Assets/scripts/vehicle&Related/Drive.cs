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

    public uint Capacity;
    public int itemCount;

    public float motorForce = 5f;
    public float speedLimit = 100f;
    public float acceleration = 0f;
    private float carlength;

    private float accelerationIncrease = 0.5f;
    public bool inCar = false;
    private Wheel wheelScript;

    [Header("Item Display")]
    public GameObject startPosMarker;
    public GameObject itemPrefab;
    public TextMeshProUGUI itemText;
    public Canvas itemCanvas;
    private GameObject[] items;
    public uint unitSize = 2;
    
    public float tempOffset;


    void Start(){
        items = new GameObject[Capacity / unitSize];
        initItemDisplay();
        
        rb.centerOfMass = new Vector3(0, -1f, 0);
        wheelScript = frontLeftWheel.GetComponent<Wheel>();
        carlength = transform.Find("body").GetComponent<Renderer>().bounds.size.z;
    }


    void Update(){
        handleDisplayItems();
        if(Input.GetKeyDown(KeyCode.E) && inCar) exit();
        else if(Input.GetKeyDown(KeyCode.E) && Vector3.Distance(player.transform.position, this.transform.position) < 10f) enter();
        if(inCar) drive();
    }


    void handleDisplayItems(){
        for(int i = 0; i < items.Length; i++){
            if(i < itemCount / unitSize){
                items[i].SetActive(true);
            }
            else{
                items[i].SetActive(false);
            }
        }
        itemText.text = itemCount + "L" + "/" + Capacity + "L";
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
                items[i * cols + j] = item;
            }
        }
    }

    void drive(){
        if(Input.GetKey(KeyCode.W)){
            acceleration += accelerationIncrease * Time.deltaTime;
        }
        else if(Input.GetKey(KeyCode.S)){
            acceleration -= accelerationIncrease * Time.deltaTime;
        }
        

        rb.AddForce(transform.forward * motorForce * acceleration, ForceMode.Impulse);

        acceleration = Mathf.Clamp(acceleration, -1f, 1f);
        acceleration *= 0.955f;

        float turnRad = carlength / Mathf.Tan(wheelScript.rotationAmount * Mathf.Deg2Rad);
        float rotationAngle = rb.velocity.magnitude * Mathf.Rad2Deg / turnRad;

        if(acceleration < 0){
            rotationAngle *= -1;
        }

        transform.Rotate(Vector3.up, rotationAngle * Time.deltaTime);

        if(rb.velocity.magnitude > speedLimit){
            rb.velocity = rb.velocity.normalized * speedLimit;
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
}
