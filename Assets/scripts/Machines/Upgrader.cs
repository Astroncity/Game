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
            double totalProbability = 0;
            foreach (ModifierChance item in modifierChanceValues){
                totalProbability += item.chance;
            }

            System.Random random = new System.Random();
            double randomNumber = random.NextDouble() * totalProbability;

            foreach (ModifierChance item in modifierChanceValues){
                randomNumber -= item.chance;
                if (randomNumber <= 0){
                    collision.gameObject.GetComponent<ItemScript>().addMod(item.mod);
                    return;
                }
            }
        }
    }


        
}
