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

        ReadPriceFromJSON();
    }

    void Update() {
        if(isHovering) {
            form.position = Vector3.Lerp(form.position, new Vector3(form.position.x, targetY + hoverOffset, form.position.z), Time.deltaTime * 10f);
        }
        else {
            form.position = Vector3.Lerp(form.position, new Vector3(form.position.x, targetY, form.position.z), Time.deltaTime * 10f);
        }
    }

    public void OnPointerEnter(PointerEventData eventData) {
        isHovering = true;
        player.focused = false;
    }

    public void OnPointerExit(PointerEventData eventData) {
        isHovering = false;
        player.focused = true;
    }

    void ReadPriceFromJSON() {
        string jsonFilePath = "MachineData/MachineData";
        TextAsset jsonFile = Resources.Load<TextAsset>(jsonFilePath);

        if(jsonFile != null) {
            MachineData machineData = JsonUtility.FromJson<MachineData>(jsonFile.text);

            if(machineData != null && machineData.Machines.ContainsKey(mName)) {
                price = machineData.Machines[mName].price;
                Debug.Log(mName + ": " + price); 
            }
            else {
                Debug.LogWarning("Machine " + mName + " not found in JSON data.");
            }
        }
        else {
            Debug.LogError("JSON file not found at path: " + jsonFilePath);
        }
    }
}

[System.Serializable]
public class MachineData {
    public Dictionary<string, Machine> Machines;
}

[System.Serializable]
public class Machine {
    public int price;
}