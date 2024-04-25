using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public struct ModifierChance{
    public Modifier mod;
    public double chance;
}
public class Upgrader : MonoBehaviour
{
    public ModifierChance[] modifierChanceValues;

    public TextMeshProUGUI[] chanceTexts;
    public int flatIncrease;
    public TextMeshProUGUI flatIncreaseText;
    /* for now, possible values are bronze, silver, gold, diamond*/

    void Update(){
        handleUI();
    }

    void handleUI(){
        for (int i = 0; i < modifierChanceValues.Length; i++){
            chanceTexts[i].text = modifierChanceValues[i].chance.ToString() + "%";
        }
        flatIncreaseText.text = "+" + flatIncrease.ToString() + "<color=yellow>g";
    }

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
                    collision.gameObject.GetComponent<ItemScript>().data.value += flatIncrease;
                    collision.gameObject.GetComponent<ItemScript>().addMod(item.mod);
                    return;
                }
            }
        }
    }


        
}
