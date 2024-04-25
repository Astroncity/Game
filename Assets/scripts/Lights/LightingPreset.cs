using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "Lighting Preset", menuName = "ScriptableObjects/LightingPreset", order = 1)]
public class LightingPreset : ScriptableObject{
    public Gradient AmbientColor;
    public Gradient DirectionalColor;
    public Gradient FogColor;

    public Material daySkybox;
    public Material nightSkybox;
    public Material sunSetSkybox;

    public float defFogDensity = 0.0025f;

    
}
