using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class playerHandler : MonoBehaviour {
    // Start is called before the first frame update
    float speed;
    public GameObject cam;

    public GameObject buildCam;
    public bool canMove = true;
    private float sens;
    void Start() {
        speed = 25f;
        Cursor.lockState = CursorLockMode.Locked;
        sens = cam.GetComponent<CameraController>().Sens;
    }

    // Update is called once per frame
    void Update() {
        if(canMove) { move(); }
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
        if(Input.GetKey(KeyCode.B)) {
            cam.SetActive(false);
            buildCam.SetActive(true);
        }

        //cam
        float xr = Input.GetAxis("Mouse X") * Time.deltaTime * sens;

        transform.Rotate(new Vector3(0, xr, 0));

    }
}