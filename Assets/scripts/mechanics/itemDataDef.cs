using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum Modifier{
    bronze,
    silver,
    gold,
    diamond
}

[Serializable]
public struct ItemData{
    public int value;
    public Dictionary<Modifier, int> modifiers;
    public string type;

    public ItemData(int value, string type, Dictionary<Modifier, int> modifiers){
        this.value = value;
        this.modifiers = modifiers;
        this.type = type;
    }
}

