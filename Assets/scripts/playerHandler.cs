using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class playerHandler : MonoBehaviour {
    // Start is called before the first frame update
    float speed;
    public GameObject cam;
    public Rigidbody rb;

    public GameObject buildCam;
    public bool canMove = true;
    private float sens;
    void Start() {
        Application.targetFrameRate = 144;
        speed = 5f;
        Cursor.lockState = CursorLockMode.Locked;
        sens = cam.GetComponent<CameraController>().Sens;
    }

    // Update is called once per frame
    void Update() {
        if(canMove) { move(); }

        // set player speed to 0 to avoid drifting (except gravity)
        rb.velocity = new Vector3(0, rb.velocity.y, 0);
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
        transform.rotation = new Quaternion(0, cam.transform.rotation.y, 0, cam.transform.rotation.w);

    }
}