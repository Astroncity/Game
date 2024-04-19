using System.Globalization;
using UnityEngine;

[ExecuteAlways]
public class LightingManager : MonoBehaviour{
    [SerializeField] private Light DirectionalLight;
    [SerializeField] private LightingPreset Preset;
    [Range(0, 24)] public float TimeOfDay;
    public float speedMult = 1;


    private Material daySkyboxMaterial;
    private Material nightSkyboxMaterial;
    private Material sunSetSkyboxMaterial;

    private SkyboxMaterial daylightMat;
    private SkyboxMaterial nightMat;
    private SkyboxMaterial sunSetMat;

    

    public struct SkyGradient{
        public Color top;
        public Color bottom;
        public double exponent;
    }

    public struct SkyboxMaterial{
        public Color sunDisc;
        public Color sunHalo;
        public Color HorizonLine;
        public SkyGradient skyGradient;

        public static SkyboxMaterial Lerp(SkyboxMaterial a, SkyboxMaterial b, float t){
            SkyboxMaterial result = new SkyboxMaterial();
            result.sunDisc = Color.Lerp(a.sunDisc, b.sunDisc, t);
            result.sunHalo = Color.Lerp(a.sunHalo, b.sunHalo, t);
            result.HorizonLine = Color.Lerp(a.HorizonLine, b.HorizonLine, t);
            result.skyGradient.top = Color.Lerp(a.skyGradient.top, b.skyGradient.top, t);
            result.skyGradient.bottom = Color.Lerp(a.skyGradient.bottom, b.skyGradient.bottom, t);
            result.skyGradient.exponent = a.skyGradient.exponent * (1 - t) + b.skyGradient.exponent * t;
            return result;
        }
    }

    public void Start(){
        daySkyboxMaterial = Instantiate(Preset.daySkybox);
        nightSkyboxMaterial = Instantiate(Preset.nightSkybox);
        sunSetSkyboxMaterial = Instantiate(Preset.sunSetSkybox);

        daylightMat = skyMatFromShader(daySkyboxMaterial);
        nightMat = skyMatFromShader(nightSkyboxMaterial);
        sunSetMat = skyMatFromShader(sunSetSkyboxMaterial);
    }


    private void Update(){
        if (Application.isPlaying){
            TimeOfDay += Time.deltaTime / 60f * speedMult;
            TimeOfDay %= 24; 
            UpdateLighting(TimeOfDay / 24f);
        }
        else{
            UpdateLighting(TimeOfDay / 24f);
        }

        setSkybox();    
    }


    public static SkyboxMaterial skyMatFromShader(Material box){
        SkyboxMaterial skyMat = new SkyboxMaterial();
        skyMat.sunDisc = box.GetColor("_SunDiscColor");
        skyMat.sunHalo = box.GetColor("_SunHaloColor");
        skyMat.HorizonLine = box.GetColor("_HorizonLineColor");
        skyMat.skyGradient.top = box.GetColor("_SkyGradientTop");
        skyMat.skyGradient.bottom = box.GetColor("_SkyGradientBottom");
        skyMat.skyGradient.exponent = box.GetFloat("_SkyGradientExponent");
        return skyMat;
    }


    public Material ShaderFromSkyMat(SkyboxMaterial skyMat){
        Material box = daySkyboxMaterial;
        box.SetColor("_SunDiscColor", skyMat.sunDisc);
        box.SetColor("_SunHaloColor", skyMat.sunHalo);
        box.SetColor("_HorizonLineColor", skyMat.HorizonLine);
        box.SetColor("_SkyGradientTop", skyMat.skyGradient.top);
        box.SetColor("_SkyGradientBottom", skyMat.skyGradient.bottom);
        box.SetFloat("_SkyGradientExponent", (float)skyMat.skyGradient.exponent);
        return box;
    }


    public void setSkybox(){
        SkyboxMaterial currentMat = skyMatFromShader(RenderSettings.skybox);

        if(TimeOfDay > 5 && TimeOfDay < 18){
            currentMat = SkyboxMaterial.Lerp(currentMat, daylightMat, 0.01f / 2 / 50);
            RenderSettings.skybox = ShaderFromSkyMat(currentMat);
        }

        else{
            currentMat = SkyboxMaterial.Lerp(currentMat, nightMat, 0.01f / 2 / 50);
            RenderSettings.skybox = ShaderFromSkyMat(currentMat);
        }

    }


    private void UpdateLighting(float timePercent){
        RenderSettings.ambientLight = Preset.AmbientColor.Evaluate(timePercent);
        RenderSettings.fogColor = Preset.FogColor.Evaluate(timePercent);

        if(DirectionalLight != null){
            DirectionalLight.color = Preset.DirectionalColor.Evaluate(timePercent);
            DirectionalLight.transform.localRotation = Quaternion.Euler(new Vector3((timePercent * 360f) - 90f, 170f, 0));
        }
    }
}