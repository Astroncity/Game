using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Conveyer1Script : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter(Collision other) {
        if(other.gameObject.tag == "Item" || other.gameObject.tag == "Player") {
            other.gameObject.GetComponent<Rigidbody>().velocity = transform.right * 250 * Time.deltaTime;
        }
        Debug.Log("Collided with " + other.gameObject.name);
    }
}
