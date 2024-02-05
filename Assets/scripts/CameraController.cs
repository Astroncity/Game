using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject player;
    private Vector3 dist;
    void Start()
    {
        dist.x = transform.position.x - player.transform.position.x;
        dist.y = transform.position.y - player.transform.position.y - 1.5f;
        dist.z = transform.position.z - player.transform.position.z;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = player.transform.position;
    }
}
