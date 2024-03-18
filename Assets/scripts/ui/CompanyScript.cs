using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Unity.VisualScripting;
using System.Collections.Generic;


public class CompanyScript : MonoBehaviour{
    //temporarily randomize the rate and company name
    public uint rate;
    public string companyName;

    public TextMeshProUGUI nameText;
    public TextMeshProUGUI rateText;
    public TextMeshProUGUI itemText;

    public Image icon;
    public Image background;
    private Color defaultColor;
    private Color hoverColor;
    public CompanyIconHolder icons;

    private bool isHovering = false;

    private Vector3 defaultScale;
    private GameObject player;

    public static CompanyScript selected = null;
    public Confirmation confirmationScript;

    //! temporary
    private int itemCount = 0;

    void Start(){
        player = GameObject.Find("Player");
        rate = (uint)Random.Range(1, 100);
        defaultScale = transform.localScale;
        defaultColor = background.color;
        hoverColor = defaultColor / 1.2f; hoverColor.a = 1;
        icons.init();
        companyName = icons.CompanyNames[Random.Range(0, icons.CompanyNames.Count)];
        nameText.text = companyName;
        rateText.text = rate + "<color=yellow><s>g</s></color> / L";
        icon.sprite = icons.CompanyIcons[Random.Range(0, icons.CompanyIcons.Length)];
        icon.color = new Color(Random.Range(0.5f, 1f), Random.Range(0.5f, 1f), Random.Range(0.5f, 1f));
    }

    void Update(){
        if(PlayerHandler.inBuildMode) return;
        IsPointerOverUIObject();
        itemText.text = itemCount + " L";

        if(isHovering){
            transform.localScale = Vector3.Lerp(transform.localScale, defaultScale * 1.1f, 10f * Time.deltaTime);
            background.color = Color.Lerp(background.color, hoverColor, 10f * Time.deltaTime);
        }
        else{
            transform.localScale = Vector3.Lerp(transform.localScale, defaultScale, 10f * Time.deltaTime);
            background.color = Color.Lerp(background.color, defaultColor, 10f * Time.deltaTime);
        }

        if(Input.GetMouseButtonDown(0) && isHovering){
            if( PlayerHandler.money >= rate * itemCount && !PlayerHandler.onMilkRun && itemCount > 0){
                selected = this;
                transform.parent.gameObject.SetActive(false);
                confirmationScript.gameObject.SetActive(true);
            }
            else{
                background.color = Color.red;
            }
        }   
    }

    
    public void addItem(int amount){
        itemCount += amount;
        if(itemCount < 0) itemCount = 0;
    }


    public void select(){
        if(selected != this){
            Debug.Log("Weird ass error");
        }
        GameObject point = getNearestDropPoint();
        DropPoint pointS = point.GetComponent<DropPoint>(); 
        pointS.init(false, false, itemCount);
        point.GetComponent<MeshRenderer>().enabled = true;
        PlayerHandler.money -= (int)rate * itemCount;
        PlayerHandler.onMilkRun = true;
        selected = null;
    }

    void IsPointerOverUIObject(){        
        RaycastHit hitInfo;
        if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo)){
            if(hitInfo.collider.gameObject.GetComponent<CompanyScript>() != null){
                if(hitInfo.collider.gameObject.GetComponent<CompanyScript>() == this){
                    isHovering = true;
                    return;
                }
            }
        }
        isHovering = false;
    }

    GameObject getNearestDropPoint(){
        List<GameObject> points = new List<GameObject>(GameObject.FindGameObjectsWithTag("DropPoint"));

        //? filter out permanent points
        for(int i = 0; i < points.Count; i++){
            if(points[i].GetComponent<DropPoint>().permanent){
                points.RemoveAt(i);
                i--;
            }
        }
        
        float closest = Vector3.Distance(points[0].transform.position, transform.position);
        GameObject closestPoint = points[0];
        foreach(GameObject point in points){
            float distance = Vector3.Distance(point.transform.position, transform.position);
            if(distance < closest){
                closest = distance;
                closestPoint = point;
            }
        }
        Debug.Log("Closest point is " + closestPoint.name);
        return closestPoint;
    }
}
