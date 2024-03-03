using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseMachine : MonoBehaviour
{
    // Start is called before the first frame update
    public bool colliding = false;
    public Collider col;
    public GameObject arrowP;

    public GameObject arrow;

    void Start()
    {
        arrow = Instantiate(arrowP, transform, false);
        arrow.transform.position = transform.position + new Vector3(0, 1, 0);
        arrow.SetActive(false);
    }


    private void OnTriggerEnter(Collider collision) {
        if(collision.gameObject.CompareTag("machine")){
            colliding = true;
            col = collision;
        }
    }

    private void OnTriggerExit(Collider collision) {
        col = null;
        if(collision.gameObject.CompareTag("machine")){
            colliding = false;
            col = null;
        }
    }

}
