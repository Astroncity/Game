using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildMode : MonoBehaviour
{
    // Start is called before the first frame update
    public Camera mainCam;
    public Camera buildCam;

    public float speed;
    public GameObject player;
    public GameObject marker;

    private GameObject[] prefabs;

    private GameObject selectedObejectPrefab;

    private GameObject liveSelected;
    private Renderer liveRend;


    public Material holographicGreen;
    public Material holographicRed;

    float plateY;
    public float sens = 25f;
    public int tileSize = 1;

    private float rotationX = 0;
    private float rotationY = 0;


    void Start()
    {
        Cursor.lockState = CursorLockMode.None;


        player.GetComponent<playerHandler>().canMove = false;
        buildCam = GetComponent<Camera>();
        plateY = marker.GetComponent<BaseMarker>().plate.transform.position.y;

        prefabs = Resources.LoadAll<GameObject>("prefabs");
        Debug.Log(prefabs[0].name);
        selectedObejectPrefab = prefabs[0];
        liveSelected = Instantiate(selectedObejectPrefab, Vector3.zero, selectedObejectPrefab.transform.rotation);
        liveSelected.GetComponent<Renderer>().material = holographicGreen;

        liveRend = liveSelected.GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButton(1)) {
            Cursor.lockState = CursorLockMode.Locked;
            float xr = Input.GetAxis("Mouse X") * Time.deltaTime * sens;
            float yr = -Input.GetAxis("Mouse Y") * Time.deltaTime * sens;
            rotationX += yr;
            rotationY += xr;

            transform.rotation = Quaternion.Euler(new Vector3(rotationX, rotationY, 0));
        }
        else {
            Cursor.lockState = CursorLockMode.None;
        }



        move();

        if(liveSelected != null) {
            displayHolo();
        }

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
    }

    public void displayHolo() {
        Vector3 mouse = Input.mousePosition;
        float distance = getDistanceFromPlate();
        if(distance == -1) { return; }
        mouse.z = distance;

        liveSelected.transform.position = buildCam.ScreenToWorldPoint(mouse);
        liveSelected.transform.position = new Vector3(liveSelected.transform.position.x, plateY + 1f, liveSelected.transform.position.z);

        Vector3 temp = liveSelected.transform.position;
        temp.x = RoundTo(temp.x, (float)tileSize); temp.z = RoundTo(temp.z, (float)tileSize);
        liveSelected.transform.position = temp;

        //check collision
        bool collided = liveSelected.GetComponent<BaseMachine>().colliding;
        if(collided) {
             liveRend.material = holographicRed;
        }
        else {
            liveRend.material = holographicGreen;
        }


        if(Input.GetMouseButtonDown(0) && !collided) {
            Instantiate(selectedObejectPrefab, liveSelected.transform.position, liveSelected.transform.rotation);
        }
    }

    public static float RoundTo(float value, float multipleOf) {
        return Mathf.Round(value / multipleOf) * multipleOf;
    }

    public void loadUI() {

    }

    public float getDistanceFromPlate() {
        RaycastHit hit;
        Ray ray = buildCam.ScreenPointToRay(Input.mousePosition);
        //Debug.DrawRay(ray.origin, ray.direction, Color.red, 200, false);
        bool DidHit = Physics.Raycast(ray, out hit, 200f);
        if(!DidHit) { return -1; }
        else {
            return hit.distance;
        }
    }
}
