using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Storage : MonoBehaviour{
    public static Stack<ItemData> items = new Stack<ItemData>();

    public Canvas canvas;
    public TextMeshProUGUI itemText;

    private void Update(){
        itemText.text = items.Count.ToString();
        if(Camera.main != null){
            canvas.transform.rotation = Quaternion.LookRotation(canvas.transform.position - Camera.main.transform.position);
        }
    }

}
