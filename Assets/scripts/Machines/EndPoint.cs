using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EndPoint : MonoBehaviour{
    
    private void OnCollisionEnter(Collision collision){
        if(collision.gameObject.tag.Equals("Item")){
            Storage.items.Push(collision.gameObject.GetComponent<ItemScript>().data);
            Destroy(collision.gameObject);
        }
    }
}
