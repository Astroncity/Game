using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.IO;
using System.Collections.Generic;

public class MachineUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler{
    public string mName;
    private int price;

    private Sprite thumbnailImage;
    private GameObject image;

    public GameObject prefab;

    public GameObject imageContainer;
    public RectTransform form;

    private BuildMode player;

    private float initY;
    private bool isHovering = false;
    private float targetY;
    public float hoverOffset = 50f;

    private Resourcessub resources;


    void Start(){
        resources = GameObject.Find("machinedata").GetComponent<Resourcessub>();
        player = GameObject.Find("BuildModeCam").GetComponent<BuildMode>();
        initY = form.position.y;
        GetComponentInChildren<Text>().text = mName;
        Sprite thumbnailImage = null;
        prefab = null;

        foreach(Sprite sprite in resources.MachineThumbnails){
            if(sprite.name == mName + "Image"){
                thumbnailImage = sprite;
                break;
            }
        }
        
        foreach(GameObject machine in resources.MachinePrefabs){
            if(machine.name == mName){
                prefab = machine;
                break;
            }
        }

        if(thumbnailImage == null){
            Debug.Log("No thumbnail found for " + mName);
            return;
        }
        if(prefab == null){
            Debug.Log("No prefab found for " + mName);
            return;
        }

        Image imageComponent = imageContainer.GetComponent<Image>();
        imageComponent.sprite = thumbnailImage;

        targetY = initY;


        MachineData data = prefab.GetComponent<MachineData>();
        price = data.price;
        
        transform.Find("price").GetComponent<Text>().text = price.ToString() + "g";

    }
    

    void Update(){
        if(isHovering){
            form.position = Vector3.Lerp(form.position, new Vector3(form.position.x, targetY + hoverOffset, form.position.z), Time.deltaTime * 10f);
            if(Input.GetMouseButtonDown(0)){
                player.select(prefab);
            }
        }
        else{
            form.position = Vector3.Lerp(form.position, new Vector3(form.position.x, targetY, form.position.z), Time.deltaTime * 10f);
        }
    }


    public void OnPointerEnter(PointerEventData eventData){
        isHovering = true;
    }


    public void OnPointerExit(PointerEventData eventData){
        isHovering = false;
    }


    void OnDisable(){
        isHovering = false;
        form.position = new Vector3(form.position.x, initY, form.position.z);
    }

}
