using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Money : MonoBehaviour{
    public TextMeshProUGUI text;        


    void Update(){
        text.text = PlayerHandler.money.ToString() + "<color=yellow>g";
    }
}
