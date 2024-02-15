using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseMachine : MonoBehaviour
{
    // Start is called before the first frame update
    public bool colliding = false;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider collision) {
        if(collision.gameObject.CompareTag("machine")) colliding = true;
    }

    private void OnTriggerExit(Collider collision) {
        if(collision.gameObject.CompareTag("machine")) colliding = false;
    }

}
