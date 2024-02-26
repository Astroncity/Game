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

        string[] thumbnailsNames = new string[thumbnailsPaths.Length];

        for(int i = 0; i < thumbnailsPaths.Length; i++) {
            thumbnailsNames[i] = Path.GetFileNameWithoutExtension(thumbnailsPaths[i]);
            thumbnailsNames[i] = thumbnailsNames[i].Substring(0, thumbnailsNames[i].Length - 5);
        }

        for(int i = 0; i < thumbnailsNames.Length; i++) {
            GameObject ui = Instantiate(uiPrefab, marker.transform.position, uiPrefab.transform.rotation, canvas.transform);
            ui.GetComponent<MachineUI>().mName = thumbnailsNames[i];
            marker.transform.Translate(new Vector3(420 / 3f, 0, 0));
        }
    }

    // Update is called once per frame
    void Update() {

    }
}
