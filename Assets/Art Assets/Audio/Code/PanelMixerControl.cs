using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class PanelMixerControl : MonoBehaviour
{
    public AudioMixer mixer;
    public string musicParameterName = "Music";
    public string sfxParameterName = "SfxPlayer";
    public string sfxEnvParameterName = "SfxEnvironment";
    public Slider musicSlider;
    public Slider sfxSlider;
    public Slider sfxEnvSlider;
    /*
    private void Start()
    {
        musicSlider.value = GetSliderValueFromParameter(GetParameterValue(musicParameterName));
        sfxSlider.value = GetSliderValueFromParameter(GetParameterValue(sfxParameterName));
        sfxEnvSlider.value = GetSliderValueFromParameter(GetParameterValue(sfxEnvParameterName)); // Corregido para actualizar el parámetro correcto
    }
    */
    public void UpdateMusicParameter()
    {
        UpdateMixerParameter(musicSlider, musicParameterName);
    }

    public void UpdateSFXParameter()
    {
        UpdateMixerParameter(sfxSlider, sfxParameterName);
    }

    public void UpdateSFXEnvironmentParameter()
    {
        UpdateMixerParameter(sfxEnvSlider, sfxEnvParameterName); // Corregido para actualizar el parámetro correcto
    }

    private void UpdateMixerParameter(Slider slider, string parameterName)
    {
        float sliderValue = slider.value;
        float remappedValue = Remap(sliderValue);
        SetParameterValue(parameterName, remappedValue);
    }

    private void SetParameterValue(string parameterName, float value)
    {
        mixer.SetFloat(parameterName, value);
    }

    private float GetParameterValue(string parameterName)
    {
        float value;
        mixer.GetFloat(parameterName, out value);
        return value;
    }

    private float Remap(float value)
    {
        if (value <= -8f) return Mathf.Lerp(-80, 0, value * 1); 
        else return Mathf.Lerp(value*10, 10, (value - 1f) * 10); 
    }

    private float GetSliderValueFromParameter(float parameterValue)
    {
        if (parameterValue <= -8) return Mathf.InverseLerp(-80, 0, parameterValue) / 1; 
        else return 1f + Mathf.InverseLerp(parameterValue*10, 10, parameterValue) * 1f; 
    }
}
