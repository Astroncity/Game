using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class playerHandler : MonoBehaviour{
    float speed;
    public GameObject cam;
    public Rigidbody rb;

    public GameObject buildCam;
    public bool canMove = true;
    private float sens;
    public bool inBuildMode = false;

    public GameObject testItem;

    void Start(){
        Application.targetFrameRate = 144;
        speed = 5f;
        Cursor.lockState = CursorLockMode.Locked;
        sens = cam.GetComponent<CameraController>().Sens;
    }

    // Update is called once per frame
    void Update(){
        if(canMove) move();

        // set player speed to 0 to avoid drifting (except gravity)
        rb.velocity = new Vector3(0, rb.velocity.y, 0);
    }


    void move(){
        if(Input.GetKey(KeyCode.W)){
            transform.Translate(transform.forward * speed * Time.deltaTime, Space.World);
        }
        if(Input.GetKey(KeyCode.S)){
            transform.Translate(transform.forward * -speed * Time.deltaTime, Space.World);
        }
        if(Input.GetKey(KeyCode.A)){
            transform.Translate(transform.right * -speed * Time.deltaTime, Space.World);
        }
        if(Input.GetKey(KeyCode.D)){
            transform.Translate(transform.right * speed * Time.deltaTime, Space.World);
        }

        if(Input.GetKeyDown(KeyCode.B)){
            cam.SetActive(false);
            buildCam.SetActive(true);
            inBuildMode = true;
        }

        if(Input.GetKeyDown(KeyCode.F)){
            for(int i = 0; i < 50; i++){
                Vector3 randomOffset = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f));
                GameObject ball = Instantiate(testItem, transform.position + transform.forward * 2 + randomOffset, Quaternion.identity);
                ball.GetComponent<Renderer>().material.color = new Color(Random.value, Random.value, Random.value);
            }
        }

        transform.rotation = new Quaternion(0, cam.transform.rotation.y, 0, cam.transform.rotation.w);
    }
}