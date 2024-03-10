using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Conveyer1Script : MonoBehaviour{
    public float force = 50;
    public float rotationOffset = 0;


    void OnCollisionStay(Collision other) {
        if(other.gameObject.tag == "Item") {
            transform.Rotate(new Vector3(0, rotationOffset, 0));
            Vector3 temp = transform.right;
            transform.Rotate(new Vector3(0, -rotationOffset, 0));

            other.rigidbody.velocity = temp * force * Time.deltaTime;
        }
    }
}
