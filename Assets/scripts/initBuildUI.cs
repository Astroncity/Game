using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class InitBuildUI : MonoBehaviour{
    public GameObject marker;
    public GameObject uiPrefab;
    public GameObject canvas;

    public string[] machineNames;
    

    void Start(){
        foreach(string name in machineNames){
            GameObject ui = Instantiate(uiPrefab, marker.transform.position, uiPrefab.transform.rotation, canvas.transform);
            ui.GetComponent<MachineUI>().mName = name;
            marker.transform.Translate(new Vector3(420 / 3f, 0, 0));
        }
    }
}
