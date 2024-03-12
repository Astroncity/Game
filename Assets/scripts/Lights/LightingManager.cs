using UnityEngine;

[ExecuteAlways]
public class LightingManager : MonoBehaviour{
    [SerializeField] private Light DirectionalLight;
    [SerializeField] private LightingPreset Preset;
    [Range(0, 24)] public float TimeOfDay;


    private void Update(){
        if (Application.isPlaying){
            TimeOfDay += Time.deltaTime;
            TimeOfDay %= 24; 
            UpdateLighting(TimeOfDay / 24f);
        }
        else{
            UpdateLighting(TimeOfDay / 24f);
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