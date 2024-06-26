using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHandler : MonoBehaviour{
    float speed;
    public static int money = 69420;
    public static bool onMilkRun = false;
    public GameObject cam;
    public Rigidbody rb;

    public GameObject buildCam;
    public static bool canMove = true;
    private float sens;
    public static bool inBuildMode = false;
    public static bool holdingItem = false;
    public static bool canHoldItem = false;
    public static GameObject currentItem;

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
        if(!holdingItem) currentItem = checkMouseOnItem();
        holdItem();

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


    void holdItem(){
        if(currentItem == null) return;
        if(Input.GetMouseButton(0) && (canHoldItem || holdingItem)){
            holdingItem = true;
            currentItem.GetComponent<ItemScript>().hold();
        }
        else{
            holdingItem = false;
        }
    }


    private GameObject checkMouseOnItem(){
        if(Camera.main == null) return null;
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(ray, out hit)){
            if(hit.collider.gameObject.tag == "Item"){
                canHoldItem = true;
                return hit.collider.gameObject;
            }
        }
        canHoldItem = false;
        return null;

    }
}