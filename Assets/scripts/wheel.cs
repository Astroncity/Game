using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wheel : MonoBehaviour{
    // Start is called before the first frame update
    private Drive car;
    public float maxSteerAngle = 30;
    [HideInInspector]
    public float rotationAmount = 0;


    void Start(){
        car = transform.parent.gameObject.GetComponent<Drive>();
    }


    void Update(){
        if(car.inCar) handleRotation();
    }


    void handleRotation(){
        if(Input.GetKey(KeyCode.A)){
            rotationAmount -= 0.3f;
        }
        else if(Input.GetKey(KeyCode.D)){
            rotationAmount += 0.3f;
        }
        else{
            if(rotationAmount > 0){
                rotationAmount -= 0.3f;
            }
            else if(rotationAmount < 0){
                rotationAmount += 0.3f;
            }
        }

        if(Math.Abs(rotationAmount) < 0.3f){
            rotationAmount = 0;
        }

        if(rotationAmount > maxSteerAngle){
            rotationAmount = maxSteerAngle;
        }
        else if(rotationAmount < -maxSteerAngle){
            rotationAmount = -maxSteerAngle;
        }

        transform.localEulerAngles = new Vector3(0, rotationAmount, 0);
    }
}
