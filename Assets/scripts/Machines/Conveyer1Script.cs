using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Conveyer1Script : MonoBehaviour
{
    // Start is called before the first frame update
    public float force = 50;
    public float rotationOffset = 0;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionStay(Collision other) {
        if(other.gameObject.tag == "Item") {
            //! GetComponent call is inefficient
            //TODO: Replace with a better method
            transform.Rotate(new Vector3(0, rotationOffset, 0));
            Vector3 temp = transform.right;
            transform.Rotate(new Vector3(0, -rotationOffset, 0));

            other.rigidbody.velocity = temp * force * Time.deltaTime;
        }
    }
}
