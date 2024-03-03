using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class itemCount : MonoBehaviour
{
    // Start is called before the first frame update
    public Text text;
    public itemScript itemScript;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //draw the item count
        text.text = "Items: " + itemScript.count.ToString();
    }
}
