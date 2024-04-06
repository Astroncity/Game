using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemScript : MonoBehaviour{
    public static int count = 0; //for UI display


    public TextMeshProUGUI valueText;
    public Canvas infoCanvas;

    public ItemData data;
    public int basePrice;

    private static Dictionary<Modifier, double> modifierMultipliers = new Dictionary<Modifier, double>{
        {Modifier.bronze, 1.1},
        {Modifier.silver, 1.3},
        {Modifier.gold, 1.5},
        {Modifier.diamond, 2},
    };

    public TextMeshProUGUI[] modifierTexts;

    void Start(){
        data.modifiers.Add(Modifier.bronze, 0);
        data.modifiers.Add(Modifier.silver, 0);
        data.modifiers.Add(Modifier.gold, 0);
        data.modifiers.Add(Modifier.diamond, 0);

        basePrice = data.value;
        count++;
    }

    public void initOnSpawn(){
        data.modifiers = new Dictionary<Modifier, int>();
    }


    public void displayModifiers(){
        modifierTexts[0].text = data.modifiers[Modifier.bronze].ToString() + "x";
        modifierTexts[1].text = data.modifiers[Modifier.silver].ToString() + "x";
        modifierTexts[2].text = data.modifiers[Modifier.gold].ToString() + "x";
        modifierTexts[3].text = data.modifiers[Modifier.diamond].ToString() + "x";
    }

    public int calcPrice(){
        double tempPrice = basePrice;
        foreach(KeyValuePair<Modifier, int> mod in data.modifiers){
            tempPrice += (basePrice * modifierMultipliers[mod.Key]) * mod.Value;
        }
        return (int)tempPrice;
    }

    public void addMod(Modifier mod){
        data.modifiers[mod]++;
    }

    void Update(){
        data.value = calcPrice();
        if(PlayerHandler.inBuildMode){  
            return;
        }

        handleUI();
    }


    void handleUI(){
        valueText.text = data.value.ToString() + "<color=yellow>g";
        infoCanvas.transform.rotation = Quaternion.LookRotation(infoCanvas.transform.position - Camera.main.transform.position);
        infoCanvas.transform.position = transform.position + new Vector3(0, 0.75f, 0);
        displayModifiers();
    }


    public void hold(){
        transform.position = Camera.main.gameObject.transform.position + Camera.main.gameObject.transform.forward * 2;
    }

}
