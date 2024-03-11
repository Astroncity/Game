using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CompanyIconHolder : MonoBehaviour{
    public Sprite[] CompanyIcons;
    public List<string> CompanyNames;

    public void init(){
        // load company names from txt file
        TextAsset txt = (TextAsset)Resources.Load("companyNamesList", typeof(TextAsset));
        string content = txt.text;
        string[] lines = content.Split(new string[] { "\n" }, StringSplitOptions.None);
        foreach(string line in lines){
            CompanyNames.Add(line);
        }
    }
}
