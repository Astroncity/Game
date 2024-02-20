using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MachineUI : MonoBehaviour
{
    public string mName;
    private int price;

    private Sprite thumbnailImage;
    private GameObject image;

    const float scaleW = 350 / 3f;
    const float scaleH = 400 / 3f;




    void Start()
    {
        GetComponentInChildren<Text>().text = mName;
        thumbnailImage = Resources.Load<Sprite>("MachineThumbnails/" + mName + "Image");
        GetComponentInChildren<Image>().sprite = thumbnailImage;
    }

    void Update()
    {
        
    }
}
