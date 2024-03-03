using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class fps : MonoBehaviour
{
    // Start is called before the first frame update
    public Text text;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //update fps every other frame
        if(Time.frameCount % 20 == 0){
            drawFps();
        }
    }

    void drawFps(){
        text.text = "FPS: " + Mathf.RoundToInt((1 / Time.deltaTime)).ToString();
    }
}
