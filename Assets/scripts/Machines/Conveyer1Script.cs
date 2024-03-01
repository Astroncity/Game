using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Conveyer1Script : BaseMachine
{
    // Start is called before the first frame update
    void Start()
    {
        init();
        Debug.Log(data.price);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
