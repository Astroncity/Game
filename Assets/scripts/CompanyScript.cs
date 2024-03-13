using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class CompanyScript : MonoBehaviour{
    //temporarily randomize the rate and company name
    public uint rate;
    public string companyName;

    public TextMeshProUGUI nameText;
    public TextMeshProUGUI rateText;

    public Image icon;
    public Image background;
    private Color defaultColor;
    private Color hoverColor;
    public CompanyIconHolder icons;

    private bool isHovering = false;

    private Vector3 defaultScale;
    private GameObject player;

    //! temporary
    private uint itemCount = 50;

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
        IsPointerOverUIObject();

        if(isHovering){
            transform.localScale = Vector3.Lerp(transform.localScale, defaultScale * 1.1f, 10f * Time.deltaTime);
            background.color = Color.Lerp(background.color, hoverColor, 10f * Time.deltaTime);
        }
        else{
            transform.localScale = Vector3.Lerp(transform.localScale, defaultScale, 10f * Time.deltaTime);
            background.color = Color.Lerp(background.color, defaultColor, 10f * Time.deltaTime);
        }

        if(Input.GetMouseButtonDown(0) && isHovering){
            if(player.GetComponent<PlayerHandler>().money >= rate * itemCount && !player.gameObject.GetComponent<PlayerHandler>().onMilkRun){
                GameObject point = getNearestDropPoint();
                point.GetComponent<DropPoint>().itemCount += itemCount;
                point.GetComponent<DropPoint>().active = true;
                point.GetComponent<MeshRenderer>().enabled = true;
                player.GetComponent<PlayerHandler>().money -= rate * itemCount;
                player.gameObject.GetComponent<PlayerHandler>().onMilkRun = true;
            }
            else{
                background.color = Color.red;
            }
        }   
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
        GameObject[] points = GameObject.FindGameObjectsWithTag("DropPoint");
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
