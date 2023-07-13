using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class UserSettings : MonoBehaviour
{
    [ReadOnly,SerializeField] public static UserSettings Instance;
    private void Awake()
    {
        if(Instance is not null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(this);

        LoadAllPrefs();
        if(sensitivityHorizontal == 0)
        {
            sensitivityHorizontal = 1;
        }
        if(sensitivityVertical == 0)
        {
            sensitivityVertical = 1;
        }
    }

    public void LoadAllPrefs()
    {
        LoadPrefsAudio();
        LoadPrefsGraphics();
        LoadPrefsControls();
    }

    public void SaveAllPrefs()
    {
        SetPrefsAudio();
        SetPrefsGraphics();
        SetPrefsControls();
        PlayerPrefs.Save();
    }

    #region Audio
    private float volumeMaster;
    private float volumeMenu;
    private float volumeMusic;
    private float volumeEffects;
    private float volumeAmbience;

    public void LoadPrefsAudio()
    {
        volumeMaster = PlayerPrefs.GetFloat("VolumeMaster");
        volumeMenu = PlayerPrefs.GetFloat("VolumeMenu");
        volumeMusic = PlayerPrefs.GetFloat("VolumeMusic");
        volumeEffects = PlayerPrefs.GetFloat("VolumeEffects");
        volumeAmbience = PlayerPrefs.GetFloat("VolumeAmbience");
    }
    public void SetPrefsAudio()
    {
        PlayerPrefs.SetFloat("VolumeMaster", volumeMaster);
        PlayerPrefs.SetFloat("VolumeMenu", volumeMenu);
        PlayerPrefs.SetFloat("VolumeMusic", volumeMusic);
        PlayerPrefs.SetFloat("VolumeAmbience", volumeAmbience);
    }

    public float VolumeMaster
    {
        get
        {
            return volumeMaster;
        }
        set
        {
            volumeMaster = value;
            AkSoundEngine.SetRTPCValue("MasterVolume", volumeMaster);
        }
    }
    public float VolumeMenu
    {
        get
        {
            return volumeMenu;
        }
        set
        {
            volumeMenu = value;
            AkSoundEngine.SetRTPCValue("MenuVolume", volumeMenu);
        }
    }
    public float VolumeMusic
    {
        get
        {
            return volumeMusic;
        }
        set
        {
            volumeMusic = value;
            AkSoundEngine.SetRTPCValue("MusicVolume", volumeMusic);
        }
    }
    public float VolumeEffects
    {
        get
        {
            return volumeEffects;
        }
        set
        {
            volumeEffects = value;
            AkSoundEngine.SetRTPCValue("SFXVolume", volumeEffects);
        }
    }
    public float VolumeAmbience
    {
        get
        {
            return volumeAmbience;
        }
        set
        {
            volumeAmbience = value;
            AkSoundEngine.SetRTPCValue("AmbienceVolume", volumeAmbience);
        }
    }
    #endregion


    #region Graphics
    public enum QualityLevels
    {
        High = 0,
        Medium = 1,
        Low = 2
    }
    private QualityLevels graphicsQuality;

    public void LoadPrefsGraphics()
    {
        graphicsQuality = (QualityLevels)PlayerPrefs.GetInt("GraphicsQuality");
    }
    public void SetPrefsGraphics()
    {
        PlayerPrefs.SetInt("GraphicsQuality", (int)graphicsQuality);
    }

    public int GraphicsQuality
    {
        get
        {
            return (int)graphicsQuality;
        }
        set
        {
            graphicsQuality = (QualityLevels)value;
        }
    }
    #endregion


    #region Controls
    private float sensitivityVertical;
    private float sensitivityHorizontal;
    private KeyCode keybindKunai;
    private KeyCode keybindSmokebomb;
    private KeyCode keybindDash;

    public void LoadPrefsControls()
    {
        sensitivityVertical = PlayerPrefs.GetFloat("ControlsSensitivityVertical");
        sensitivityHorizontal = PlayerPrefs.GetFloat("ControlsSensitivityHorizontal");
        keybindKunai = (KeyCode)PlayerPrefs.GetInt("KeybindKunai");
        keybindSmokebomb = (KeyCode)PlayerPrefs.GetInt("KeybindSmokebomb");
        keybindDash = (KeyCode)PlayerPrefs.GetInt("KeybindDash");
    }
    public void SetPrefsControls()
    {
        PlayerPrefs.SetFloat("ControlsSensitivityVertical", sensitivityVertical);
        PlayerPrefs.SetFloat("ControlsSensitivityHorizontal", sensitivityHorizontal);
        PlayerPrefs.SetInt("KeybindKunai", (int)keybindKunai);
        PlayerPrefs.SetInt("KeybindSmokebomb", (int)keybindSmokebomb);
        PlayerPrefs.SetInt("KeybindDash", (int)keybindDash);
    }

    public float SensitivityVertical
    {
        get
        {
            return sensitivityVertical;
        }
        set
        {
            sensitivityVertical = value;
        }
    }
    public string SensitivityVerticalString
    {
        get
        {
            return SensitivityVertical.ToString();
        }
        set
        {
            SensitivityVertical = Convert.ToInt32(value);
        }
    }
    public float SensitivityHorizontal
    {
        get
        {
            return sensitivityHorizontal;
        }
        set
        {
            sensitivityHorizontal = value;
        }
    }
    public string SensitivityHorizontalString
    {
        get
        {
            return SensitivityHorizontal.ToString();
        }
        set
        {
            SensitivityHorizontal = Convert.ToInt32(value);
        }
    }
    public KeyCode KeybindKunai
    {
        get
        {
            return keybindKunai;
        }
        set
        {
            keybindKunai = value;
        }
    }
    public KeyCode KeybindSmokebomb
    {
        get
        {
            return keybindSmokebomb;
        }
        set
        {
            keybindSmokebomb = value;
        }
    }
    public KeyCode KeybindDash
    {
        get
        {
            return keybindDash;
        }
        set
        {
            keybindDash = value;
        }
    }
    #endregion

    private void OnApplicationQuit()
    {
        SaveAllPrefs();
    }
}
