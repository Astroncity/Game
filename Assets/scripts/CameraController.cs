using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
    public GameObject player;
    private Vector3 dist;

    //Rotation Sensitivity
    public float minAngle = -90f;
    public float maxAngle = 90f;
    public float Sens = 50;

    private float rotationX = 0;
    private float rotationY = 0;
  
    //Rotation Value

    void Start() {
        dist.x = transform.position.x - player.transform.position.x;
        dist.y = transform.position.y - player.transform.position.y - 1.5f;
        dist.z = transform.position.z - player.transform.position.z;
    }

    // Update is called once per frame
    void Update() {
        transform.position = player.transform.position - dist;

        float xr = Input.GetAxis("Mouse X") * Time.deltaTime * Sens;
        float yr = -Input.GetAxis("Mouse Y") * Time.deltaTime * Sens;
        rotationX += yr;
        rotationY += xr;

        transform.rotation = Quaternion.Euler(new Vector3(rotationX, rotationY, 0));

        if(rotationX > maxAngle){
            rotationX = Mathf.Lerp(rotationX, maxAngle, 0.1f / 2);
        }
        else if(rotationX < minAngle) {
            rotationX = Mathf.Lerp(rotationX, minAngle, 0.1f / 2);
        }
    }
}
