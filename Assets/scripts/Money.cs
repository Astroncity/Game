using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Money : MonoBehaviour{
    public PlayerHandler player;
    public TextMeshProUGUI text;        


    void Update(){
        text.text = player.money.ToString() + "<color=yellow>g";
    }
}
