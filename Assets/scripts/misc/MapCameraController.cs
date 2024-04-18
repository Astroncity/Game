using System;
using UnityEngine;
using UnityEngine.UI;

public class MapCameraController : MonoBehaviour {

    public Transform player;
    public float baseSize;
    private Camera thisCam;

    void Start(){
        thisCam = GetComponent<Camera>();
        baseSize = thisCam.orthographicSize;
    }   
    void Update(){
        transform.position = new Vector3(player.position.x, 100, player.position.z);

        if(Drive.inCar){
            thisCam.orthographicSize = Mathf.Lerp(thisCam.orthographicSize, baseSize*2, 5f * Time.deltaTime);
        }
        else{
            thisCam.orthographicSize = Mathf.Lerp(thisCam.orthographicSize, baseSize, 5f * Time.deltaTime);
        }
    }
}