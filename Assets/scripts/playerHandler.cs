using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerHandler : MonoBehaviour {
    // Start is called before the first frame update
    float speed;
    public GameObject cam;
    public GameObject tracker;
    void Start() {
        speed = 25f;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update() {
        move();
        Debug.Log("testing");
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


        //cam
        float xr = Input.GetAxis("Mouse X") * Time.deltaTime * cam.GetComponent<CameraController>().Sens;
        float yr = Input.GetAxis("Mouse Y") * Time.deltaTime * cam.GetComponent<CameraController>().Sens;

        transform.Rotate(new Vector3(0, xr, 0));
        cam.transform.Rotate(new Vector3(yr, xr, 0));
        cam.transform.eulerAngles = new Vector3(cam.transform.eulerAngles.x, cam.transform.eulerAngles.y, 0);

    }
}