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

    private Vector3 mouse;

    public Material holographic;

    void Start()
    {
        player.GetComponent<playerHandler>().canMove = false;
        buildCam = GetComponent<Camera>();
        
        prefabs = Resources.LoadAll<GameObject>("prefabs");
        Debug.Log(prefabs[0].name);
        selectedObejectPrefab = prefabs[0];
        liveSelected = Instantiate(selectedObejectPrefab, mainCam.ScreenToWorldPoint(Input.mousePosition), selectedObejectPrefab.transform.rotation);
        liveSelected.GetComponent<Renderer>().material = holographic;

    }

    // Update is called once per frame
    void Update()
    {
        float xr = Input.GetAxis("Mouse X") * Time.deltaTime * mainCam.GetComponent<CameraController>().Sens;
        float yr = -Input.GetAxis("Mouse Y") * Time.deltaTime * mainCam.GetComponent<CameraController>().Sens;

        transform.Rotate(new Vector3(yr, xr, 0));
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, 0);

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

    public void initBuild() {
        mainCam.enabled = false;
        gameObject.SetActive(gameObject);
        
    }

    public void displayHolo() {
        liveSelected.transform.position = buildCam.ScreenToWorldPoint(Input.mousePosition);
        liveSelected.transform.position.Set(liveSelected.transform.position.x, marker.GetComponent<BaseMarker>().plate.transform.position.y + 0.1);
    }

    public void loadUI() {

    }
}
