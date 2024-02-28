using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class initBuildUI : MonoBehaviour {
    // Start is called before the first frame update
    public GameObject marker;
    public GameObject uiPrefab;
    public GameObject canvas;

    void Start() {
        string[] thumbnailsPaths = Directory.GetFiles(Application.dataPath + "/Resources/MachineThumbnails");

        List<string> thumbnailsNames = new List<string>(); 

        foreach(string path in thumbnailsPaths) {
            if(Path.GetExtension(path) == ".meta") {
                continue;
            }
            string str = Path.GetFileNameWithoutExtension(path);
            thumbnailsNames.Add(str.Substring(0, str.Length - 5));
        }

        foreach(string name in thumbnailsNames) {
            GameObject ui = Instantiate(uiPrefab, marker.transform.position, uiPrefab.transform.rotation, canvas.transform);
            ui.GetComponent<MachineUI>().mName = name;
            marker.transform.Translate(new Vector3(420 / 3f, 0, 0));
        }
    }

    // Update is called once per frame
    void Update() {

    }
}
