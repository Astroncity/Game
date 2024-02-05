using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerHandler : MonoBehaviour {
    // Start is called before the first frame update
    float speed;
    public GameObject cam;
    void Start() {
        speed = 25f;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update() {
        move();
        Debug.Log(transform.forward);
    }


    void move() {
        if(Input.GetKey(KeyCode.W)) {
            transform.Translate(cam.transform.forward * speed * Time.deltaTime);
        }
        if(Input.GetKey(KeyCode.S)) {
            transform.Translate(cam.transform.forward * -speed * Time.deltaTime);
        }
        if(Input.GetKey(KeyCode.A)) {
            transform.Translate(cam.transform.right * -speed * Time.deltaTime);
        }
        if(Input.GetKey(KeyCode.D)) {
            transform.Translate(cam.transform.right * speed * Time.deltaTime);
        }


        //cam
        float xr = Input.GetAxis("Mouse X");
        float yr = Input.GetAxis("Mouse Y");

        transform.Rotate(new Vector3(0, xr, 0));
        cam.transform.Rotate(new Vector3(yr, xr, 0));

        cam.transform.rotation = Quaternion.Euler(cam.transform.eulerAngles.x, cam.transform.eulerAngles.y, 0);

    }
}