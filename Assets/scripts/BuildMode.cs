using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public struct plateData{
    public float data;
    public Vector3 hitPoint;
}

public class BuildMode : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject mainCam;
    public Camera buildCam;

    public float speed;
    public GameObject player;
    public GameObject marker;
    public GameObject ui;

    private GameObject prefab;

    private GameObject selectedObejectPrefab;

    private GameObject liveSelected;
    private Renderer[] liveRends;


    public Material holographicGreen;
    public Material holographicRed;

    float plateY;
    public float sens = 25f;
    public int tileSize = 1;

    private float rotationX = 0;
    private float rotationY = 0;

    public bool focused = true;

    private Collider col;
    private BaseMachine baseMachine;
    private Vector2 offset;

    private GameObject plate;


    void OnEnable()
    {

        //?ui.SetActive(true);
        Cursor.lockState = CursorLockMode.Locked;
        focused = true;


        player.GetComponent<playerHandler>().canMove = false;
        buildCam = GetComponent<Camera>();
        plateY = marker.GetComponent<BaseMarker>().plate.transform.position.y;
        plate = marker.GetComponent<BaseMarker>().plate;
        plateY += marker.GetComponent<BaseMarker>().plate.GetComponent<Collider>().bounds.extents.y;


    
    }


    public void select(GameObject prefab){
        if(liveSelected != null) {
            Destroy(liveSelected);
        }
        Debug.Log(prefab.name);
        selectedObejectPrefab = prefab;
        liveSelected = Instantiate(selectedObejectPrefab, Vector3.zero, selectedObejectPrefab.transform.rotation);

        Renderer liveRend; 

        if(TryGetComponent<Renderer>(out liveRend)){
            liveRends = new Renderer[1];
            liveRends[0] = liveRend;
        }
        else {
            liveRends = liveSelected.GetComponentsInChildren<Renderer>();
        }
        col = liveSelected.GetComponent<Collider>();
        baseMachine = liveSelected.GetComponent<BaseMachine>();
        baseMachine.arrow.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        /*if(Input.GetMouseButton(1)) {
            Cursor.lockState = CursorLockMode.Locked;
            float xr = Input.GetAxis("Mouse X") * sens;
            float yr = -Input.GetAxis("Mouse Y") * sens;
            rotationX += yr;
            rotationY += xr;

            transform.rotation = Quaternion.Euler(new Vector3(rotationX, rotationY, 0));
        }
        else {
            Cursor.lockState = CursorLockMode.None;
        }*/
        if(focused){
            float xr = Input.GetAxis("Mouse X") * sens;
            float yr = -Input.GetAxis("Mouse Y") * sens;
            rotationX += yr;
            rotationY += xr;
            transform.rotation = Quaternion.Euler(new Vector3(rotationX, rotationY, 0));
        }

        if(Input.GetKeyDown(KeyCode.B)) {
            mainCam.SetActive(true);
            ui.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            player.GetComponent<playerHandler>().canMove = true;

            if(liveSelected != null) {
                Destroy(liveSelected);
            }
            gameObject.SetActive(false);
        }

        if(Input.GetKeyDown(KeyCode.Tab)){
            toggleMenu();
        }



        move();

        if(liveSelected != null && focused) {
            displayHolo();
        }

    }


    public void toggleMenu(){
        focused = !focused;
        ui.SetActive(!focused);
        Cursor.lockState = focused ? CursorLockMode.Locked : CursorLockMode.None;
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
        if(Input.GetKey(KeyCode.Space)) {
            transform.Translate(transform.up * speed * Time.deltaTime, Space.World);
        }
        if(Input.GetKey(KeyCode.LeftShift)) {
            transform.Translate(transform.up * -speed * Time.deltaTime, Space.World);
        }
    }

    public void displayHolo() {
        Vector3 mouse = Input.mousePosition;
        plateData dat = getDistanceFromPlate();
        float distance = dat.data;
        if(distance == -1) { return; }
        mouse.z = distance;

        //liveSelected.transform.position = buildCam.ScreenToWorldPoint(mouse);
        liveSelected.transform.position = dat.hitPoint;
        float posy;
        posy = plateY;


        liveSelected.transform.position = new Vector3(liveSelected.transform.position.x, posy, liveSelected.transform.position.z);

        Vector3 temp = liveSelected.transform.position;
        temp.x = RoundTo(temp.x, (float)tileSize) + selectedObejectPrefab.transform.position.x ; temp.z = RoundTo(temp.z, (float)tileSize) + selectedObejectPrefab.transform.position.z;
        liveSelected.transform.position = temp;

        //check collision
        bool collided = baseMachine.colliding;
        if(collided) {
            foreach(Renderer rend in liveRends) {
                rend.material = holographicRed;
            }
        }
        else{
            foreach(Renderer rend in liveRends) {
                rend.material = holographicGreen;
            }
        }

        baseMachine.arrow.SetActive(true);


        if(Input.GetMouseButtonDown(0) && !collided) {
            Instantiate(selectedObejectPrefab, liveSelected.transform.position, liveSelected.transform.rotation);
        }
        if(Input.GetMouseButtonDown(1) && collided) {
            Destroy(baseMachine.col.gameObject);
            baseMachine.col = null;
            baseMachine.colliding = false;
        }
        if(Input.GetKeyDown(KeyCode.R)) {
            liveSelected.transform.Rotate(0, 90, 0);
        }
    }

    public static float RoundTo(float value, float multipleOf) {
        return Mathf.Round(value / multipleOf) * multipleOf;
    }

    public void loadUI() {

    }

    public plateData getDistanceFromPlate() {
        RaycastHit hit;
        Ray ray = buildCam.ScreenPointToRay(Input.mousePosition);
        //Debug.DrawRay(ray.origin, ray.direction, Color.red, 200, false);
        bool DidHit = Physics.Raycast(ray, out hit, 200f);
        if(!DidHit){
            return new plateData{data = -1, hitPoint = Vector3.zero};
        }
        else {
            return new plateData{data = hit.distance, hitPoint = hit.point};
        }
    }
}
