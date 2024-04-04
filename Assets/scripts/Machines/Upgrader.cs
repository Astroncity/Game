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

    private void OnTriggerEnter(Collider collision){
        if(collision.gameObject.tag.Equals("item")){
            float rand = UnityEngine.Random.Range(0, 100);
            foreach(ModifierChance packet in modifierChanceValues){
                if(rand < packet.chance){
                    collision.gameObject.GetComponent<ItemData>().modifiers.Add(packet.mod);
                    return;
                }
            }
        }
    }


        
}
