using UnityEngine;
using UnityEngine.UI;

public class Crosshair : MonoBehaviour{
    // Start is called before the first frame update
    private Color canHoldColor;
    private Color isHoldingColor;
    private Color defaultColor;

    private Vector3 defaultScale;
    private Vector3 activeScale;

    public Image crosshairImage;
    void Start(){
        defaultColor = crosshairImage.color;
        canHoldColor = createColor(255, 209, 0, 255);
        isHoldingColor = createColor(255, 165, 167, 255);

        crosshairImage = GetComponent<Image>();
        defaultScale = crosshairImage.transform.localScale;
        activeScale = defaultScale * 1.5f;
    }


    void Update(){
        if(PlayerHandler.holdingItem){
            crosshairImage.color = Color.Lerp(crosshairImage.color, isHoldingColor, 10f * Time.deltaTime);
            crosshairImage.transform.localScale = Vector3.Lerp(crosshairImage.transform.localScale, activeScale, 10f * Time.deltaTime);
        }
        else if(PlayerHandler.canHoldItem){
            crosshairImage.color = Color.Lerp(crosshairImage.color, canHoldColor, 10f * Time.deltaTime);
            crosshairImage.transform.localScale = Vector3.Lerp(crosshairImage.transform.localScale, activeScale, 10f * Time.deltaTime);
        }
        else{
            crosshairImage.color = Color.Lerp(crosshairImage.color, defaultColor, 10f * Time.deltaTime);
            crosshairImage.transform.localScale = Vector3.Lerp(crosshairImage.transform.localScale, defaultScale, 10f * Time.deltaTime);
        }
    }


    public static Color createColor(float r, float g, float b, float a){
        return new Color(r / 255, g / 255, b / 255, a / 255);
    }
}
