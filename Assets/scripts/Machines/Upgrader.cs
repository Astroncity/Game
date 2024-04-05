using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public struct ModifierChance{
    public Modifier mod;
    public double chance;
}
public class Upgrader : MonoBehaviour
{
    public ModifierChance[] modifierChanceValues;
    /* for now, possible values are bronze, silver, gold, diamond*/

    void OnTriggerEnter(Collider collision){
        if(collision.gameObject.tag.Equals("Item")){
            float rand = UnityEngine.Random.Range(0f, 100f);
            foreach(ModifierChance packet in modifierChanceValues){
                if(rand < packet.chance){
                    collision.gameObject.GetComponent<ItemScript>().data.modifiers.Add(packet.mod);
                    Debug.Log("Added modifier: " + packet.mod.ToString());
                    return;
                }
            }
        }
    }


        
}
