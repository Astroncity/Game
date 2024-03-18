using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class fps : MonoBehaviour
{
    public Text text;
    public Text timeText;
    private LightingManager lightingManager;
    private float time;
    void Start()
    {
        lightingManager = GameObject.Find("Global Volume").GetComponent<LightingManager>();
        time = lightingManager.TimeOfDay;   
    }

    void Update()
    {
        time = lightingManager.TimeOfDay;
        if(Time.frameCount % 20 == 0){
            drawFps();
        }
        ShowTime();
    }


    void drawFps(){
        text.text = "FPS: " + Mathf.RoundToInt((1 / Time.deltaTime)).ToString();
    }


    void ShowTime(){
        int hours = (int)time;
        int minutes = (int)((time - hours) * 60);

        int displayHours = hours % 12;
        if (displayHours == 0) displayHours = 12; 

        string minutesString = (minutes < 10) ? "0" + minutes.ToString() : minutes.ToString();
        string ampm = (hours < 12) ? "AM" : "PM";
        string timeString = displayHours.ToString() + ":" + minutesString + " " + ampm;

        timeText.text = timeString; 
    }


    
}
