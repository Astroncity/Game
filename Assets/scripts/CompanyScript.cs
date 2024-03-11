using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class CompanyScript : MonoBehaviour{
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

    void Start(){
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
            transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(1.1f, 1.1f, 1.1f), 10f * Time.deltaTime);
            background.color = Color.Lerp(background.color, hoverColor, 10f * Time.deltaTime);
            Debug.Log("Hovering");
        }
        else{
            transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(1f, 1f, 1f), 10f * Time.deltaTime);
            background.color = Color.Lerp(background.color, defaultColor, 10f * Time.deltaTime);
            Debug.Log("Not Hovering");
        }
    }
    void IsPointerOverUIObject(){        
        RaycastHit hitInfo;
        if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo)){
            if(hitInfo.collider.gameObject.GetComponent<CompanyScript>() != null){
                Debug.Log("Hovering over company");
                if(hitInfo.collider.gameObject.GetComponent<CompanyScript>() == this){
                    isHovering = true;
                    return;
                }
            }
        }
        isHovering = false;
    }
}
