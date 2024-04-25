using System.Globalization;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.LookDev;

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

    
    public class SunDisc{
        public Color color;
        public float multiplier;
        public float exponent;
    }

    public class SunHalo{
        public Color color;
        public float exponent;
        public float contribution;
    }

    public class HorizonLine : SunHalo{}

    public class SkyGradient{
        public Color top;
        public Color bottom;
        public float exponent;
    }






    public struct SkyboxMaterial{
        public SunDisc sunDisc;
        public SunHalo sunHalo;
        public HorizonLine HorizonLine;
        public SkyGradient skyGradient;

        public static SkyboxMaterial Lerp(SkyboxMaterial a, SkyboxMaterial b, float t){

            if(a.IsUnityNull() || b.IsUnityNull()){
                Debug.Log("SkyboxMaterial is null");
                throw new System.NullReferenceException("balls");
            }


            SkyboxMaterial result = new SkyboxMaterial
            {
                sunDisc = new SunDisc(),
                sunHalo = new SunHalo(),
                HorizonLine = new HorizonLine(),
                skyGradient = new SkyGradient()
            };
            result.sunDisc.color = Color.Lerp(a.sunDisc.color, b.sunDisc.color, t);
            result.sunDisc.multiplier = Mathf.Lerp(a.sunDisc.multiplier, b.sunDisc.multiplier, t);
            result.sunDisc.exponent = Mathf.Lerp(a.sunDisc.exponent, b.sunDisc.exponent, t);

            result.sunHalo.color = Color.Lerp(a.sunHalo.color, b.sunHalo.color, t);
            result.sunHalo.exponent = Mathf.Lerp(a.sunHalo.exponent, b.sunHalo.exponent, t);
            result.sunHalo.contribution = Mathf.Lerp(a.sunHalo.contribution, b.sunHalo.contribution, t);

            result.HorizonLine.color = Color.Lerp(a.HorizonLine.color, b.HorizonLine.color, t);
            result.HorizonLine.exponent = Mathf.Lerp(a.HorizonLine.exponent, b.HorizonLine.exponent, t);
            result.HorizonLine.contribution = Mathf.Lerp(a.HorizonLine.contribution, b.HorizonLine.contribution, t);

            result.skyGradient.top = Color.Lerp(a.skyGradient.top, b.skyGradient.top, t);
            result.skyGradient.bottom = Color.Lerp(a.skyGradient.bottom, b.skyGradient.bottom, t);
            result.skyGradient.exponent = Mathf.Lerp(a.skyGradient.exponent, b.skyGradient.exponent, t);
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
        RenderSettings.skybox = daySkyboxMaterial;
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


    public static SunDisc sunDiscFromShader(Material box){
        SunDisc sunDisc = new SunDisc
        {
            color = box.GetColor("_SunDiscColor"),
            multiplier = box.GetFloat("_SunDiscMultiplier"),
            exponent = box.GetFloat("_SunDiscExponent")
        };
        return sunDisc;
    }


    public static SunHalo sunHaloFromShader(Material box){
        SunHalo sunHalo = new SunHalo
        {
            color = box.GetColor("_SunHaloColor"),
            exponent = box.GetFloat("_SunHaloExponent"),
            contribution = box.GetFloat("_SunHaloContribution")
        };
        return sunHalo;
    }

    public static HorizonLine horizonLineFromShader(Material box){
        HorizonLine horizonLine = new HorizonLine
        {
            color = box.GetColor("_HorizonLineColor"),
            exponent = box.GetFloat("_HorizonLineExponent"),
            contribution = box.GetFloat("_HorizonLineContribution")
        };
        return horizonLine;
    }

    public static SkyGradient skyGradientFromShader(Material box){
        SkyGradient skyGradient = new SkyGradient
        {
            top = box.GetColor("_SkyGradientTop"),
            bottom = box.GetColor("_SkyGradientBottom"),
            exponent = box.GetFloat("_SkyGradientExponent")
        };
        return skyGradient;
    }
    
    public static SkyboxMaterial skyMatFromShader(Material box){
        SkyboxMaterial skyMat = new SkyboxMaterial
        {
            sunDisc = sunDiscFromShader(box),
            sunHalo = sunHaloFromShader(box),
            HorizonLine = horizonLineFromShader(box),
            skyGradient = skyGradientFromShader(box)
        };


        return skyMat;
    }


    public Material ShaderFromSkyMat(SkyboxMaterial skyMat){
        Material box = daySkyboxMaterial;

        box.SetColor("_SunDiscColor", skyMat.sunDisc.color);
        box.SetFloat("_SunDiscMultiplier", skyMat.sunDisc.multiplier);
        box.SetFloat("_SunDiscExponent", skyMat.sunDisc.exponent);

        box.SetColor("_SunHaloColor", skyMat.sunHalo.color);
        box.SetFloat("_SunHaloExponent", skyMat.sunHalo.exponent);
        box.SetFloat("_SunHaloContribution", skyMat.sunHalo.contribution);

        box.SetColor("_HorizonLineColor", skyMat.HorizonLine.color);
        box.SetFloat("_HorizonLineExponent", skyMat.HorizonLine.exponent);
        box.SetFloat("_HorizonLineContribution", skyMat.HorizonLine.contribution);

        box.SetColor("_SkyGradientTop", skyMat.skyGradient.top);
        box.SetColor("_SkyGradientBottom", skyMat.skyGradient.bottom);
        box.SetFloat("_SkyGradientExponent", skyMat.skyGradient.exponent);
        return box;
    }


    public void setSkybox(){
        SkyboxMaterial currentMat;
        float t;
        float mult = 1f;
        if(TimeOfDay >= 6 && TimeOfDay <= 8){ // Sunrise
            t = (TimeOfDay - 6) / (8 - 6);
            t *= mult;
            currentMat = SkyboxMaterial.Lerp(nightMat, sunSetMat, t);
            RenderSettings.fogDensity = Mathf.Lerp(RenderSettings.fogDensity, Preset.defFogDensity, t);
        }
        else if(TimeOfDay > 8 && TimeOfDay < 16){ // Day
            t = (TimeOfDay - 8) / (16 - 8);
            t *= mult;
            currentMat = SkyboxMaterial.Lerp(sunSetMat, daylightMat, t);
            RenderSettings.fogDensity = Mathf.Lerp(RenderSettings.fogDensity, Preset.defFogDensity, t);
        }
        else if(TimeOfDay >= 16 && TimeOfDay <= 18){ // Sunset
            t = (TimeOfDay - 16) / (18 - 16);
            t *= mult;
            currentMat = SkyboxMaterial.Lerp(daylightMat, sunSetMat, t);
        }
        else{ // Night
            if(TimeOfDay > 18)
                t = (TimeOfDay - 18) / (24 - 18);
            else
                t = TimeOfDay / 6;
            t *= mult;
            currentMat = SkyboxMaterial.Lerp(sunSetMat, nightMat, t);
            RenderSettings.fogDensity = Mathf.Lerp(RenderSettings.fogDensity, 0.01f, t);
        }

        RenderSettings.skybox = ShaderFromSkyMat(currentMat);
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