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

    public ItemData(int value, List<Modifier> modifiers){
        this.value = value;
        this.modifiers = modifiers;
    }
}

