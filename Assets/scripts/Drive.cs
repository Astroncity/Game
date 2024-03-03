using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drive : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject player;

    public GameObject frontLeftWheel;
    public Rigidbody rb;
    public GameObject mainCamera;
    public float motorForce = 10f;
    public float speedLimit = 100f;
    public float acceleration = 0f;

    public bool inCar = false;

    void Start()
    {
        rb.centerOfMass = new Vector3(0, -1f, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.E) && inCar) exit();
        else if(Input.GetKeyDown(KeyCode.E) && Vector3.Distance(player.transform.position, this.transform.position) < 10f) enter();
        if(inCar) drive();
    }

    void drive(){
        if(Input.GetKey(KeyCode.W))
        {
            acceleration += 0.005f;
        }
        else if(Input.GetKey(KeyCode.S))
        {
            acceleration -= 0.005f;
        }

        rb.AddForce(transform.forward * motorForce * Time.deltaTime * 10f * acceleration, ForceMode.Impulse);

        if(acceleration > 1f){
            acceleration = 1f;
        }
        else if(acceleration < -1f){
            acceleration = -1f;
        }

        if(acceleration > 0){
            acceleration -= 0.003f;
        }
        else if(acceleration < 0){
            acceleration += 0.003f;
        }


        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, frontLeftWheel.transform.eulerAngles.y, 0), 0.8f * Time.deltaTime * rb.velocity.magnitude * acceleration);

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
