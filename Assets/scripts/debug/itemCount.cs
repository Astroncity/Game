using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class itemCount : MonoBehaviour
{
    public Text text;
    void Start()
    {
        
    }

    void Update()
    {
        text.text = "Items: " + ItemScript.count.ToString();
    }
}
