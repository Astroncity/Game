using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
    public GameObject player;
    private Vector3 dist;
    private Vector3 carDist;

    //Rotation Sensitivity
    public float minAngle = -90f;
    public float maxAngle = 90f;
    public float Sens = 0.05f;

    private float rotationX = 0;
    private float rotationY = 0;

    public bool followingPlayer = true;
    public bool followingCar = false;

    public GameObject car = null;

  
    //Rotation Value

    void Start() {
        dist.x = transform.position.x - player.transform.position.x;
        dist.y = transform.position.y - player.transform.position.y - 1.15f;
        dist.z = transform.position.z - player.transform.position.z;

        carDist = new Vector3(0, -6, 5);
    }

    // Update is called once per frame
    void Update() {
        if(followingPlayer){
            followPlayer();
        }
        else if(followingCar){
            followCar();
        }

        float xr = Input.GetAxis("Mouse X") * Sens;
        float yr = -Input.GetAxis("Mouse Y") * Sens;
        rotationX += yr;
        rotationY += xr;

        transform.rotation = Quaternion.Euler(new Vector3(rotationX, rotationY, 0));

        if(rotationX > maxAngle){
            rotationX = Mathf.Lerp(rotationX, maxAngle, 0.05f * Time.deltaTime * 70);
        }
        else if(rotationX < minAngle) {
            rotationX = Mathf.Lerp(rotationX, minAngle, 0.05f * Time.deltaTime * 70);
        }
    }

    void followPlayer() {
        transform.position = player.transform.position - dist;
    }

    void followCar(){
        transform.position = car.transform.position - carDist;
    }
}
