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

    void OnCollisionStay(Collision other) {
        if(other.gameObject.tag == "Item") {
            //! GetComponent call is inefficient
            //TODO: Replace with a better method
            other.rigidbody.velocity = transform.right * 50 * Time.deltaTime;
        }
    }
}
