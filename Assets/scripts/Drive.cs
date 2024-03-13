using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drive : MonoBehaviour{
    public GameObject player;
    public GameObject mainCamera;
    public GameObject frontLeftWheel;

    public Rigidbody rb;

    public uint Capacity;
    public uint itemCount;

    public float motorForce = 5f;
    public float speedLimit = 100f;
    public float acceleration = 0f;
    private float carlength;

    private float accelerationIncrease = 0.5f;
    public bool inCar = false;
    private Wheel wheelScript;


    void Start(){
        rb.centerOfMass = new Vector3(0, -1f, 0);
        wheelScript = frontLeftWheel.GetComponent<Wheel>();
        carlength = transform.Find("body").GetComponent<Renderer>().bounds.size.z;
    }


    void Update(){
        if(Input.GetKeyDown(KeyCode.E) && inCar) exit();
        else if(Input.GetKeyDown(KeyCode.E) && Vector3.Distance(player.transform.position, this.transform.position) < 10f) enter();
        if(inCar) drive();
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
