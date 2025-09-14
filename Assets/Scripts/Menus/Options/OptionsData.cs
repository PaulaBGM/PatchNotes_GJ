using UnityEngine;

[System.Serializable]
public class OptionsData
{
    public float brightness = 1f;
    public float musicVolume = 1f;
    public float sfxVolume = 1f;

    public void Load()
    {
        brightness = PlayerPrefs.GetFloat("brightness", 1f);
        musicVolume = PlayerPrefs.GetFloat("musicVolume", 1f);
        sfxVolume = PlayerPrefs.GetFloat("sfxVolume", 1f);
    }

    public void Save()
    {
        PlayerPrefs.SetFloat("brightness", brightness);
        PlayerPrefs.SetFloat("musicVolume", musicVolume);
        PlayerPrefs.SetFloat("sfxVolume", sfxVolume);
        PlayerPrefs.Save();
    }
}
