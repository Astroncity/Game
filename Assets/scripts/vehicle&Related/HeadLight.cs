using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadLight : MonoBehaviour{
    private float timeOfDay;
    private LightingManager lightingManager;
    
    void Start(){
        lightingManager = GameObject.Find("Global Volume").GetComponent<LightingManager>();
        timeOfDay = lightingManager.TimeOfDay;
    }

    void Update(){
        timeOfDay = lightingManager.TimeOfDay;
        if(timeOfDay > 18 || timeOfDay < 6){
            GetComponent<Light>().enabled = true;
        }
        else{
            GetComponent<Light>().enabled = false;
        }
    }
}
