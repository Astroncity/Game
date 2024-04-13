using UnityEngine;
using UnityEngine.UI;

public class Confirmation : MonoBehaviour{
    public GameObject vendorUI;


    public void yes(){
        if(CompanyScript.selected == null) return;
        CompanyScript.selected.select();
        vendorUI.SetActive(true);
        gameObject.SetActive(false);
    }

    public void back(){
        CompanyScript.selected = null;
        vendorUI.SetActive(true);
        gameObject.SetActive(false);
    }
}
