using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.IO;
using System.Collections.Generic;

public class MachineUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
    public string mName;
    private int price;

    private Sprite thumbnailImage;
    private GameObject image;


    private GameObject prefab;

    public GameObject imageContainer;
    public RectTransform form;

    private BuildMode player;

    private float initY;
    private bool isHovering = false;
    private float targetY;
    public float hoverOffset = 50f;

    void Start() {
        player = GameObject.Find("BuildModeCam").GetComponent<BuildMode>();
        initY = form.position.y;
        GetComponentInChildren<Text>().text = mName;
        thumbnailImage = Resources.Load<Sprite>("MachineThumbnails/" + mName + "Image");

        Image imageComponent = imageContainer.GetComponent<Image>();
        imageComponent.sprite = thumbnailImage;

        targetY = initY;
        string path = "prefabs/" + mName;
        prefab = Resources.Load<GameObject>(path);
        MachineData data = prefab.GetComponent<MachineData>();
        price = data.price;
        
        transform.Find("price").GetComponent<Text>().text = price.ToString() + "g";

    }



    void Update() {
        if(isHovering) {
            form.position = Vector3.Lerp(form.position, new Vector3(form.position.x, targetY + hoverOffset, form.position.z), Time.deltaTime * 10f);
            if(Input.GetMouseButtonDown(0)) {
                player.select(prefab);
                player.toggleMenu();
            }
        }
        else {
            form.position = Vector3.Lerp(form.position, new Vector3(form.position.x, targetY, form.position.z), Time.deltaTime * 10f);
        }
    }

    public void OnPointerEnter(PointerEventData eventData) {
        isHovering = true;
    }

    public void OnPointerExit(PointerEventData eventData) {
        isHovering = false;
    }
}
