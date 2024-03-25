using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum Modifier{
    bronze,
    silver,
    gold,
    diamond
}

public struct ItemData{
    public int value;
    public List<Modifier> modifiers;
    public string type;

    public ItemData(int value, string type, List<Modifier> modifiers){
        this.value = value;
        this.modifiers = modifiers;
        this.type = type;
    }
}

